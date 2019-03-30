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
  assignedTickets: Ticket[];
  showAllSimilar: boolean = false;

  @ViewChild("deleteAll") deleteAllModal: ElementRef;

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
    this.titleService.setTitle('Моя циклограмма');
    this.range = new Array<Date>();

    setInterval(async () => {
      this.assignedTickets = await this.schedule.assignedTickets();
    }, 10000);
  }

  async ngOnInit() {
    //await this.loadData();
  }

  async loadData() {
    this.ngxService.start();
    this.packs = await this.schedule.myTicketPacks(this.range);
    this.newTicket = new Ticket();
    this.newTicket.date = this.selectedDate;
    this.assignedTickets = await this.schedule.assignedTickets();
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
        this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Запись добавлена в циклограмму", life: 5000 });
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

  openModal(modal, date = null) {
    if (date) {
      this.newTicket = new Ticket();
      this.newTicket.date = date;
    }

    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
    this.showAllSimilar = false;
  }
  copy(ticket: Ticket) {
    this.newTicket = Object.assign({}, ticket);
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
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Циклограмма отправлена на вашу почту", life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async makeDone(id) {
    try {
      var response = await this.schedule.makeDone(id);
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


  async acceptTicket(ticket: Ticket) {
    try {
      await this.schedule.acceptTicket(ticket);
      await this.loadData();
      this.closeModal();
      this.messageService.add({ severity: 'success', summary: 'готово', detail: "Задача занесена в циклограмму", life: 5000 });
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

}
