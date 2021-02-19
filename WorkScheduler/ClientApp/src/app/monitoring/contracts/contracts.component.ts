import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { MessageService } from 'primeng/api';
import { Contract } from '../models/contract.model';
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


  //contracts: Contract[] = [];

  columns = [
    {
      name: "Организация",
      visibility: true,
      variable: "organization"
    },
    {
      name: "№ договора",
      visibility: true,
      variable: "number"
    },
    {
      name: "Дата подписания, срок действия",
      visibility: false,
      variable: "signingData"
    },
    {
      name: "Предмет договора",
      visibility: false,
      variable: "subject"
    },
    {
      name: "Кем подписан",
      visibility: false,
      variable: "signedBy"
    },
    {
      name: "Сумма договора",
      visibility: false,
      variable: "sum"
    },
    {
      name: "Статус",
      visibility: false,
      variable: "ContractStatus"
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
    private messageService: MessageService) {
      this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
      
     }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData(){
    //this.contracts = await this.contract.getContracts();
    await this.contract.loadContracts();
  }

  getPropertyValue(object, fieldName) {
    fieldName.split('.').forEach(function(token) {
      if (object) object = object[token];
    });
    return object;
  }

  showAllHandler(){
    this.showAll = !this.showAll;
  }

  getVisibleColumns(){
    return this.columns.filter(i => i.visibility);
  }

  // copy(contract: Contract){
  //   this.newContract = Object.assign({}, contract);
  //   if(this.newContract.signingData)
  //   this.newContract.signingData = new Date(this.newContract.signingData.toString());
  // }

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
    debugger
    await this.contract.createContract(this.contractToCreate);
    this.closeModal();
    await this.loadData();
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

}
