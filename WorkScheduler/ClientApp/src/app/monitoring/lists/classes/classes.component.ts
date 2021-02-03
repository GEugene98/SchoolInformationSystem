import { Component, OnInit, ViewChild } from '@angular/core';
import { NgSelectComponent } from '@ng-select/ng-select';
import { indexOf } from 'lodash';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Observable } from 'rxjs';
import { AcademicYear } from '../../../shared/models/academic-year.model';
import { Student } from '../../../shared/models/student';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { Class } from '../../models/class.model';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-classes',
  templateUrl: './classes.component.html',
  styleUrls: ['./classes.component.css']
})
export class ClassesComponent implements OnInit {

  modalRef: BsModalRef;
  selectedAcademicYear: AcademicYear;
  allAcademicYears: AcademicYear[];
  allStudents: Student[] = [];
  studentsToAdd: Student[] = [];
  classesWithStudents: Class[] = [];
  
  constructor(private modalService: BsModalService, private dictionary: DictionaryService, private student: StudentService) { }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData(){
    this.allStudents = await this.student.getStudents();
    this.allAcademicYears = await this.dictionary.getAcademicYears();
  }

  async academicYearChanged() {
    this.classesWithStudents = await this.student.getStudentsByClasses(this.selectedAcademicYear.id);
  }

  openModal(modal) {
    this.studentsToAdd = [];
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  updateStudentsToAdd(students: Student[]) {
    this.studentsToAdd = students;
  }

  bindStudentsToClass() {

  }
}
