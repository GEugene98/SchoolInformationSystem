import { Component, OnInit } from '@angular/core';
import { UserState } from '../../shared/states/user.state';

@Component({
  selector: 'app-register-main',
  templateUrl: './register-main.component.html',
  styleUrls: ['./register-main.component.css']
})
export class RegisterMainComponent implements OnInit {

  isExpanded = false;

  constructor(public userState: UserState) { }

  ngOnInit() {
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

}
