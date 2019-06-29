import { Component, OnInit, Input } from '@angular/core';
import { Day } from '../../../../shared/models/general-schedule.model';
import { Action } from '../../../../shared/models/action.model';
import { ScheduleService } from '../../../services/schedule.service';
import { User, isUserInRole } from '../../../../shared/models/user';
import { UserState } from '../../../../shared/states/user.state';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Ticket } from '../../../../shared/models/ticket.model';
import { Time } from '../../../../shared/models/time.model';


@Component({
  selector: 'app-day',
  templateUrl: './day.component.html',
  styleUrls: ['./day.component.css']
})
export class DayComponent implements OnInit {

  bsConfig: any;
  date: Date = new Date();
  actions: Action[];
  day: Day;
  modalRef: BsModalRef;
  showMine: boolean = false;

  bufferedAction: Action;
  newTicket: Ticket;

  constructor(private schedule: ScheduleService, public userState: UserState,
    private ngxService: NgxUiLoaderService,
    private messageService: MessageService,
    private modalService: BsModalService,
  ) {
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.ngxService.start();
    try {
      this.day = await this.schedule.getScheduleForDay(this.date, this.showMine);
      this.actions = new Array<Action>();
      this.day.actions.forEach(a => {
        this.actions.push(a);
      });
    } catch (e) {
      console.log(e);
    }
    finally {
      this.ngxService.stop();
    }
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
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Запись добавлена в органайзер", life: 5000 });
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
}
