import { Family } from './../models/family.model';
import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";

@Injectable()
export class FamilyService {
     

    constructor (private http: HttpClient) {

    }

    async getFamilies(classId: number) {
      const params = new HttpParams()
        .set('classId', classId.toString());
      return await this.http.get<Family[]>('api/Family', { params: params }).toPromise();
    }

    async upDateFamily(family: Family) {
        return await this.http.put('api/Family', family).toPromise();
    }

    async createFamily(family: Family) {
        return await this.http.post('api/Family', family).toPromise();
    }
   
    async deleteFamily(familyId: number) {
        const params = new HttpParams()
        .set('id', familyId.toString());
        return await this.http.delete('api/Family', { params: params }).toPromise();
    }
}