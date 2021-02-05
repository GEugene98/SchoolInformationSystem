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
  students: Student[];

  constructor(private modalService: BsModalService, private student: StudentService) { 
    this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.students = await this.student.getStudents();
    console.log(this.students)
  }

  async createStudent() {
    await this.student.createStudent(this.studentToCreate);
    this.closeModal();
    await this.loadData();
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

}
