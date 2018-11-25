import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AcademicYear } from "../models/academic-year.model";
import { Activity } from "../models/activity.model";
import { User } from "../models/user";
import { ConfirmationForm } from "../models/confirmation-form.model";
import { Dictionary } from "../models/dictionary.model";

@Injectable()
export class DictionaryService {

  constructor(private http: HttpClient) {

  }

  async getAcademicYears() {
    return await this.http.get<AcademicYear[]>('api/Dictionary/AcademicYears').toPromise();
  }

  async getActivities() {
    return await this.http.get<Activity[]>('api/Dictionary/Activities').toPromise();
  }

  async getResponsibles() {
    return await this.http.get<User[]>('api/Dictionary/Responsibles').toPromise();
  }

  async getConfForms() {
    return await this.http.get<ConfirmationForm[]>('api/Dictionary/ConfirmationForms').toPromise();
  }

  async getUsers() {
    return await this.http.get<User[]>('api/Dictionary/Users').toPromise();
  }

  async getRoles() {
    return await this.http.get<Dictionary<string>[]>('api/Dictionary/Roles').toPromise();
  }

  //async getActivities() {
  //  return await this.http.get<Activity[]>('api/Dictionary/Activities').toPromise();
  //}

  //async getActivities() {
  //  return await this.http.get<Activity[]>('api/Dictionary/Activities').toPromise();
  //}
}
