import { Component, OnInit } from '@angular/core';
import { IncomingFilter } from '../../shared/table/incoming-filter';
import { SortDirection } from '../../shared/table/sort-direction';

@Component({
  selector: 'app-outgoing',
  templateUrl: './outgoing.component.html',
  styleUrls: ['./outgoing.component.css']
})
export class OutgoingComponent implements OnInit {

  filter = new IncomingFilter();
  sortProperty: string = 'Taken';
  sortDirection: SortDirection = SortDirection.Descending;

  constructor() { }

  ngOnInit(): void {
  }

  async loadData() {
    
  }

  async sort(sortProperty: string) {
    this.sortDirection = (this.sortDirection == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
    this.sortProperty = sortProperty;
    await this.loadData();
  }

}
