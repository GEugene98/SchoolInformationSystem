import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AcademicYear } from '../../../shared/models/academic-year.model';
import { Dictionary } from '../../../shared/models/dictionary.model';
import { Group } from '../../models/group.model';

@Component({
  selector: 'app-register-params',
  templateUrl: './register-params.component.html',
  styleUrls: ['./register-params.component.css']
})
export class RegisterParamsComponent implements OnInit {

  @Input() allAcademicYears: AcademicYear[];
  selectedAcademicYearId: number;

  @Input() associations: Dictionary<number>[];
  selectedAssociationId: number;

  // @Input() academicPeriods: Dictionary<number>[];

  @Input() groups: Group[];
  selectedGroupId: number;


  @Output() selectedAcademicYearChanged: EventEmitter<number> = new EventEmitter();
  @Output() selectedAssociationChanged: EventEmitter<number> = new EventEmitter();
  @Output() selectedGroupChanged: EventEmitter<number> = new EventEmitter();
  
  constructor() { }

  ngOnInit() {
  }

  academicYearChanged() {
    this.selectedAssociationId = undefined;
    this.associations = [];
    this.selectedAcademicYearChanged.emit(this.selectedAcademicYearId);
  }

  associationChanged(){
    this.selectedGroupId = undefined;
    this.groups = [];
    this.selectedAssociationChanged.emit(this.selectedAssociationId);
  }

  groupChanged(){
    this.selectedGroupChanged.emit(this.selectedGroupId);
  }

}
