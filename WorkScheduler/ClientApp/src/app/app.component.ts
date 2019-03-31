import { Component, OnInit } from '@angular/core';
import { setTheme } from 'ngx-bootstrap/utils';
import { HttpClient } from '@angular/common/http';
import { UserState } from './shared/states/user.state';
import { User } from './shared/models/user';
import { ScheduleService } from './scheduler/services/schedule.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor() {
    setTheme('bs3');

    

  }

 
}
