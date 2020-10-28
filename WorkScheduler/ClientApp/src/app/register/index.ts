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
import { RegisterScheduleComponent } from './register-schedule/register-schedule.component';
import { RegisterSettingsComponent } from './register-settings/register-settings.component';
import { FreeTimeComponent } from './free-time/free-time.component';
import { AdditionalComponent } from './additional/additional.component';
import { RegisterMainComponent } from './register-main/register-main.component';
import { RegisterParamsComponent } from './shared/register-params/register-params.component';
import { RegisterTableComponent } from './shared/register-table/register-table.component';
import { RegisterPlaningComponent } from './shared/register-planing/register-planing.component';

defineLocale('ru', ruLocale);

@NgModule({
  declarations: [
    AdditionalComponent,
    FreeTimeComponent,
    RegisterSettingsComponent,
    RegisterScheduleComponent,
    RegisterMainComponent,
    RegisterParamsComponent,
    RegisterTableComponent,
    RegisterPlaningComponent,
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
    MessageService,
    AccountService,
    ProblemService
  ],
  entryComponents: [
  ]
})
export class RegisterModule {

}
