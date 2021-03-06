import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../monitoring/services/student.service';
import { AcademicYear } from '../../shared/models/academic-year.model';
import { DictionaryService } from '../../shared/services/dictionary.service';

@Component({
  selector: 'app-register-settings',
  templateUrl: './register-settings.component.html',
  styleUrls: ['./register-settings.component.css']
})
export class RegisterSettingsComponent implements OnInit {

  allAcademicYears: AcademicYear[];
  
  constructor(private dictionary: DictionaryService, public studentService: StudentService) { }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    await this.studentService.loadStudents();
  }


  academicYearChanged() {
    
  }

}
