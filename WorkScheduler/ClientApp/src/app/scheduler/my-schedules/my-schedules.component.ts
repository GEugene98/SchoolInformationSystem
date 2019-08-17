import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { AcademicYear } from '../../shared/models/academic-year.model';
import { Activity, Color } from '../../shared/models/activity.model';
import { WorkSchedule } from '../../shared/models/work-schedule.model';
import { MessageService } from 'primeng/components/common/messageservice';
import { Message } from 'primeng/api';
import { ScheduleService } from '../services/schedule.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-my-schedules-component',
  templateUrl: './my-schedules.component.html',
  styleUrls: ['./my-schedules.component.scss']
})
export class MySchedules {
  modalRef: BsModalRef;
  allAcademicYears: AcademicYear[];
  academicYearsToShow: AcademicYear[];
  allActivities: Activity[];
  selectedAcademicYearId: number;
  name: string;
  selectedActivityId: number;

  constructor(private modalService: BsModalService,
    private dictionary: DictionaryService,
    private messageService: MessageService,
    private titleService: Title,
    private schedule: ScheduleService) {
    this.loadData();
    this.selectedActivityId = this.selectedAcademicYearId = 0;
    this.titleService.setTitle('Мои планы');
  }

  async loadData() {
    this.allActivities = await this.dictionary.getActivities();
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    await this.loadSchedules();
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
