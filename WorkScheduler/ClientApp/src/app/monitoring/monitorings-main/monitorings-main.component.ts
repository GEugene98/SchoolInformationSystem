import { Component, OnInit } from '@angular/core';
import { UserState } from '../../shared/states/user.state';

@Component({
  selector: 'app-monitorings-main',
  templateUrl: './monitorings-main.component.html',
  styleUrls: ['./monitorings-main.component.css']
})
export class MonitoringsMainComponent implements OnInit {

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
