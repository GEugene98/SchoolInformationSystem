import { Component, OnInit } from '@angular/core';
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

@Component({
  selector: 'app-timeline',
  templateUrl: './timeline.component.html',
  styleUrls: ['./timeline.component.css']
})
export class TimelineComponent implements OnInit {

  bsConfig: any;
  packs: TicketPack[];
  selectedDate: Date = new Date();
  newTicket: Ticket;
  modalRef: BsModalRef;

  days: number[] = undefined;
  dateTo: Date = undefined;
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
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.packs = await this.schedule.myTicketPacks(this.selectedDate);
    this.newTicket = new Ticket();
    this.newTicket.date = this.selectedDate;
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
      await this.schedule.deleteTicket(ticket);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Запись удалена", life: 5000 });
      this.loadData();
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
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
}
