import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
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

  modalRef: BsModalRef;
  importModel: ImportPlaning = new ImportPlaning();

  records: PlaningRecord[] = [];

  @Input() selectedAcademicYear: AcademicYear;
  @Input() selectedAssociation: Dictionary<number>;
  @Input() selectedGroup: Dictionary<number>;

  @Input() needRefresh: Subject<boolean>;
  
  constructor(private planingService: RegisterPlaningService, 
    private modalService: BsModalService, 
    private messageService: MessageService) { }

  async ngOnInit() {
    await this.loadData();

    this.needRefresh.subscribe(() => this.loadData());
  }

  async loadData(){
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
    this.refreshImportModel();
    this.modalRef = this.modalService.show(modal);
  }

  openPreImportModal(modal) {
    if(!this.selectedAcademicYear || !this.selectedAssociation || !this.selectedGroup){
      this.messageService.add({ severity: 'warn', summary: 'Не все параметры выбраны', detail: "Выберите учебный год, объединение и группу", life: 5000 });
      return;
    }
    this.openModal(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

}
