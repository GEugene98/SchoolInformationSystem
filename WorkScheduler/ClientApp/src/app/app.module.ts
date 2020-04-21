import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule, ModalModule, TimepickerModule, ButtonsModule } from 'ngx-bootstrap';
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
import { SettingsComponent } from './dashboard/components/settings/settings.component';
import { ConfirmComponent } from './scheduler/confirm/confirm.component';
import { AcceptComponent } from './scheduler/accept/accept.component';
import { ScheduleDetailsComponent } from './scheduler/schedule-details/schedule-details.component';
import { TimelineComponent } from './scheduler/timeline/timeline.component';
import { NgxUiLoaderModule, NgxUiLoaderConfig } from 'ngx-ui-loader';
import { ActionComponent } from './scheduler/action/action.component';
import { ConfirmAcceptScheduleComponent } from './scheduler/confirm-accept-schedule/confirm-accept-schedule.component';
import { ChecklistsComponent } from './scheduler/checklists/checklists.component';
import { ChartModule } from 'primeng/chart';
import { ChecklistDetailsComponent } from './scheduler/checklist-details/checklist-details.component';
import { OtherChecklistsComponent } from './scheduler/other-checklists/other-checklists.component';
import { UploadModule } from '@progress/kendo-angular-upload';
import { CallboardComponent } from './dashboard/components/callboard/callboard.component';
import { CallboardService } from './shared/services/callboard.service';
import { ToastModule } from 'primeng/toast';
import { ProtocolsComponent } from './scheduler/protocols/protocols.component';
import { ProtocolDetailsComponent } from './scheduler/protocols/components/protocol-details/protocol-details.component';


defineLocale('ru', ruLocale);

const routes = [
  { path: '', component: DashboardComponent, pathMatch: 'full' },
  { path: 'settings', component: SettingsComponent, pathMatch: 'full'  },
  {
    path: 'scheduler', component: SchedulerHomeComponent,
    children: [
      { path: 'my-schedule', component: MySchedules },
      { path: 'protocols', component: ProtocolsComponent },
      { path: 'general-schedule', component: GeneralScheduleComponent },
      { path: 'confirm', component: ConfirmComponent },
      { path: 'accept', component: AcceptComponent },
      { path: 'timeline', component: TimelineComponent },
      { path: 'checklists', component: ChecklistsComponent },
      { path: 'schedule-details/:id', component: ScheduleDetailsComponent },
      { path: 'checklist-details/:id', component: ChecklistDetailsComponent },
      { path: 'protocol-details/:id', component: ProtocolDetailsComponent },
    ]
  }
];

const ngxUiLoaderConfig: NgxUiLoaderConfig =
{
  "bgsColor": "rgb(76,175,80)",
  "bgsOpacity": 0.5,
  "bgsPosition": "bottom-right",
  "bgsSize": 60,
  "bgsType": "square-jelly-box",
  "blur": 0,
  "fgsColor": "rgb(76,175,80)",
  "fgsPosition": "center-center",
  "fgsSize": 60,
  "fgsType": "square-jelly-box",
  "gap": 24,
  "logoPosition": "center-center",
  "logoSize": 120,
  "logoUrl": "",
  "overlayColor": "rgba(40, 40, 40, 0.8)",
  "pbColor": "rgb(76,175,80)",
  "pbDirection": "ltr",
  "pbThickness": 3,
  "hasProgressBar": false,
  "text": "Загрузка данных с сервера...",
  "textColor": "#c5c5c5",
  "textPosition": "center-center",
  "threshold": 500
}

@NgModule({
  declarations: [ 
    AppComponent,
    DashboardComponent,
    SettingsComponent,
    CallboardComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    BrowserAnimationsModule,
    NgSelectModule,
    TabsModule,
    ChartModule,
    ToastModule,
    //TabsModule.forRoot(),
    //BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    FormsModule,
    SchedulerModule,
    NgxUiLoaderModule.forRoot(ngxUiLoaderConfig),
    RouterModule.forRoot(routes),
    UploadModule,
    TabsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    ButtonsModule.forRoot()
  ],
  providers: [
    UserState,
    CallboardService
  ],
  entryComponents: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
