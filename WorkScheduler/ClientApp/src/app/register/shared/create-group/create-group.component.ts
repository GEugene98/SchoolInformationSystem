import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Student } from '../../../shared/models/student';
import { Group } from '../../models/group.model';

@Component({
  selector: 'app-create-group',
  templateUrl: './create-group.component.html',
  styleUrls: ['./create-group.component.css']
})
export class CreateGroupComponent implements OnInit {

  @Input() group: Group;
  @Input() students: Student[];

  constructor() { }

  ngOnInit() {
  }

  updateStudentsToAdd(selectedStudents: Student[]){
    this.group.students = selectedStudents;
  }
}
