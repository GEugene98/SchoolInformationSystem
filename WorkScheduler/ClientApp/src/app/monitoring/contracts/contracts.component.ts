import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-contracts',
  templateUrl: './contracts.component.html',
  styleUrls: ['./contracts.component.css']
})
export class ContractsComponent implements OnInit {
  showAll: boolean;


  organization: boolean = false;
  contractNumber: boolean = false;
  dateSigning: boolean = false;
  signedBy: boolean = false;
  subject: boolean = false;
  contractSum: boolean = false;
  status: boolean = false;
  controlDate: boolean = false;
  
  list: string[] = ["Привет", "Пока"]; 

  constructor() { }

  ngOnInit() {

  }

  showAllHandler(){
    this.showAll = !this.showAll;
  }

  columnsChanged() {
    var selectedColumns: number[] = []

    if (this.organization) selectedColumns.push(0);
    if (this.contractNumber) selectedColumns.push(1);
    if (this.dateSigning) selectedColumns.push(2);
    if (this.signedBy) selectedColumns.push(3);
    if (this.subject) selectedColumns.push(4);
    if (this.contractSum) selectedColumns.push(5);
    if (this.status) selectedColumns.push(6);
    if (this.controlDate) selectedColumns.push(7);
  }
}
