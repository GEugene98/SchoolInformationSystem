import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Student } from '../../../../shared/models/student';

@Component({
  selector: 'app-student-selector',
  templateUrl: './student-selector.component.html',
  styleUrls: ['./student-selector.component.css']
})
export class StudentSelectorComponent implements OnInit {


  @Input() allStudents : Student[];
  @Input() selectedStudents: Student[] = []; 
  foundStudent: Student;

  @Output() selectedStudentsChanged = new EventEmitter<Student[]>();
  
  constructor() { 
    
  }

  ngOnInit() {
  }


  addFoundStudentToSelectList(){
    if(this.foundStudent && this.selectedStudents.indexOf(this.foundStudent) == -1){
      this.selectedStudents.push(this.foundStudent);
      this.selectedStudentsChanged.emit(this.selectedStudents);
    }
    setTimeout(() => {
      this.foundStudent = null;
    })
    
  }

  excludeStudentFromSelectList(student: Student){
    const index = this.selectedStudents.indexOf(student);
    if (index > -1) {
      this.selectedStudents.splice(index, 1);
      this.selectedStudentsChanged.emit(this.selectedStudents);
    }
  }

}
