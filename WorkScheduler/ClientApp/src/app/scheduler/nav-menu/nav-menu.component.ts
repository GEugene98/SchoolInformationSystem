import { Component } from '@angular/core';
import { UserState } from '../../shared/states/user.state';
import { isUserInRole, User } from '../../shared/models/user';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(public userState: UserState) {
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
}
