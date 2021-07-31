import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Student } from '../../../shared/models/student';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css']
})
export class StudentsComponent implements OnInit {

  bsConfig: any;
  modalRef: BsModalRef;
  studentToCreate: Student = new Student();
  studentIdTodelete;

  constructor(private modalService: BsModalService, public studentService: StudentService) { 
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    //this.students = await this.studentService.getStudents();
    await this.studentService.loadStudents();
  }

  async createStudent() {
    await this.studentService.createStudent(this.studentToCreate);
    this.closeModal();
    await this.loadData();
  }

  async updateStudent() {
    await this.studentService.updateStudent(this.studentToCreate);
    this.closeModal();
    await this.loadData();
  }

  deleteStudent(id, modal) {
    this.studentIdTodelete = id;
    this.modalRef = this.modalService.show(modal);
  }

  openModal(modal) {
    this.studentToCreate = new Student();
    this.modalRef = this.modalService.show(modal);
  }

  openEditModal(modal, student: Student) {
    this.studentToCreate = _.cloneDeep(student);
    if(this.studentToCreate.birthday)
      this.studentToCreate.birthday = new Date(this.studentToCreate.birthday.toString()); //Костыль для ngx-datepicker'а
    this.modalRef = this.modalService.show(modal);
  }

  async delete(id) {
    await this.studentService.deleteStudent(id);
    await this.loadData();
    this.closeModal();
  }

  closeModal() {
    this.modalRef.hide();
  }

}
