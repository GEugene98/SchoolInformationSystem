import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Checklist } from '../../shared/models/checklist.model';
import { ScheduleService } from '../services/schedule.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Ticket } from '../../shared/models/ticket.model';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { User } from '../../shared/models/user';
import { MessageService } from 'primeng/api';
import { Title } from '@angular/platform-browser';
import { NgxUiLoaderService } from 'ngx-ui-loader'
import { SuccessEvent, UploadEvent } from '@progress/kendo-angular-upload';
import { guid } from '../../shared/guid';
import { SortDirection } from '../../shared/table/sort-direction';
import { ChecklistFilter } from '../../shared/table/checklist/checklist-filter';
import { TableRequest } from '../../shared/table/table-request';
import Debounce from 'debounce-decorator'

@Component({
  selector: 'app-checklist-details',
  templateUrl: './checklist-details.component.html',
  styleUrls: ['./checklist-details.component.scss']
})
export class ChecklistDetailsComponent implements OnInit, OnDestroy {

  bsConfig: any;
  modalRef: BsModalRef;

  responsibles;

  statuses = [{id: undefined, name: ''},{id: 0, name: 'Не назначено'}, {id: 1, name: 'Назначено'}, {id: 2, name: 'Принято'}, {id: 3, name: 'Отклонено'}, {id: 4, name: 'Готово'}];

  checklistId: number;
  checklist: Checklist;

  newTicket: Ticket;

  detailsTiclet: Ticket;

  transactionId: string;
  fileUploadUrl = '/api/File/UploadTemporaryFiles';
  fileRemoveUrl = '/api/File/RemoveTemporaryFiles';

  sortProperty: string = 'Created';
  sortDirection: SortDirection = SortDirection.Descending;
  filter = new ChecklistFilter();

  currentPage: number = 1;
  totalPages: number;
  totalItemCount: number;

  refreshIntervalId;

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
    this.transactionId = guid();
    this.loadData();

    this.refreshIntervalId = setInterval(async () => {
      let response = await this.schedule.getChecklist(this.checklistId, this.getRequestDetails());
      this.checklist = response.body;
      this.totalItemCount = response.totalItemCount;
      this.totalPages = response.pageCount;
    }, 30000); 
  }

  ngOnDestroy(){
    clearInterval(this.refreshIntervalId);
  }

  @Debounce(1500)
  async loadData(showLoader: boolean = true) {
    if (showLoader) this.ngxService.start();
    let response = await this.schedule.getChecklist(this.checklistId, this.getRequestDetails());
    this.checklist = response.body;
    this.totalItemCount = response.totalItemCount;
    this.totalPages = response.pageCount;
    this.titleService.setTitle(this.checklist.name);
    this.responsibles = this.dictionary.getResponsibles();
    this.newTicket = new Ticket();
    if (showLoader)this.ngxService.stop();
  }

  async sort(sortProperty: string) {
    this.sortDirection = (this.sortDirection == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
    this.sortProperty = sortProperty;
    await this.loadData();
  }

  getRequestDetails(): TableRequest<ChecklistFilter> {
    let reqest = new TableRequest<ChecklistFilter>();
    reqest.filter = this.filter;
    reqest.pageNumber = this.currentPage;
    reqest.pageSize = 15;
    reqest.sortDirection = this.sortDirection;
    reqest.sortProperty = this.sortProperty;
    return reqest;
  }

  openModal(modal) {
    this.transactionId = guid();
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  showBoundaryLinks() {
    if (this.totalPages >= 5) {
      return true;
    }
    else {
      return false;
    }
  }

  copy(ticket: Ticket) {
    this.newTicket = Object.assign({}, ticket);
    if(this.newTicket.date)
      this.newTicket.date = new Date(this.newTicket.date.toString()); //Костыль для ngx-datepicker'а
  }

  scrollToTop() {
    window.scrollTo(0, 0);
  }

  async deleteFileBinding(file, ticket: Ticket) {
    try {
      await this.schedule.deleteFileBinding(file.id, ticket.id, 1);

      var index = ticket.inFiles.indexOf(file);
      if (index > -1) {
        ticket.inFiles.splice(index, 1);
      }

    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async addTicket(){
    try {
      this.newTicket.checklistId = this.checklistId;
      if(this.newTicket.user) this.newTicket.userId = this.newTicket.user.id;
      await this.schedule.addTicketFromChecklist(this.newTicket, this.transactionId);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Задание создано", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
      this.newTicket = new Ticket();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  openDetails(ticket: Ticket, modal) {
    this.copy(ticket);
    this.openModal(modal);
  }

  downloadFile(fileId){
    location.href = 'api/File/download?fileId=' + fileId;
  }

  async saveReply(){
    try {
      await this.schedule.saveChecklistTicketDetails(this.newTicket, this.transactionId);
      await this.loadData();
      this.closeModal();
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Детали задания сохранены", life: 5000 });
    } catch (e) {
      console.error(e);
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
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async makeDone(ticket: Ticket) {
    try {
      var response = await this.schedule.makeDone(ticket.id, true);
      await this.loadData(false);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: response.message, life: 5000 });
    } catch (e) {
      console.error(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  clear(){
    this.newTicket = new Ticket();
  }

  uploadEventHandler(e: UploadEvent) {
    e.headers = e.headers.append('transaction-id', this.transactionId);
  }
  removeEventHandler(e: UploadEvent) {
    e.headers = e.headers.append('transaction-id', this.transactionId);
  }

  uploaded($event: SuccessEvent) {
    //(<any>$event.files[0]).serverFileName = $event.response.text();
  }
}
