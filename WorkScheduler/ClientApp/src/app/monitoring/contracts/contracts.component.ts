import { Component, OnInit } from '@angular/core';
import { Contract } from '../models/contract.model';
import { ContractService } from '../services/contract.service';

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css']
})
export class ContractsComponent implements OnInit {
  showAll: boolean;

  data: Contract[] = [];

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

  constructor( private contract: ContractService) { }

  async ngOnInit() {
    this.data = await this.contract.getContracts();
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

}
