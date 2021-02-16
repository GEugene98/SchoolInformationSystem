import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { NgSelectModule } from "@ng-select/ng-select";
import { StudentSelectorComponent } from "./monitoring/lists/components/student-selector/student-selector.component";

@NgModule({
    imports: [
        NgSelectModule,
        FormsModule,
        CommonModule,
        BrowserModule
     ],
    declarations: [
        StudentSelectorComponent
    ],
    exports: [
        StudentSelectorComponent
    ]
})
 
export class SharedModule {}