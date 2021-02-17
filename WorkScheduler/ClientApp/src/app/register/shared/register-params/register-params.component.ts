import { Component, OnInit } from '@angular/core';
import { AcademicYear } from '../../../shared/models/academic-year.model';
import { Dictionary } from '../../../shared/models/dictionary.model';
import { Group } from '../../models/group.model';

@Component({
  selector: 'app-register-params',
  templateUrl: './register-params.component.html',
  styleUrls: ['./register-params.component.css']
})
export class RegisterParamsComponent implements OnInit {

  allAcademicYears: AcademicYear[];
  selectedAcademicYear: AcademicYear;
  associations: Dictionary<number>[];
  academicPeriods: Dictionary<number>[];
  groups: Group[];
  
  constructor() { }

  ngOnInit() {
  }

}
