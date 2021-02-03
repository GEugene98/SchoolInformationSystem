import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule, ModalModule, PaginationModule, defineLocale, ruLocale } from 'ngx-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MessageService } from 'primeng/components/common/messageservice';
import { GrowlModule } from 'primeng/growl';
import { ToastModule } from 'primeng/toast';
import { ChartModule } from 'primeng/chart';
import { UserState } from '../shared/states/user.state';
import { DictionaryService } from '../shared/services/dictionary.service';
import { AccountService } from '../shared/services/account.service';
import { TooltipModule } from 'ng2-tooltip-directive';
import { ProblemService } from '../shared/services/problem.service';
import { UploadModule } from '@progress/kendo-angular-upload';
import { MonitoringsMainComponent } from './monitorings-main/monitorings-main.component';
import { ListsComponent } from './lists/lists.component';
import { ClassesComponent } from './lists/classes/classes.component';
import { StudentsComponent } from './lists/students/students.component';
import { AccordionModule } from 'primeng/accordion';
import { ContractsComponent } from './contracts/contracts.component';
import { StudentService } from './services/student.service';
import { StudentSelectorComponent } from './lists/components/student-selector/student-selector.component';

defineLocale('ru', ruLocale);

@NgModule({
  declarations: [
      MonitoringsMainComponent,
      ListsComponent,
      StudentsComponent,
      ClassesComponent,
      ContractsComponent,
      StudentSelectorComponent
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
    AccordionModule,
    PaginationModule.forRoot()
  ],
  providers: [
    UserState,
    DictionaryService,
    MessageService,
    AccountService,
    ProblemService,
    StudentService
  ],
  entryComponents: [
  ]
})
export class MonitoringModule {

}
