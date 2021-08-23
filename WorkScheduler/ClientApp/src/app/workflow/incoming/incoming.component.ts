import { Component, OnInit } from '@angular/core';
import { IncomingFilter } from '../../shared/table/incoming-filter';
import { SortDirection } from '../../shared/table/sort-direction';

@Component({
  selector: 'app-incoming',
  templateUrl: './incoming.component.html',
  styleUrls: ['./incoming.component.css']
})
export class IncomingComponent implements OnInit {

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
