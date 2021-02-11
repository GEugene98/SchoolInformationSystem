import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
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

  constructor(private modalService: BsModalService, private studentService: StudentService) { 
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

  openModal(modal) {
    this.studentToCreate = new Student();
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

}
