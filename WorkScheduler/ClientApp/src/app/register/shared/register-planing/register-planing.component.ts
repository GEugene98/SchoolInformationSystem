import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { MessageService } from 'primeng/api';
import { Subject } from 'rxjs';
import { AcademicYear } from '../../../shared/models/academic-year.model';
import { Dictionary } from '../../../shared/models/dictionary.model';
import { ImportPlaning } from '../../models/import-planing.model';
import { PlaningRecord } from '../../models/planing-record.model';
import { RegisterPlaningService } from '../../services/register-planing.service';

@Component({
  selector: 'app-register-planing',
  templateUrl: './register-planing.component.html',
  styleUrls: ['./register-planing.component.css']
})
export class RegisterPlaningComponent implements OnInit {

  bsConfig: any;
  modalRef: BsModalRef;

  importModel: ImportPlaning = new ImportPlaning();

  @Input() records: PlaningRecord[] = [];
  editedRecord: PlaningRecord = new PlaningRecord();

  @Input() selectedAcademicYear: AcademicYear;
  @Input() selectedAssociation: Dictionary<number>;
  @Input() selectedGroup: Dictionary<number>;

  @Output() ktpChanged = new EventEmitter();
  
  constructor(private planingService: RegisterPlaningService, 
    private modalService: BsModalService, 
    private messageService: MessageService) { 
      this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
    }

  async ngOnInit() {
    //await this.loadData();
  }

  async loadData(){
    this.ktpChanged.emit();
    if(this.selectedAcademicYear && this.selectedAssociation.id && this.selectedGroup){
      this.records = await this.planingService.getRecords(this.selectedAcademicYear.id, this.selectedAssociation.id, this.selectedGroup.id);
    }
  }

  async uploadExcel(uploadForm){
    try{
      await this.planingService.uploadPlaningExcel(new FormData(uploadForm), this.importModel);
      await this.loadData();
      this.closeModal();
    }
    catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка обработки файла', detail: e.error , life: 5000 });
    }
  }

  refreshImportModel(){
    this.importModel = new ImportPlaning();
    this.importModel.academicYearId = this.selectedAcademicYear.id;
    this.importModel.associationId = this.selectedAssociation.id;
    this.importModel.groupId = this.selectedGroup.id;
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  openEditModal(modal, record: PlaningRecord){
    this.editedRecord = Object.assign({}, record);
    if(this.editedRecord.date)
      this.editedRecord.date = new Date(this.editedRecord.date.toString()); //Костыль для ngx-datepicker'а

    this.openModal(modal);
  }

  openPreImportModal(modal) {
    if(!this.selectedAcademicYear || !this.selectedAssociation || !this.selectedGroup){
      this.messageService.add({ severity: 'warn', summary: 'Не все параметры выбраны', detail: "Выберите учебный год, объединение и группу", life: 5000 });
      return;
    }
    this.refreshImportModel();
    this.openModal(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  async saveEditedRecord(){
    try{
      await this.planingService.updateRecord(this.editedRecord);
      await this.loadData();
      this.closeModal();
    }
    catch(e){
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error , life: 5000 });
    }   
  }

  async deleteRecord(recordId: number){
    try{
      await this.planingService.deleteRecord(recordId);
      await this.loadData();
    }
    catch(e){
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error , life: 5000 });
    }   
  }

}
