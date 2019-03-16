import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule, ModalModule, TimepickerModule } from 'ngx-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';

import { defineLocale } from 'ngx-bootstrap/chronos';
import { ruLocale } from 'ngx-bootstrap/locale';
import { UserState } from './shared/states/user.state';
import { DictionaryService } from './shared/services/dictionary.service';
import { AccountService } from './shared/services/account.service';
import { MessageService } from 'primeng/api';
import { SchedulerModule } from './scheduler';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SchedulerHomeComponent } from './scheduler/scheduler-home/scheduler-home.component';
import { MySchedules } from './scheduler/my-schedules/my-schedules.component';
import { GeneralScheduleComponent } from './scheduler/general-schedule/general-schedule.component';
import { SettingsComponent } from './scheduler/settings/settings.component';
import { ConfirmComponent } from './scheduler/confirm/confirm.component';
import { AcceptComponent } from './scheduler/accept/accept.component';
import { ScheduleDetailsComponent } from './scheduler/schedule-details/schedule-details.component';
import { TimelineComponent } from './scheduler/timeline/timeline.component';
import { NgxUiLoaderModule, NgxUiLoaderConfig } from 'ngx-ui-loader';
import { ActionComponent } from './scheduler/action/action.component';
import { ConfirmAcceptScheduleComponent } from './scheduler/confirm-accept-schedule/confirm-accept-schedule.component';
import { ChecklistsComponent } from './scheduler/checklists/checklists.component';

defineLocale('ru', ruLocale);

const routes = [
  { path: '', component: DashboardComponent, pathMatch: 'full' },
  {
    path: 'scheduler', component: SchedulerHomeComponent,
    children: [
      { path: 'my-schedule', component: MySchedules },
      { path: 'general-schedule', component: GeneralScheduleComponent },
      { path: 'settings', component: SettingsComponent },
      { path: 'confirm', component: ConfirmComponent },
      { path: 'accept', component: AcceptComponent },
      { path: 'timeline', component: TimelineComponent },
      { path: 'checklists', component: ChecklistsComponent },
      { path: 'schedule-details/:id', component: ScheduleDetailsComponent },
    ]
  }
];

const ngxUiLoaderConfig: NgxUiLoaderConfig =
{
  "bgsColor": "rgb(76,175,80)",
  "bgsOpacity": 0.5,
  "bgsPosition": "bottom-right",
  "bgsSize": 60,
  "bgsType": "fading-circle",
  "blur": 0,
  "fgsColor": "rgb(76,175,80)",
  "fgsPosition": "center-center",
  "fgsSize": 60,
  "fgsType": "fading-circle",
  "gap": 24,
  "logoPosition": "center-center",
  "logoSize": 120,
  "logoUrl": "",
  "overlayColor": "rgba(40, 40, 40, 0.8)",
  "pbColor": "rgb(76,175,80)",
  "pbDirection": "ltr",
  "pbThickness": 3,
  "hasProgressBar": false,
  "text": "",
  "textColor": "#FFFFFF",
  "textPosition": "center-center",
  "threshold": 500
}

@NgModule({
  declarations: [ 
    AppComponent,
    DashboardComponent,
    ChecklistsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    BrowserAnimationsModule,
    NgSelectModule,
    TabsModule,
    //TabsModule.forRoot(),
    //BsDatepickerModule.forRoot(),
    //ModalModule.forRoot(),
    FormsModule,
    SchedulerModule,
    NgxUiLoaderModule.forRoot(ngxUiLoaderConfig),
    RouterModule.forRoot(routes)
  ],
  providers: [
    UserState,
  ],
  entryComponents: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
