import { Component, OnInit } from '@angular/core';
import { UserState } from '../../shared/states/user.state';

@Component({
  selector: 'app-workflow-main',
  templateUrl: './workflow-main.component.html',
  styleUrls: ['./workflow-main.component.css']
})
export class WorkflowMainComponent implements OnInit {

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
