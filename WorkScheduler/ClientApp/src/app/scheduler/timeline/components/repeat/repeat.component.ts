import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-repeat',
  templateUrl: './repeat.component.html',
  styleUrls: ['./repeat.component.css']
})
export class RepeatComponent implements OnInit {

  bsConfig: any;
  modalRef: BsModalRef;
  selectedDateTo: Date = new Date();
  repeat: boolean = false;

  mon: boolean = false;
  tue: boolean = false;
  wed: boolean = false;
  thr: boolean = false;
  fri: boolean = false;
  sat: boolean = false;
  sun: boolean = false;

  @Output() days = new EventEmitter<number[]>();
  @Output() dateTo = new EventEmitter<Date>();
  @Output() repeatAction = new EventEmitter<boolean>();

  constructor() {
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
  }

  ngOnInit() {
  }

  dayChanged() {
    var selectedDays: number[] = [];

    if (this.mon) selectedDays.push(1);
    if (this.tue) selectedDays.push(2);
    if (this.wed) selectedDays.push(3);
    if (this.thr) selectedDays.push(4);
    if (this.fri) selectedDays.push(5);
    if (this.sat) selectedDays.push(6);
    if (this.sun) selectedDays.push(0);

    this.days.emit(selectedDays);
  }

  dateChanged() {
    this.dateTo.emit(this.selectedDateTo);
  }

  repeatChanged() {
    this.repeatAction.emit(this.repeat);
  }
}
