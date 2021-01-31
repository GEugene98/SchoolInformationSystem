import { Component, OnInit, ViewChild } from '@angular/core';
import { NgSelectComponent } from '@ng-select/ng-select';
import { indexOf } from 'lodash';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { AcademicYear } from '../../../shared/models/academic-year.model';
import { Student } from '../../../shared/models/student';
import { DictionaryService } from '../../../shared/services/dictionary.service';
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
  
  constructor(private modalService: BsModalService, private dictionary: DictionaryService, private student: StudentService) { }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData(){
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    this.allStudents = await this.student.GetStudents();
  }

  openModal(modal) {
    this.studentsToAdd = [];
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  //Модалка для добавления учеников
  @ViewChild(NgSelectComponent) studentsSelect: NgSelectComponent;
  allStudents : Student[] = [];
  studentsToAdd: Student[] = [];
  foundStudent: Student;

  addFoundStudentToAddList(){
    if(this.foundStudent && this.studentsToAdd.indexOf(this.foundStudent) == -1){
      this.studentsToAdd.push(this.foundStudent);
    }
    setTimeout(() => {
      this.foundStudent = null;
    })
    
  }

  excludeStudentFromAddList(student: Student){
    const index = this.studentsToAdd.indexOf(student);
    if (index > -1) {
      this.studentsToAdd.splice(index, 1);
    }
  }

  bindStudentsToClass() {

  }
  //-------------------------------
}
