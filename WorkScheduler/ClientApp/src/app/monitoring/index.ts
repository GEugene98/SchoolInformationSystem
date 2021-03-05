import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule } from 'ngx-bootstrap/tabs'; 
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ChartModule } from 'primeng/chart';
import { UserState } from '../shared/states/user.state';
import { DictionaryService } from '../shared/services/dictionary.service';
import { AccountService } from '../shared/services/account.service';
import { ProblemService } from '../shared/services/problem.service';
import { UploadModule } from '@progress/kendo-angular-upload';
import { MonitoringsMainComponent } from './monitorings-main/monitorings-main.component';
import { ListsComponent } from './lists/lists.component';
import { ClassesComponent } from './lists/classes/classes.component';
import { StudentsComponent } from './lists/students/students.component';
import { ContractsComponent } from './contracts/contracts.component';
import { StudentService } from './services/student.service';
import { ClassService } from './services/class.service';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ruLocale } from 'ngx-bootstrap/locale';

import { ContractService } from './services/contract.service';
import { SharedModule } from '../shared.module';
import { DatePipe } from '@angular/common';


defineLocale('ru', ruLocale);

@NgModule({
  declarations: [
      MonitoringsMainComponent,
      ListsComponent,
      StudentsComponent,
      ClassesComponent,
      ContractsComponent,
      
  ],
  imports: [
    SharedModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    BrowserAnimationsModule,
    NgSelectModule,
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
    MessageService,
    AccountService,
    ProblemService,
    StudentService,
    ClassService,
    ContractService,
    DatePipe,
    BsModalService
  ],
  entryComponents: [
  ]
})
export class MonitoringModule {

}
