import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { AcademicYear } from "../models/academic-year.model";
import { Activity } from "../models/activity.model";
import { User } from "../models/user";
import { ConfirmationForm } from "../models/confirmation-form.model";
import { Dictionary } from "../models/dictionary.model";

@Injectable()
export class ProblemService {

  constructor(private http: HttpClient) {

        
  }

   async sendReport(report){
    const params = new HttpParams()
    .set('report', report);

    return await this.http.get('api/Problem/Report', { params: params }).toPromise();
    }
}