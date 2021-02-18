import { Component, Input, OnInit } from '@angular/core';
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
  @Input() selectedAcademicYear: AcademicYear;
  @Input() associations: Dictionary<number>[];
  @Input() academicPeriods: Dictionary<number>[];
  @Input() groups: Group[];
  
  constructor() { }

  ngOnInit() {
  }

}
