import { Component, OnInit, ViewChild } from '@angular/core';
import { NgSelectComponent } from '@ng-select/ng-select';
import * as _ from 'lodash';
import { indexOf } from 'lodash';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
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
  studentsToAdd: Student[] = [];
  classesWithStudents: Class[] = [];
  newClass: Class = new Class();
  classIdToBindStudents: number;
  classToDelete: Class;
  
  constructor(private modalService: BsModalService, 
    private dictionary: DictionaryService, 
    private studentService: StudentService,
    private classService: ClassService) { }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData(){
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    this.selectedAcademicYear = this.allAcademicYears[0];
    await this.academicYearChanged();
  }

  async academicYearChanged() {
    if(this.selectedAcademicYear){
      this.classesWithStudents = await this.studentService.getStudentsByClasses(this.selectedAcademicYear.id);
    }
  }

  async createClass() {
    this.newClass.academicYearId = this.selectedAcademicYear.id;
    await this.classService.createClass(this.newClass);
    this.classesWithStudents = await this.studentService.getStudentsByClasses(this.selectedAcademicYear.id);
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

  deleteClass(classModel: Class, modal) {
    this.classToDelete = _.cloneDeep(classModel);
    this.openModal(modal);
  }

  async delete(id) {
    await this.classService.deleteClass(id);
    this.classesWithStudents = await this.studentService.getStudentsByClasses(this.selectedAcademicYear.id);
    this.closeModal();
  }

  updateStudentsToAdd(students: Student[]) {
    this.studentsToAdd = students;
  }

  async bindStudentsToClass() {
    await this.studentService.putStudentsToClass(this.studentsToAdd.map(s => s.id), this.classIdToBindStudents, this.selectedAcademicYear.id);
    this.classesWithStudents = await this.studentService.getStudentsByClasses(this.selectedAcademicYear.id);
    this.closeModal();
  }

  async excludeStudentFromClass(studentId, classId) {
    await this.studentService.excludeFromClass(studentId, classId);
    this.classesWithStudents = await this.studentService.getStudentsByClasses(this.selectedAcademicYear.id);
  }
}
