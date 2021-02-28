import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { MessageService } from 'primeng/api';
import { User } from '../../shared/models/user';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { Contract } from '../models/contract.model';
import { Organization } from '../models/organization.model';
import { ContractService } from '../services/contract.service';

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css']
})
export class ContractsComponent implements OnInit {

  bsConfig: any;
  showAll: boolean;
  modalRef: BsModalRef;
  contractToCreate: Contract = new Contract();
  newContract: Contract;
  statuses = [{id: 0, name: 'Подготовлен'}, {id: 1, name: 'На подписании'}, {id: 2, name: 'В исполнении'}, {id: 3, name: 'Завершен'}];
  organizations: Organization[] = [];
  users: User[] = [];

  columns = [
    {
      name: "Организация",
      visibility: false,
      variable: "organization.name"
    },
    {
      name: "№ договора",
      visibility: false,
      variable: "number"
    },
    {
      name: "Дата подписания, срок действия",
      visibility: true,
      variable: "signingDate"
    },
    {
      name: "Предмет договора",
      visibility: false,
      variable: "subject"
    },
    {
      name: "Кем подписан",
      visibility: false,
      variable: "signedBy.fullName"
    },
    {
      name: "Сумма договора",
      visibility: false,
      variable: "sum"
    },
    {
      name: "Статус",
      visibility: true,
      variable: "status"
    },
    {
      name: "Дата контроля",
      visibility: false,
      variable: "controlDate"
    },
    {
      name: "Комментарий",
      visibility: false,
      variable: "comment"
    }
  ];

  constructor( private contract: ContractService, 
    private modalService: BsModalService,
    private messageService: MessageService,
    private dictionary: DictionaryService,
    private datePipe: DatePipe) {
      this.contractToCreate.organization = new Organization();
      this.contractToCreate.signedBy = new User();
      this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
     }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData(){
    this.users = await this.dictionary.getUsers();
    this.organizations = await this.dictionary.getOrganizations();
    await this.contract.loadContracts();
  }

  getPropertyValue(object, fieldName) {
    let result;
    result = fieldName.split('.')
    .reduce(function(o, k) {
      return o && o[k];
    }, object)
    if(fieldName == "status"){
      if(result == undefined){
        return " "
      }
      return this.statuses.filter(s => s.id == result)[0].name;
    }
    if(fieldName == "signingDate" || fieldName == "controlDate"){
      return this.datePipe.transform(result, "dd.MM.yyyy");
    }
    return result;
  }

  showAllHandler(){
    this.showAll = !this.showAll;
  }

  getVisibleColumns(){
    return this.columns.filter(i => i.visibility);
  }

  copy(contract: Contract){
    this.newContract = Object.assign({}, contract);
    if(this.newContract.signingDate && this.newContract.controlDate){
      this.newContract.signingDate = new Date(this.newContract.signingDate.toString());
      this.newContract.controlDate = new Date(this.newContract.controlDate.toString());
    }
  }

  async saveContract(){
    try{
      await this.contract.editContarct(this.newContract);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Договор сохранен", life: 5000 });
      await this.loadData();
      this.closeModal();
      this.newContract = new Contract();
    } catch (e){
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async delete(contractId: number){
    try {
      await this.contract.deleteContract(contractId);
      this.messageService.add({severity: 'success', summary: 'Готово', detail: "Задание удалено", life: 5000 });
      await this.loadData();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async createContract(){
    try {
      await this.contract.createContract(this.contractToCreate);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Договор добавлен в таблицу", life: 5000 });
      this.closeModal();
      await this.loadData();
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

}
