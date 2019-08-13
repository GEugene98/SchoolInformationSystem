import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule, ModalModule, PaginationModule } from 'ngx-bootstrap';
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
import { ConfirmAcceptScheduleComponent } from './confirm-accept-schedule/confirm-accept-schedule.component';
import { RepeatComponent } from './timeline/components/repeat/repeat.component';
import { TooltipModule } from 'ng2-tooltip-directive';
import { ChecklistsComponent } from './checklists/checklists.component';
import { ChecklistDetailsComponent } from './checklist-details/checklist-details.component';
import { OtherChecklistsComponent } from './other-checklists/other-checklists.component';
import { ProblemService } from '../shared/services/problem.service';
import { UploadModule } from '@progress/kendo-angular-upload';
defineLocale('ru', ruLocale); 

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
    ConfirmAcceptScheduleComponent,
    ActionComponent,
    RepeatComponent,
    ChecklistsComponent,
    ChecklistDetailsComponent,
    OtherChecklistsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    MatDialogModule,
    BrowserAnimationsModule,
    NgSelectModule,
    GrowlModule,
    TooltipModule,
    ToastModule,
    ChartModule,
    TabsModule,
    TabsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    FormsModule,
    RouterModule.forChild([]),
    UploadModule,
    PaginationModule.forRoot()
  ],
  providers: [
    UserState,
    DictionaryService,
    ScheduleService,
    MessageService,
    AccountService, 
    ProblemService
  ],
  entryComponents: [
  ]
})
export class SchedulerModule {

}
