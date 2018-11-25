import { Component, OnInit, Input } from '@angular/core';
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

  range: Date[];
  bsConfig: any;
  generalSchedule: GeneralSchedule;

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
      + `&endDay=${this.range[1].getDate()}&endMonth=${this.range[1].getMonth() + 1}&endYear=${this.range[1].getFullYear()}`);
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
    this.newTicket = new Ticket();
    this.newTicket.action = this.bufferedAction;
    this.newTicket.start = new Time();
    this.newTicket.date = action.date;
  }

  async saveTicket() {
    try {
      await this.schedule.addTicket(this.newTicket);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Запись добавлена в циклограмму", life: 5000 });
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
    var curr = new Date; // get current date
    var first = curr.getDate() - curr.getDay() + 1; // First day is the day of the month - the day of the week + 1 to start from Monday
    var last = first + 6; // last day is the first day + 6

    var firstDay = new Date(curr.setDate(first));
    var lastDay = new Date(curr.setDate(last));

    this.range.push(firstDay);
    this.range.push(lastDay);

    this.loadData();
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
