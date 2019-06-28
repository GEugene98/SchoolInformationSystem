import { Component, OnInit, Input, ElementRef, ViewChild } from '@angular/core';
import { GeneralSchedule } from '../../../../shared/models/general-schedule.model';
import { isUserInRole, User } from '../../../../shared/models/user';
import { UserState } from '../../../../shared/states/user.state';
import { ScheduleService } from '../../../services/schedule.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Action } from '../../../../shared/models/action.model';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { Ticket } from '../../../../shared/models/ticket.model';
import { Time } from '../../../../shared/models/time.model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-month',
  templateUrl: './month.component.html',
  styleUrls: ['./month.component.css']
})
export class MonthComponent implements OnInit {

  confirmDate: Date;
  acceptDate: Date;

  range: Date[];
  bsConfig: any;
  generalSchedule: GeneralSchedule;

  @ViewChild("selectDate") selectDateModal: ElementRef;

  modalRef: BsModalRef;

  bufferedAction: Action;
  newTicket: Ticket;

  allResponsibles;
  selectedResponsibles: User[];
  allConfForms;
  selectedConfFormId: number;
  selectedName: string;
  showMine: boolean = false;


  constructor(private schedule: ScheduleService,
    public userState: UserState,
    private ngxService: NgxUiLoaderService,
    private messageService: MessageService,
    private modalService: BsModalService, private dictionary: DictionaryService) {
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
    this.range = new Array<Date>();
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.ngxService.start();
    try {
      this.generalSchedule = await this.schedule.getScheduleForPeriod(this.range[0], this.range[1], this.showMine);
      this.allResponsibles = this.dictionary.getResponsibles();
      this.allConfForms = this.dictionary.getConfForms();
    } catch (e) {
    }
    finally {
      this.ngxService.stop();
    }

  }

  filterMine() {
    //if (this.showMine) {
    //  this.generalSchedule.days.forEach(d => {
    //    d.actions = d.actions.filter(a => a.responsibles.indexOf())
    //  });
    //}
  }

  getDocument() {
    window.open(`/api/Report/ForPeriod?` +
      `startDay=${this.range[0].getDate()}&startMonth=${this.range[0].getMonth() + 1}&startYear=${this.range[0].getFullYear()}`
      + `&endDay=${this.range[1].getDate()}&endMonth=${this.range[1].getMonth() + 1}&endYear=${this.range[1].getFullYear()}`
      + `&confDay=${this.confirmDate.getDate()}&confMonth=${this.confirmDate.getMonth() + 1}&confYear=${this.confirmDate.getFullYear()}`
      + `&acpDay=${this.acceptDate.getDate()}&acpMonth=${this.acceptDate.getMonth() + 1}&acpYear=${this.acceptDate.getFullYear()}`);

    this.closeModal();
  }

  selectDateOpenModal() {
    this.openModal(this.selectDateModal);
  }

  checkRole(user: User, role: string) {
    return isUserInRole(user, role);
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  copy(action: Action) {
    action.responsibles.forEach(r => r.fullName = `${r.lastName} ${r.firstName[0]}. ${r.surName[0]}.`);
    this.bufferedAction = Object.assign({}, action);
    this.bufferedAction.date = new Date(this.bufferedAction.date.toString()); //Костыль для ngx-datepicker'а
    this.newTicket = new Ticket();
    this.newTicket.action = this.bufferedAction;
    this.newTicket.start = new Time();
    this.newTicket.date = new Date(action.date.toString());
  }

  async saveTicket() {
    try {
      await this.schedule.addTicket(this.newTicket);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Запись добавлена в планинг", life: 5000 });
      this.loadData();
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
 
  }

  async saveAction() {
    try {
      await this.schedule.editAction(this.bufferedAction);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Изменения сохранены", life: 5000 });
      this.loadData();
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async deleteAction() {
    try {
      await this.schedule.deleteAction(this.bufferedAction);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Мероприятие удалено", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  currentWeek() {

    this.range = new Array<Date>();

    var monday = this.getMondayOfCurrentWeek(new Date());
    var sunday = this.getSundayOfCurrentWeek(new Date());

    this.range.push(monday);
    this.range.push(sunday);
    this.loadData();
  }

  getMondayOfCurrentWeek(d) {
    var day = d.getDay();
    return new Date(d.getFullYear(), d.getMonth(), d.getDate() + (day == 0 ? -6 : 1) - day);
  }
  getSundayOfCurrentWeek(d) {
    var day = d.getDay();
    return new Date(d.getFullYear(), d.getMonth(), d.getDate() + (day == 0 ? 0 : 7) - day);
  }

  currentMonth() {
    this.range = new Array<Date>();
    var date = new Date();
    var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

    this.range.push(firstDay);
    this.range.push(lastDay);

    this.loadData();
  }
}
