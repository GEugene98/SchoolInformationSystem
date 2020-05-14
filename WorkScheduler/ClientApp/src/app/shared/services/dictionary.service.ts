import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { AcademicYear } from "../models/academic-year.model";
import { Activity } from "../models/activity.model";
import { User } from "../models/user";
import { ConfirmationForm } from "../models/confirmation-form.model";
import { Dictionary } from "../models/dictionary.model";
import { WorkSchedule } from "../models/work-schedule.model";

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

  async saveUser(user: User) {
    return await this.http.post('api/Dictionary/SaveUser', user).toPromise();
  }

  async getRoles() {
    return await this.http.get<Dictionary<string>[]>('api/Dictionary/Roles').toPromise();
  }

  async getUserActivity(userId: string) {
    const params = new HttpParams()
      .set('userId', userId.toString());

    return await this.http.get<string[]>('api/Dictionary/UserActivity', { params: params }).toPromise();
  }

  async getAllActivity(range: Date[]) {
    return await this.http.post<string[]>('api/Dictionary/AllActivity', range).toPromise();
  }

  async getUserSchedules(){
    return await this.http.get<WorkSchedule[]>('api/Dictionary/MySchedules').toPromise();
  }

  async getNotifications() {
    return await this.http.get<Dictionary<string>[]>('api/Dictionary/Notifications').toPromise();
  }

  async getActionNames() {
    return await this.http.get<string[]>('api/Dictionary/ActionNames').toPromise();
  }

  async updateActionNames(actionNames) {
    return await this.http.post('api/Dictionary/UpdateActionNames', { actionNames: actionNames.toString() }).toPromise();
  }

  //async getActivities() {
  //  return await this.http.get<Activity[]>('api/Dictionary/Activities').toPromise();
  //}

  //async getActivities() {
  //  return await this.http.get<Activity[]>('api/Dictionary/Activities').toPromise();
  //}
}
