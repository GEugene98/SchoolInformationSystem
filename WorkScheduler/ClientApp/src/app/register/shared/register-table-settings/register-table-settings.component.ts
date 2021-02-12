import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { StudentService } from '../../../monitoring/services/student.service';
import { AcademicYear } from '../../../shared/models/academic-year.model';
import { Student } from '../../../shared/models/student';
import { Association } from '../../models/association.model';
import { AssociationType } from '../../models/enums/association-type.enum';
import { Group } from '../../models/group.model';
import { AssociationService } from '../../services/association.service';
import { GroupService } from '../../services/group.service';
import _ = require('lodash');

@Component({
  selector: 'app-register-table-settings',
  templateUrl: './register-table-settings.component.html',
  styleUrls: ['./register-table-settings.component.css']
})
export class RegisterTableSettingsComponent implements OnInit {

  @Input() allAcademicYears: AcademicYear[];
  @Input() associationType: AssociationType;
  @Input() students: Student[];
  
  associations: Association[];

  allGroupsByTypeAndYear: Group[];
  selectedGroupsToCreateAssociation: Group[];
  newGroups: Group[] = [];

  selectedAcademicYear: AcademicYear;
  newAssociation: Association;
  modalRef: BsModalRef;
  
  constructor(private groupService: GroupService, private associationService: AssociationService, private modalService: BsModalService) { }

  async ngOnInit() {
    await this.loadData();
  }

  async academicYearChanged(){
    await this.loadData();
  }

  async loadData() {
    if(this.selectedAcademicYear){
      this.associations = await this.associationService.getAssotiations(this.associationType, this.selectedAcademicYear.id);
      this.allGroupsByTypeAndYear = await this.groupService.getGroups(this.associationType, this.selectedAcademicYear.id);
    }
  }

  createGroup(){
    this.newGroups.push(new Group());
  }

  async createAssociation(){
    this.newAssociation.groups = _.union(this.newGroups, this.selectedGroupsToCreateAssociation);
    await this.associationService.createAssotiation(this.newAssociation, this.selectedAcademicYear.id);
    await this.loadData();
    this.closeModal();
  }

  openModal(modal) {
    this.newAssociation = new Association();
    this.newGroups = []; this.selectedGroupsToCreateAssociation = [];
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }

  getStudentsInGroup(group: Group) {
    var str = "";
    group.students.forEach(element => {
      str += element.fullName + '\n'
    });
    return str;
  }

}
