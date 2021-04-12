import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";

@Injectable()
export class FamilyService {
     

    constructor (private http: HttpClient) {

    }

    async getFamilies() {
        return await this.http.get<any>('api/Family').toPromise();
    }

   
}