import { Component } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { AcademicYear } from '../../shared/models/academic-year.model';
import { Activity } from '../../shared/models/activity.model';
import { ScheduleService } from '../services/schedule.service';
import { Title } from '@angular/platform-browser';
import { UserState } from '../../shared/states/user.state';
import * as _ from 'lodash';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-my-schedules-component',
  templateUrl: './my-schedules.component.html',
  styleUrls: ['./my-schedules.component.css']
})
export class MySchedules {
  modalRef: BsModalRef;
  allAcademicYears: AcademicYear[];
  academicYearsToShow: AcademicYear[];
  allActivities: Activity[];
  selectedAcademicYearId: number;
  name: string;
  selectedActivityId: number;

  otherSchedules : any;

  constructor(private modalService: BsModalService,
    private dictionary: DictionaryService,
    private messageService: MessageService,
    public userState: UserState,
    private titleService: Title,
    public schedule: ScheduleService) {
    this.loadData();
    this.selectedActivityId = this.selectedAcademicYearId = 0;
    this.titleService.setTitle('Мои планы');
  }

  async loadData() {
    this.allActivities = await this.dictionary.getActivities();
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    await this.loadSchedules();

    if (this.userState.currentUser.state.canSeeAllSchedules) {
      let response = await this.schedule.getOtherWorkSchedules();
      let groupped = [];
      response.forEach(s => {
        let name = s[0].user.lastName + ' ' + s[0].user.firstName[0] + '. ' + s[0].user.surName[0] + '. ';
        groupped.push({ name: name, schedules: _.groupBy(s, 'academicYear.name') });
       });
      this.otherSchedules = groupped;
    }
  }


  getKeys(obj) {
    return Object.keys(obj)
  }

  async loadSchedules() {
    var schedules = await this.schedule.getMyWorkSchedules();

    this.allAcademicYears.forEach(ay => {
      ay.workSchedules = schedules.filter(s => s.academicYear.id == ay.id)
    });

    this.academicYearsToShow = this.allAcademicYears.filter(a => a.workSchedules.length > 0);
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  async createSchedule() {
    try {
      await this.schedule.addWorkSchedule(this.selectedAcademicYearId, this.selectedActivityId, this.name);
      this.loadSchedules();
      this.modalRef.hide();
      this.selectedActivityId = this.selectedAcademicYearId = 0; this.name = "";

    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }
}
