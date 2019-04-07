import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserState } from '../../shared/states/user.state';
import { User } from '../../shared/models/user';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ScheduleService } from '../services/schedule.service';
import { DictionaryService } from '../../shared/services/dictionary.service';

@Component({
  selector: 'app-home',
  templateUrl: './scheduler-home.component.html',
})
export class SchedulerHomeComponent implements OnInit {

  constructor(private http: HttpClient, private userState: UserState, private schedule: ScheduleService, private dictionary: DictionaryService, private ngxService: NgxUiLoaderService) {
  }

  async ngOnInit() {
    try {
        this.ngxService.start();
        this.userState.currentUser.state = await this.http.get<User>('/api/Account/GetCurrentUserInfo').toPromise();

        let notifications = await this.dictionary.getNotifications();
        this.userState.assignedTicketCount.state = parseInt(notifications.filter(n => n.id == 'assignedTickets')[0].name);

        this.userState.assignedTickets.state = await this.schedule.assignedTickets();


      setInterval(async () => {
        let notifications = await this.dictionary.getNotifications();
        this.userState.assignedTicketCount.state = parseInt(notifications.filter(n => n.id == 'assignedTickets')[0].name);

        if (this.userState.assignedTickets.state.length != this.userState.assignedTicketCount.state) {
          this.userState.assignedTickets.state = await this.schedule.assignedTickets();
        }

      }, 20000); 

    } catch (e) {
      location.href = '/api/Account/Login';
    }
    finally {
      this.ngxService.stop();
    }
  }

}
