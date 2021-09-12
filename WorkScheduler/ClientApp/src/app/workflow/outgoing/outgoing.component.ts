import { Component, OnInit } from '@angular/core';
import { UploadEvent } from '@progress/kendo-angular-upload';
import * as _ from 'lodash';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { MessageService } from 'primeng/api';
import { guid } from '../../shared/guid';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { UserState } from '../../shared/states/user.state';
import { IncomingFilter } from '../../shared/table/incoming-filter';
import { SortDirection } from '../../shared/table/sort-direction';
import { OutgoingDocument } from '../models/outgoing-document.mode';
import { WorkflowService } from '../services/workflow.service';

@Component({
  selector: 'app-outgoing',
  templateUrl: './outgoing.component.html',
  styleUrls: ['./outgoing.component.css']
})
export class OutgoingComponent implements OnInit {

  bsConfig: any;
  modalRef: BsModalRef;

  responsibles;
  organizations;
  
  filter = new IncomingFilter();
  sortProperty: string = 'Taken';
  sortDirection: SortDirection = SortDirection.Descending;

  transactionId: string;
  fileUploadUrl = '/api/File/UploadTemporaryFiles';
  fileRemoveUrl = '/api/File/RemoveTemporaryFiles';

  newDoc: OutgoingDocument;
  docs: OutgoingDocument[];

  constructor(private modalService: BsModalService,
    private dictionary: DictionaryService,
    private workflowService: WorkflowService,
    private userState: UserState,
    private messageService: MessageService,) { 
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
  }

  ngOnInit(): void {
    this.transactionId = guid();
    this.responsibles = this.dictionary.getResponsibles();
    this.organizations = this.dictionary.getOrganizations();
    this.loadData();
  }

  async loadData() {
    this.docs = await this.workflowService.getOutgoings();
  }

  async sort(sortProperty: string) {
    this.sortDirection = (this.sortDirection == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
    this.sortProperty = sortProperty;
    await this.loadData();
  }

  async addDoc() {
    await this.workflowService.createOutgoing(this.newDoc, this.transactionId);
    this.loadData();
    this.closeModal();
  }

  async editDoc() {
    await this.workflowService.updateOutgoing(this.newDoc, this.transactionId);
    this.loadData();
    this.closeModal();
  }

  deleteId;
  openDeleteDocModal(modal, id) {
    this.deleteId = id;
    this.openModal(modal);
  }

  async deleteDoc(id) {
    await this.workflowService.deleteOutgoing(id);
    await this.loadData();
    this.closeModal();
  }

  openModal(modal, doc: OutgoingDocument = undefined) {
    if (doc) {
      this.newDoc = _.cloneDeep(doc);
      if (this.newDoc.taken) {
        this.newDoc.taken = new Date(this.newDoc.taken?.toString()); //Костыль для ngx-datepicker'а
      }
      if (this.newDoc.deadline) {
        this.newDoc.deadline = new Date(this.newDoc.deadline?.toString()); //Костыль для ngx-datepicker'а
      }
    }
    else {
      this.newDoc = new OutgoingDocument();
    }
    this.transactionId = guid();
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  uploadEventHandler(e: UploadEvent) {
    e.headers = e.headers.append('transaction-id', this.transactionId);
  }
  removeEventHandler(e: UploadEvent) {
    e.headers = e.headers.append('transaction-id', this.transactionId);
  }
  downloadFile(fileId){
    location.href = 'api/File/download?fileId=' + fileId;
  }

}
