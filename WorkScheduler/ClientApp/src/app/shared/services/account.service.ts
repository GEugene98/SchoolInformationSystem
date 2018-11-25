import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Register } from "../models/register.model";

@Injectable()
export class AccountService {

  constructor(private http: HttpClient) {

  }

  async register(model: Register) {
    return await this.http.post<any>('api/Account/Register', model).toPromise();
  }

  async block(userId: string) {
    const params = new HttpParams()
      .set('userId', userId.toString());

    return await this.http.get('api/Account/Block', { params: params }).toPromise();
  }
}
