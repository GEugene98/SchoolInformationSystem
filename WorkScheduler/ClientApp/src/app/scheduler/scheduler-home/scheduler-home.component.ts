import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserState } from '../../shared/states/user.state';
import { User } from '../../shared/models/user';
import { ScheduleService } from '../services/schedule.service';
import { DictionaryService } from '../../shared/services/dictionary.service';

@Component({
  selector: 'app-home',
  templateUrl: './scheduler-home.component.html',
  styleUrls: ['./scheduler-home.component.css']
})
export class SchedulerHomeComponent implements OnInit {

  constructor(private http: HttpClient, private userState: UserState, private schedule: ScheduleService, private dictionary: DictionaryService) {
  }

  async ngOnInit() {
    try {
        this.userState.currentUser.state = await this.http.get<User>('/api/Account/GetCurrentUserInfo').toPromise();

        let notifications = await this.dictionary.getNotifications();
        this.userState.assignedTicketCount.state = parseInt(notifications.filter(n => n.id == 'assignedTickets')[0].name);
        this.userState.schedulesToAccept.state = parseInt(notifications.filter(n => n.id == 'schedulesToAccept')[0].name);
        this.userState.schedulesToConfirm.state = parseInt(notifications.filter(n => n.id == 'schedulesToConfirm')[0].name);
        this.userState.unseenChecklistTickets.state = parseInt(notifications.filter(n => n.id == 'unseenChecklistTickets')[0].name);

        this.userState.assignedTickets.state = await this.schedule.assignedTickets();


      setInterval(async () => {
        let notifications = await this.dictionary.getNotifications();
        this.userState.assignedTicketCount.state = parseInt(notifications.filter(n => n.id == 'assignedTickets')[0].name);
        this.userState.schedulesToAccept.state = parseInt(notifications.filter(n => n.id == 'schedulesToAccept')[0].name);
        this.userState.schedulesToConfirm.state = parseInt(notifications.filter(n => n.id == 'schedulesToConfirm')[0].name);
        this.userState.unseenChecklistTickets.state = parseInt(notifications.filter(n => n.id == 'unseenChecklistTickets')[0].name);

        if (this.userState.assignedTickets.state.length != this.userState.assignedTicketCount.state) {
          this.userState.assignedTickets.state = await this.schedule.assignedTickets();
        }

      }, 20000); 

    } catch (e) {
      location.href = '/api/Account/Login';
    }
    finally {
    }
  }

}
