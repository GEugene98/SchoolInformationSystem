import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { WorkSchedule } from '../../shared/models/work-schedule.model';

@Component({
  selector: 'app-confirm-accept-schedule',
  templateUrl: './confirm-accept-schedule.component.html',
  styleUrls: ['./confirm-accept-schedule.component.css']
})
export class ConfirmAcceptScheduleComponent implements OnInit {

  @Input() schedule: WorkSchedule;
  @Input() accept: boolean = false;
  @Output() execute = new EventEmitter<number[]>();
  @Output() cancel = new EventEmitter<number[]>();

  selectedAll: boolean;
  showAll: boolean;

  constructor() { }

  ngOnInit() {
  }

  selection() {
    this.selectedAll = !this.selectedAll;

    if (this.selectedAll) {
      this.schedule.actions.forEach(a => {
        a.selected = true;
      });
    }
    else {
      this.schedule.actions.forEach(a => a.selected = false);
    }
  }

  select() {
    for (var i = 0; i < this.schedule.actions.length; i++) {
      if (this.schedule.actions[i].selected) {
        this.selectedAll = true;
      }
      else {
        this.selectedAll = false;
        break;
      }
    }
  }

  executeEmit() {
    let actionIdsToConfirm = this.schedule.actions.filter(a => a.selected).map(a => a.id);
    this.selectedAll = false;
    this.execute.emit(actionIdsToConfirm);
  }

  cancelEmit() {
    let actionIdsToCancel = this.schedule.actions.filter(a => a.selected).map(a => a.id);
    this.selectedAll = false;
    this.cancel.emit(actionIdsToCancel);
  }
}
