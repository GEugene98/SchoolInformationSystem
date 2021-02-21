import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AssociationType } from "../models/enums/association-type.enum";
import { Group } from "../models/group.model";
import { ImportPlaning } from "../models/import-planing.model";
import { PlaningRecord } from "../models/planing-record.model";

@Injectable()
export class RegisterPlaningService {
    constructor(private http: HttpClient) { }

    async uploadPlaningExcel(formData, importModel: ImportPlaning) {
        const params = new HttpParams().set('importModelJSON', JSON.stringify(importModel));
        return await this.http.post('api/PlaningRecord/UploadPlaningExcel', formData, { params: params }).toPromise();
    }

    async getRecords(academicYearId: number, associationId: number, groupId: number){
        const params = new HttpParams()
            .set('associationId', associationId.toString())
            .set('groupId', groupId.toString())
            .set('academicYearId', academicYearId.toString());
  
        return await this.http.get<PlaningRecord[]>('api/PlaningRecord/GetRecords', { params: params } ).toPromise();
    }

    async updateRecord(record: PlaningRecord){
        return await this.http.post('api/PlaningRecord/UpdateRecord', record).toPromise();
    }

    async deleteRecord(recordId: number){
        const params = new HttpParams()
            .set('recordId', recordId.toString());

        return await this.http.delete('api/PlaningRecord/DeleteRecord', { params: params }).toPromise();
    }

}