import { defineLocale, ModalModule, BsDatepickerModule, TabsModule, TooltipModule } from "ngx-bootstrap";
import { BrowserModule } from "@angular/platform-browser";
import { HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgSelectModule } from "@ng-select/ng-select";
import { NgModule } from "@angular/core";
import { ruLocale } from 'ngx-bootstrap/locale';
import { FormsModule } from "@angular/forms";
import { ChartModule } from "primeng/chart";
import { ToastModule } from "primeng/toast";
import { GrowlModule } from "primeng/growl";
import { UserState } from "../shared/states/user.state";
import { DictionaryService } from "../shared/services/dictionary.service";
import { MessageService } from "primeng/api";
import { AccountService } from "../shared/services/account.service";
import { ProblemService } from "../shared/services/problem.service";
import { RouterModule } from "@angular/router";
import { CatalogHomeComponent } from "./catalog-home/catalog-home.component";
import { StudentsComponent } from "./catalogs/students/students.component";

defineLocale('ru', ruLocale);

@NgModule({
  declarations: [
    CatalogHomeComponent,
    StudentsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
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
    RouterModule
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
export class CatalogModule {

}