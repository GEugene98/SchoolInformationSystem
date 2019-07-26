import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap';
import { ScheduleService } from '../services/schedule.service';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { MessageService } from 'primeng/api';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { UserState } from '../../shared/states/user.state';
import { Ticket } from '../../shared/models/ticket.model';
import { TicketPack } from '../../shared/models/ticket-pack.model';
import { BsModalRef } from 'ngx-bootstrap';
import { Title } from '@angular/platform-browser';
import { concat } from 'rxjs';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit {

  range: Date[];
  bsConfig: any;
  packs: TicketPack[];
  selectedDate: Date = new Date();
  newTicket: Ticket;
  modalRef: BsModalRef;

  currentTicket: Ticket;
  similarTickets: Ticket[];
  showAllSimilar: boolean = false;

  @ViewChild("deleteAll") deleteAllModal: ElementRef;
  @ViewChild("addWithTime") addWithTimeModal: ElementRef;

  days: number[] = undefined;
  dateTo: Date = new Date();
  repeat: boolean = false;

  constructor(private activateRoute: ActivatedRoute,
    private modalService: BsModalService,
    private schedule: ScheduleService,
    private dictionary: DictionaryService,
    private messageService: MessageService,
    private ngxService: NgxUiLoaderService,
    private titleService: Title,
    private userState: UserState) {
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
    this.titleService.setTitle('Тайм-лист');
    this.currentWeek();
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.ngxService.start();
    this.packs = await this.schedule.myTicketPacks(this.range);
    this.newTicket = new Ticket();
    this.newTicket.date = this.selectedDate;
    let notifications = await this.dictionary.getNotifications();
    this.userState.assignedTicketCount.state = parseInt(notifications.filter(n => n.id == 'assignedTickets')[0].name);
    this.userState.assignedTickets.state = await this.schedule.assignedTickets();
    this.ngxService.stop();
  }

  async saveTicket() {
    if (this.isTimeCorrect()) {
      try {
        await this.schedule.updateTicket(this.newTicket);
        this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Запись изменена", life: 5000 });
        this.loadData();
        this.modalRef.hide();
      } catch (e) {
        this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
      }
    }
  }

  async addTicket() {
    if (this.isTimeCorrect()) {
      try {
        await this.schedule.addTicket(this.newTicket, this.repeat, this.dateTo, this.days);
        this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Запись добавлена в тайм-лист", life: 5000 });
        this.dateTo = new Date();
        this.selectedDate = new Date();
        this.repeat = false;
        this.days = undefined;
        this.loadData();
        this.modalRef.hide();
      } catch (e) {
        this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
      }
    }
  }

  isTimeCorrect() {
    if ((this.newTicket.hours > 23 || this.newTicket.hours < 0) || (this.newTicket.minutes > 59 || this.newTicket.minutes < 0)) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Время введено не корректно', life: 5000 });
      return false;
    }
    return true;
  }

  async delete(ticket: Ticket) {
    try {
      var similarTickets = await this.schedule.deleteTicket(ticket);
      if (!similarTickets) {
        await this.loadData();
        this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Запись удалена", life: 5000 });
        return;
      }
      else {
        this.currentTicket = Object.assign({}, ticket);
        this.similarTickets = similarTickets;
        this.openModal(this.deleteAllModal);
      }

    } catch (e) {
      console.log(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async deleteSimilar() {
    try {
      await this.schedule.deleteSimilarTickets(this.currentTicket);
      this.loadData();
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Записи удалены", life: 5000 });
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async deleteOne() {
    try {
      await this.schedule.deleteOneTicket(this.currentTicket);
      this.loadData();
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Одна запись удалена", life: 5000 });
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  openModal(modal, date: Date = null) {
    if (date) {
      this.newTicket = new Ticket();
      this.newTicket.date = new Date(date.toString());
    }

    if (this.newTicket.hasChecklist) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Изменить запись здесь нельзя, так как она находится в чек-листе. Измените ее в чек-листе, если он принадлежит Вам.', life: 5000 });
      return;
    }

    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
    this.showAllSimilar = false;
  }

  copy(ticket: Ticket) {
    this.newTicket = Object.assign({}, ticket);
    this.newTicket.date = new Date(this.newTicket.date.toString()); //Костыль для ngx-datepicker'а
  }

  setDays(days: number[]) {
    this.days = days;
  }

  setDateTo(dateTo: Date) {
    this.dateTo = dateTo;
  }

  setRepeat(repeat: boolean) {
    this.repeat = repeat;
  }

  async sendTimeline() {
    try {
      await this.schedule.sendTimeline(this.range);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Тайм-лист отправлен на вашу почту", life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async makeDone(ticket: Ticket) {
    try {
      var response = await this.schedule.makeDone(ticket.id, ticket.hasChecklist);
      await this.loadData();
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: response.message, life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async makeImportant(id) {
    try {
      var response = await this.schedule.makeImportant(id);
      await this.loadData();
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: response.message, life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }


  ticketToAccept: Ticket;

  async acceptTicket(ticket: Ticket = null) {
    // if(ticket != null){
    //   this.ticketToAccept = Object.assign({}, ticket);
    //   if(this.ticketToAccept.date){
    //     this.ticketToAccept.date = new Date(this.ticketToAccept.date.toString()); //Костыль для ngx-datepicker'а
    //   }
    // }

    try {
      if (!ticket.date || !ticket.hours || (!ticket.minutes && ticket.minutes != 0)) {
        this.ticketToAccept = Object.assign({}, ticket);
        if (this.ticketToAccept.date) {
          this.ticketToAccept.date = new Date(this.ticketToAccept.date.toString()); //Костыль для ngx-datepicker'а
          this.ticketToAccept.blockedData = true;
        }
        if((ticket.hours && ticket.hours != 0) || (ticket.minutes && ticket.minutes != 0)){
          this.ticketToAccept.blockedTime = true;
        }

        this.openModal(this.addWithTimeModal);
        return;
      }
      await this.schedule.acceptTicket(ticket);
      await this.loadData();
      this.userState.assignedTickets.state.splice(this.userState.assignedTickets.state.indexOf(ticket), 1);
      if (this.userState.assignedTickets.state.length == 0) {
        this.closeModal();
      }
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задача занесена в тайм-лист", life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async acceptWithNewDateTimeTicket() {
    try {
      if(!this.ticketToAccept.date || !this.ticketToAccept.hours || (!this.ticketToAccept.minutes && this.ticketToAccept.minutes != 0)){
        throw {error: "Укажите дату или время"};
      }
      await this.schedule.acceptTicket(this.ticketToAccept);
      await this.loadData();
      this.closeModal();
      this.userState.assignedTickets.state.splice(this.userState.assignedTickets.state.indexOf(this.ticketToAccept), 1);
      if (this.userState.assignedTickets.state.length == 0) {
        this.closeModal();
      }
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задача занесена в тайм-лист", life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async declineTicket(ticket: Ticket){
    try {
      await this.schedule.declineTicket(ticket);
      await this.loadData();
      this.closeModal();
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задача отклонена", life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  getDocument() {
    window.open(`/api/Report/ForTimeline?` +
      `startDay=${this.range[0].getDate()}&startMonth=${this.range[0].getMonth() + 1}&startYear=${this.range[0].getFullYear()}`
      + `&endDay=${this.range[1].getDate()}&endMonth=${this.range[1].getMonth() + 1}&endYear=${this.range[1].getFullYear()}`);
  }

  currentWeek() {

    this.range = new Array<Date>();

    var monday = this.getMondayOfCurrentWeek(new Date());
    var sunday = this.getSundayOfCurrentWeek(new Date());

    this.range.push(monday);
    this.range.push(sunday);
  }

  getMondayOfCurrentWeek(d) {
    var day = d.getDay();
    return new Date(d.getFullYear(), d.getMonth(), d.getDate() + (day == 0 ? -6 : 1) - day);
  }
  getSundayOfCurrentWeek(d) {
    var day = d.getDay();
    return new Date(d.getFullYear(), d.getMonth(), d.getDate() + (day == 0 ? 0 : 7) - day);
  }

  getTooltip(ticket: Ticket) {
    return `Задание из чеклиста &laquo;${ticket.checklist.name}&raquo; <br/> пользователя <br/> ${ticket.checklist.user.fullName}`;
  }

}
