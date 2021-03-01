import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PlaningRecord } from "../models/planing-record.model";
import { RegisterRow } from "../models/register-row.model";

@Injectable()
export class RegisterService {
    constructor(private http: HttpClient) { }

    async getRecords(academicYearId: number, associationId: number, groupId: number){
        const params = new HttpParams()
            .set('associationId', associationId.toString())
            .set('groupId', groupId.toString())
            .set('academicYearId', academicYearId.toString());
  
        return await this.http.get<RegisterRow[]>('api/Register/GetRecords', { params: params } ).toPromise();
    }

    async updateCell(planingRecordId: number, studentId: string, cellId: number, content: string){
        const params = new HttpParams()
            .set('planingRecordId', planingRecordId.toString())
            .set('studentId', studentId)
            .set('cellId', cellId.toString())
            .set('content', content);
  
        return await this.http.get<RegisterRow[]>('api/Register/MakeMark', { params: params } ).toPromise();
    }
}