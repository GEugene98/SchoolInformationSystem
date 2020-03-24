import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { UserState } from '../shared/states/user.state';
import { AccountService } from '../shared/services/account.service';
import { User, isUserInRole } from '../shared/models/user';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  modalRef: BsModalRef;

  constructor(private titleService: Title, private modalService: BsModalService, public userState: UserState, public accountService: AccountService) {
    //this.titleService.setTitle('');
  }

  async ngOnInit() {
    try {
      this.userState.currentUser.state = await this.accountService.getCurrentUser();
    }
    catch (e) {
      console.log(e);
      location.href = '/api/Account/Login';
    }
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  logout() {
    location.href = "/api/Account/Logout";
  }

  checkRole(user: User, role: string) {
    return isUserInRole(user, role);
  }

}
