import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule } from 'ngx-bootstrap/tabs'; 
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastModule } from 'primeng/toast';
import { ChartModule } from 'primeng/chart';
import { UserState } from '../shared/states/user.state';
import { DictionaryService } from '../shared/services/dictionary.service';
import { AccountService } from '../shared/services/account.service';
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
import { RegisterGpdComponent } from './register-gpd/register-gpd.component';
import { RegisterTableSettingsComponent } from './shared/register-table-settings/register-table-settings.component';
import { GroupService } from './services/group.service';
import { AssociationService } from './services/association.service';
import { CreateGroupComponent } from './shared/create-group/create-group.component';
import { SharedModule } from '../shared.module';
import { RegisterPlaningService } from './services/register-planing.service';
import { RegisterService } from './services/register.service';
import { MessageService } from 'primeng/api';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ruLocale } from 'ngx-bootstrap/locale';
import { TieredMenuModule } from 'primeng/tieredmenu';

defineLocale('ru', ruLocale);

@NgModule({
  declarations: [
    AdditionalComponent,
    FreeTimeComponent,
    RegisterGpdComponent,
    RegisterSettingsComponent,
    RegisterScheduleComponent,
    RegisterMainComponent,
    RegisterParamsComponent,
    RegisterTableComponent,
    RegisterPlaningComponent,
    RegisterTableSettingsComponent,
    CreateGroupComponent,
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
    PaginationModule.forRoot(),
    TieredMenuModule
  ],
  providers: [
    UserState,
    DictionaryService,
    MessageService,
    AccountService,
    ProblemService,
    GroupService,
    AssociationService,
    RegisterPlaningService,
    RegisterService,
    BsModalService
  ],
  entryComponents: [
  ]
})
export class RegisterModule {

}
