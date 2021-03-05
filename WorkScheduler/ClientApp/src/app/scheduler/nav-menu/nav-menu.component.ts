import { Component } from '@angular/core';
import { UserState } from '../../shared/states/user.state';
import { isUserInRole, User } from '../../shared/models/user';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ProblemService } from '../../shared/services/problem.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  report: string;

  modalRef: BsModalRef;

  constructor(public userState: UserState, private modalService: BsModalService, private problem: ProblemService) {
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    location.href = "/api/Account/Logout";
  }

  checkRole(user: User, role: string) {
    return isUserInRole(user, role);
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  sendReport(){
    this.problem.sendReport(this.report);
    this.modalRef.hide();
  }
}
