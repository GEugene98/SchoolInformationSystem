import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AssociationType } from "../models/enums/association-type.enum";
import { Group } from "../models/group.model";

@Injectable()
export class RegisterPlaningService {
    constructor(private http: HttpClient) { }

    async uploadPlaningExcel(formData) {
        return await this.http.post('api/PlaningRecord/UploadPlaningExcel', formData).toPromise();
    }


}