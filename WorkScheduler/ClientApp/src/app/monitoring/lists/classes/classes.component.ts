import { Component, OnInit, ViewChild } from '@angular/core';
import { NgSelectComponent } from '@ng-select/ng-select';
import { indexOf } from 'lodash';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Observable } from 'rxjs';
import { AcademicYear } from '../../../shared/models/academic-year.model';
import { Student } from '../../../shared/models/student';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { Class } from '../../models/class.model';
import { ClassService } from '../../services/class.service';
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
  newClass: Class = new Class();
  classIdToBindStudents: number;
  
  constructor(private modalService: BsModalService, 
    private dictionary: DictionaryService, 
    private student: StudentService,
    private classService: ClassService) { }

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

  async createClass() {
    this.newClass.academicYearId = this.selectedAcademicYear.id;
    await this.classService.createClass(this.newClass);
    this.classesWithStudents = await this.student.getStudentsByClasses(this.selectedAcademicYear.id);
    this.closeModal();
  }

  openModal(modal) {
    this.studentsToAdd = [];
    this.newClass = new Class();
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
    this.classIdToBindStudents = undefined;
  }

  updateStudentsToAdd(students: Student[]) {
    this.studentsToAdd = students;
  }

  async bindStudentsToClass() {
    await this.student.putStudentsToClass(this.studentsToAdd.map(s => s.id), this.classIdToBindStudents, this.selectedAcademicYear.id);
    this.classesWithStudents = await this.student.getStudentsByClasses(this.selectedAcademicYear.id);
    this.closeModal();
  }
}
