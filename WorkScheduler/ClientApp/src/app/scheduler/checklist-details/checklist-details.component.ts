import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Checklist } from '../../shared/models/checklist.model';
import { ScheduleService } from '../services/schedule.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Ticket } from '../../shared/models/ticket.model';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { User } from '../../shared/models/user';
import { MessageService } from 'primeng/api';
import { Title } from '@angular/platform-browser';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
  selector: 'app-checklist-details',
  templateUrl: './checklist-details.component.html',
  styleUrls: ['./checklist-details.component.css']
})
export class ChecklistDetailsComponent implements OnInit {

  bsConfig: any;
  modalRef: BsModalRef;

  responsibles;

  checklistId: number;
  checklist: Checklist;

  newTicket: Ticket;

  constructor(private activateRoute: ActivatedRoute,
    private modalService: BsModalService,
    private dictionary: DictionaryService,
    private messageService: MessageService,
    private ngxService: NgxUiLoaderService,
    private titleService: Title,
    private schedule: ScheduleService) { 
    this.checklistId = this.activateRoute.snapshot.params['id'];
    
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
  }

  ngOnInit() {
    this.loadData();
  }

  async loadData(){
    this.ngxService.start();
    this.checklist = await this.schedule.getChecklist(this.checklistId);
    this.titleService.setTitle(this.checklist.name);
    this.responsibles = this.dictionary.getResponsibles();
    this.newTicket = new Ticket();
    this.ngxService.stop();
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }
  
  copy(ticket: Ticket) {
    this.newTicket = Object.assign({}, ticket);
    if(this.newTicket.date)
      this.newTicket.date = new Date(this.newTicket.date.toString()); //Костыль для ngx-datepicker'а
  }

  async addTicket(){
    try {
      this.newTicket.checklistId = this.checklistId;
      if(this.newTicket.user) this.newTicket.userId = this.newTicket.user.id;
      await this.schedule.addTicketFromChecklist(this.newTicket);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задание создано", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
      this.newTicket = new Ticket();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async saveTicket(){
    try {
      if(this.newTicket.user) this.newTicket.userId = this.newTicket.user.id;
      await this.schedule.editTicketFromChecklist(this.newTicket);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задание сохранено", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
      this.newTicket = new Ticket();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async delete(ticket: Ticket){
    try {
      await this.schedule.deleteTicketFromChecklist(ticket);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задание удалено", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  clear(){
    this.newTicket = new Ticket();
  }

}
