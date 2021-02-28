import { Component, Input, OnInit } from '@angular/core';
import { RegisterRecord } from '../../models/register-record.model';
import { RegisterRow } from '../../models/register-row.model';

const monthNames = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];

@Component({
  selector: 'app-register-table',
  templateUrl: './register-table.component.html',
  styleUrls: ['./register-table.component.css']
})
export class RegisterTableComponent implements OnInit {

  @Input() rows: RegisterRow[] = [];

  constructor() { }

  ngOnInit() {
  }

  getDays() {
    if(this.rows && this.rows[0].cells){
      return this.rows[0].cells.map(c => (new Date(c.date)).getDate());
    }
    return [];
  }

  getMonths(){
    if(this.rows && this.rows[0].cells){
      var months = [];
      for(let i = 0; i < this.rows[0].cells.length; i++){
        let month = (new Date(this.rows[0].cells[i].date)).getMonth();
        if(i == 0) {
          months.push({month: month, days: 1, monthName: monthNames[month]});
          continue;
        }
        let found = months.filter(m => m.month == month)
        if(found.length > 0) {
          found[0].days++;
        }
        else {
          months.push({month: month, days: 1, monthName: monthNames[month]});
        }
      }
      return months;
    }
    return [];
  }

  updateCell(cell: RegisterRecord) {
    console.log(cell);
  }
}
