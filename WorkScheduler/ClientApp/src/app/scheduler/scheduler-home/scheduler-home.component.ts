import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserState } from '../../shared/states/user.state';
import { User } from '../../shared/models/user';

@Component({
  selector: 'app-home',
  templateUrl: './scheduler-home.component.html',
})
export class SchedulerHomeComponent implements OnInit {

  data: any;

  constructor(private http: HttpClient, private userState: UserState) {
    this.data = {
      labels: ['A', 'B', 'C'],
      datasets: [
        {
          data: [300, 50, 100],
          backgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ],
          hoverBackgroundColor: [
            "#FF6384",
            "#36A2EB",
            "#FFCE56"
          ]
        }]
    };
  }

  async ngOnInit() {
    try {
      this.userState.currentUser.state = await this.http.get<User>('/api/Account/GetCurrentUserInfo').toPromise();
    } catch (e) {
      location.href = '/api/Account/Login';
    }
  }

}
