import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ruLocale } from 'ngx-bootstrap/locale';
import { UserState } from './shared/states/user.state';
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
import { ChecklistsComponent } from './scheduler/checklists/checklists.component';
import { ChartModule } from 'primeng/chart';
import { ChecklistDetailsComponent } from './scheduler/checklist-details/checklist-details.component';
import { UploadModule } from '@progress/kendo-angular-upload';
import { CallboardComponent } from './dashboard/components/callboard/callboard.component';
import { CallboardService } from './shared/services/callboard.service';
import { ToastModule } from 'primeng/toast';
import { ProtocolsComponent } from './scheduler/protocols/protocols.component';
import { ProtocolDetailsComponent } from './scheduler/protocols/components/protocol-details/protocol-details.component';
import { RegisterModule } from './register';
import { RegisterMainComponent } from './register/register-main/register-main.component';
import { AdditionalComponent } from './register/additional/additional.component';
import { FreeTimeComponent } from './register/free-time/free-time.component';
import { RegisterSettingsComponent } from './register/register-settings/register-settings.component';
import { RegisterScheduleComponent } from './register/register-schedule/register-schedule.component';
import { RegisterGpdComponent } from './register/register-gpd/register-gpd.component';
import { MonitoringsMainComponent } from './monitoring/monitorings-main/monitorings-main.component';
import { MonitoringModule } from './monitoring';
import { ListsComponent } from './monitoring/lists/lists.component';
import { ContractsComponent } from './monitoring/contracts/contracts.component';
import { TabsModule } from 'ngx-bootstrap/tabs'; 
import { ModalModule } from 'ngx-bootstrap/modal';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FamilyComponent } from './monitoring/family/family.component';
import { WorkflowModule } from './workflow';
import { WorkflowMainComponent } from './workflow/workflow-main/workflow-main.component';
import { IncomingComponent } from './workflow/incoming/incoming.component';
import { OutgoingComponent } from './workflow/outgoing/outgoing.component';

defineLocale('ru', ruLocale);

const routes = [
  { path: '', component: DashboardComponent, pathMatch: 'full' },
  { path: 'settings', component: SettingsComponent, pathMatch: 'full'  },
  {
    path: 'scheduler', loadChildren: () => import('./scheduler/index').then(m => m.SchedulerModule),
    component: SchedulerHomeComponent,
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
  },
  {
    path: 'register', loadChildren: () => import('./register/index').then(m => m.RegisterModule),
    component: RegisterMainComponent,
    children: [
      { path: 'additional', component: AdditionalComponent },
      { path: 'free-time', component: FreeTimeComponent },
      { path: 'gpd', component: RegisterGpdComponent },
      { path: 'register-settings', component: RegisterSettingsComponent },
      { path: 'schedule', component: RegisterScheduleComponent }
    ]
  },
  {   path: 'monitorings', loadChildren: () => import('./monitoring/index').then(m => m.MonitoringModule),
      component: MonitoringsMainComponent,
      children: [
        { path: 'lists', component: ListsComponent },
        { path: 'contracts', component: ContractsComponent },
        { path: 'family', component: FamilyComponent}
      ]
  },
  {   path: 'workflow', loadChildren: () => import('./workflow/index').then(m => m.WorkflowModule),
      component: WorkflowMainComponent,
      children: [
        { path: 'incoming', component: IncomingComponent },
        { path: 'outgoing', component: OutgoingComponent },
      ]
  }
];


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
    MonitoringModule,
    ModalModule.forRoot(),
    FormsModule,
    SchedulerModule,
    RegisterModule,
    WorkflowModule,
    RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' }),
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
