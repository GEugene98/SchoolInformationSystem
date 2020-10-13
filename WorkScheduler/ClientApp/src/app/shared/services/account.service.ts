import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Register } from "../models/register.model";
import { User } from "../models/user";

@Injectable()
export class AccountService {

  constructor(private http: HttpClient) {

  }

  async getCurrentUser() {
    return await this.http.get<User>('/api/Account/GetCurrentUserInfo').toPromise();
  }

  async register(model: Register) {
    return await this.http.post<any>('api/Account/Register', model).toPromise();
  }

  async setPermission(userId: string, permission: string, value: boolean) {
    const params = new HttpParams()
      .set('userId', userId)
      .set('permission', permission)
      .set('value', value.toString());

    return await this.http.get('/api/Account/SetPermission', { params: params }).toPromise();
  }

  async block(userId: string) {
    const params = new HttpParams()
      .set('userId', userId.toString());

    return await this.http.get('api/Account/Block', { params: params }).toPromise();
  }

  async getNewPassword(userId: string) {
    const params = new HttpParams()
      .set('userId', userId.toString());

    return await this.http.get<string>('api/Account/NewPassword', { params: params }).toPromise();
  }
}
