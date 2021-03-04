import { Component, OnInit, ElementRef, ViewChild, OnDestroy } from '@angular/core';
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
import * as _ from 'lodash';
import { guid } from '../../shared/guid';
import { UploadEvent } from '@progress/kendo-angular-upload';

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit, OnDestroy {

  range: Date[];
  rangeBsConfig: any;
  bsConfig: any;
  packs: TicketPack[];
  originalPacks: TicketPack[];
  selectedDate: Date = new Date();
  newTicket: Ticket;
  modalRef: BsModalRef;

  checklistTicketToSeeDetails: Ticket;

  refreshIntervalId;

  transactionId: string;
  fileUploadUrl = '/api/File/UploadTemporaryFiles';
  fileRemoveUrl = '/api/File/RemoveTemporaryFiles';

  checklistTicketsOnly: boolean;

  checklistTicketToReply: Ticket;
  currentTicket: Ticket;
  similarTickets: Ticket[];
  showAllSimilar: boolean = false;

  @ViewChild("deleteAll", { static: true }) deleteAllModal: ElementRef;
  @ViewChild("addWithTime", { static: true }) addWithTimeModal: ElementRef;
  @ViewChild("fullComment", { static: true }) fullCommentModal: ElementRef;

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
    this.rangeBsConfig = { rangeInputFormat: 'DD.MM.YYYY', locale: 'ru' };
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
    this.titleService.setTitle('Тайм-лист');
    this.currentWeek();
  }

  async ngOnInit() {
    this.transactionId = guid();
    await this.loadData();

    this.refreshIntervalId = setInterval(async () => {
      this.originalPacks = await this.schedule.myTicketPacks(this.range);
      this.filterChecklistTickets();
    }, 30000); 
  }

  ngOnDestroy(){
    clearInterval(this.refreshIntervalId);
  }

  async loadData(showLoader: boolean = true) {
    // if (showLoader) this.ngxService.start();
    this.originalPacks = await this.schedule.myTicketPacks(this.range);
    this.filterChecklistTickets();
    this.newTicket = new Ticket();
    this.newTicket.date = this.selectedDate;
    this.newTicket.hours = this.newTicket.minutes = 0;
    this.userState.assignedTickets.state = await this.schedule.assignedTickets();
    let notifications = await this.dictionary.getNotifications();
    this.userState.assignedTicketCount.state = parseInt(notifications.filter(n => n.id == 'assignedTickets')[0].name);
    // if (showLoader)this.ngxService.stop();
  }

  checklistTicketsFilterHandler() {
    this.filterChecklistTickets();
  }

  filterChecklistTickets() {
    this.packs = _.cloneDeep(this.originalPacks);

    if (this.checklistTicketsOnly) {
      this.packs.forEach(p => {
        p.timeGroups.forEach(g => {
          g.tickets = g.tickets.filter(t => t.hasChecklist)
        });
      });
    }
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

  async deleteFileBinding(file, ticket: Ticket) {
    try {
      await this.schedule.deleteFileBinding(file.id, ticket.id, 2);

      var index = ticket.outFiles.indexOf(file);
      if (index > -1) {
        ticket.outFiles.splice(index, 1);
      }
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }


  async delete(ticket: Ticket) {
    if (ticket.hasChecklist) {
      await this.schedule.declineTicket(ticket);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задание отклонено", life: 5000 });
      await this.loadData();
      return;
    }

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

  openEditModal(modal){  
    //if (this.newTicket.hasChecklist) {
      //this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Изменить запись здесь нельзя, так как она находится в чек-листе. Измените ее в чек-листе, если он принадлежит Вам.', life: 5000 });
      //return;
    //}

    this.openModal(modal);
  }

  openModal(modal, date: Date = null) {
    if (date) {
      this.newTicket = new Ticket();
      this.newTicket.date = new Date(date.toString());
    }

    this.modalRef = this.modalService.show(modal);
  }

  ticketToDecline: Ticket;

  openDeclineDialog(modal, ticket: Ticket) {
    this.ticketToDecline = Object.assign({}, ticket);
    this.openModal(modal);
  }

  closeModal() {
    this.modalRef.hide();
    this.showAllSimilar = false;
  }


  getCutComment(comment: string) {
    if (comment.length < 150) {
      return comment+'';
    }
    return comment.substring(0, 150) + "...";
  }

  openFullCommentModal() {
    if (this.getCutComment(this.newTicket.comment) != this.newTicket.comment) {
      this.modalRef = this.modalService.show(this.fullCommentModal);
    }
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
    this.messageService.add({ severity: 'success', detail: "Идет отправка тайм-листа...", life: 5000 });
    try {
      await this.schedule.sendTimeline(this.range);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Тайм-лист успешно отправлен на вашу почту", life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async makeDone(ticket: Ticket) {
    try {
      if (ticket.done) {
        ticket.done = false;
      }
      else {
        ticket.done = true;
      }
      
      var response = await this.schedule.makeDone(ticket.id, ticket.hasChecklist);
      await this.loadData(false);
      //this.messageService.add({ severity: 'success', summary: 'Готово', detail: response.message, life: 5000 });
    } catch (e) {
      if (ticket.done) {
        ticket.done = false;
      }
      else {
        ticket.done = true;
      }
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async makeImportant(ticket: Ticket) {
    try {
      if (ticket.important) {
        ticket.important = false;
      }
      else {
        ticket.important = true;
      }
      var response = await this.schedule.makeImportant(ticket.id);
      await this.loadData(false);
      //this.messageService.add({ severity: 'success', summary: 'Готово', detail: response.message, life: 5000 });
    } catch (e) {
      if (ticket.important) {
        ticket.important = false;
      }
      else {
        ticket.important = true;
      }
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }


  ticketToAccept: Ticket;

  async acceptTicket(ticket: Ticket = null, closeDetailsModal: boolean = false) {
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
      if (this.userState.assignedTickets.state.length == 0) {
        this.closeModal();

        if (closeDetailsModal) {
          this.modalRef.hide(); 
        }
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
      if (this.userState.assignedTickets.state.length == 0) {
        this.closeModal();
      }
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задача занесена в тайм-лист", life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  openReplyModal(ticket: Ticket, modal) {
    this.transactionId = guid();
    this.checklistTicketToReply = Object.assign({}, ticket);
    this.openModal(modal);
  }

  openDetailsModal(ticket: Ticket, modal) {
    this.checklistTicketToSeeDetails = Object.assign({}, ticket);
    this.openModal(modal);
  }

  async saveReply(){
    try {
      await this.schedule.saveReply(this.checklistTicketToReply, this.transactionId);
      await this.loadData();
      this.closeModal();
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Детали задания сохранены", life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async declineTicket(){
    try {
      await this.schedule.declineTicket(this.ticketToDecline);
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

  downloadFile(fileId){
    location.href = 'api/File/download?fileId=' + fileId;
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

  uploadEventHandler(e: UploadEvent) {
    e.headers = e.headers.append('transaction-id', this.transactionId);
  }
  removeEventHandler(e: UploadEvent) {
    e.headers = e.headers.append('transaction-id', this.transactionId);
  }

}
