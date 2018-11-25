import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule, ModalModule } from 'ngx-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SchedulerHomeComponent } from './scheduler-home/scheduler-home.component';
import { MySchedules } from './my-schedules/my-schedules.component';
import { MatDialogModule } from '@angular/material/dialog';
import { GeneralScheduleComponent } from './general-schedule/general-schedule.component';
import { DayComponent } from './general-schedule/components/day/day.component';
import { WeekComponent } from './general-schedule/components/week/week.component';
import { MonthComponent } from './general-schedule/components/month/month.component';
import { YearComponent } from './general-schedule/components/year/year.component';
import { SettingsComponent } from './settings/settings.component';
import { ConfirmComponent } from './confirm/confirm.component';
import { AcceptComponent } from './accept/accept.component';
import { MessageService } from 'primeng/components/common/messageservice';
import { GrowlModule } from 'primeng/growl';
import { ToastModule } from 'primeng/toast';
import { ChartModule } from 'primeng/chart';

import { defineLocale } from 'ngx-bootstrap/chronos';
import { ruLocale } from 'ngx-bootstrap/locale';
import { UserState } from '../shared/states/user.state';
import { ScheduleService } from './services/schedule.service';
import { DictionaryService } from '../shared/services/dictionary.service';

import { ScheduleDetailsComponent } from './schedule-details/schedule-details.component';
import { AccountService } from '../shared/services/account.service';
import { TimelineComponent } from './timeline/timeline.component';
import { ActionComponent } from './action/action.component';
defineLocale('ru', ruLocale); 

const schedulerRoutes = [
  //{ path: 'my-schedule', component: MySchedules, outlet: "schedulerRouter" },
  //{ path: 'general-schedule', component: GeneralScheduleComponent, outlet: "schedulerRouter" },
  //{ path: 'settings', component: SettingsComponent, outlet: "schedulerRouter" },
  //{ path: 'confirm', component: ConfirmComponent, outlet: "schedulerRouter" },
  //{ path: 'accept', component: AcceptComponent, outlet: "schedulerRouter" },
  //{ path: 'schedule-details/:id', component: ScheduleDetailsComponent, outlet: "schedulerRouter" }
];

@NgModule({
  declarations: [
    NavMenuComponent,
    SchedulerHomeComponent,
    MySchedules,
    GeneralScheduleComponent,
    DayComponent,
    WeekComponent,
    MonthComponent,
    YearComponent,
    SettingsComponent,
    ConfirmComponent,
    AcceptComponent,
    ScheduleDetailsComponent,
    TimelineComponent,
    ActionComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    MatDialogModule,
    BrowserAnimationsModule,
    NgSelectModule,
    GrowlModule,
    ToastModule,
    ChartModule,
    TabsModule,
    TabsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    FormsModule,
    RouterModule.forChild(schedulerRoutes),
    //RouterModule.forRoot()
  ],
  providers: [
    UserState,
    DictionaryService,
    ScheduleService,
    MessageService,
    AccountService
  ],
  entryComponents: [
  ]
})
export class SchedulerModule {

}
