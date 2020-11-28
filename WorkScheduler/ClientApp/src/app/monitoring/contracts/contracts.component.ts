import { Component, OnInit } from '@angular/core';
import { Contract } from '../models/contract.model';

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css']
})
export class ContractsComponent implements OnInit {
  showAll: boolean;

  data: Contract[] = [
    new Contract(),
    new Contract(),
    new Contract(),
    new Contract(),
  ];

  columns = [
    {
      name: "Организация",
      visibility: true,
      variable: "organization"
    },
    {
      name: "№ договора",
      visibility: true,
      variable: "contractNumber"
    },
    {
      name: "Дата подписания, срок действия",
      visibility: false
    },
    {
      name: "Предмет договора",
      visibility: false,
      variable: "subject"
    },
    {
      name: "Кем подписан",
      visibility: false
    },
    {
      name: "Сумма договора",
      visibility: false
    },
    {
      name: "Статус",
      visibility: false
    },
    {
      name: "Дата контроля",
      visibility: false
    },
    {
      name: "Комментарий",
      visibility: false
    }
  ];

  constructor() { }

  ngOnInit() {

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
