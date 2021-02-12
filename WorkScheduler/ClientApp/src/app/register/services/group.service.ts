import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AssociationType } from "../models/enums/association-type.enum";
import { Group } from "../models/group.model";

@Injectable()
export class GroupService {
    constructor(private http: HttpClient) { }

    async getGroups(type: AssociationType, academicYearId: number) {
        const params = new HttpParams()
            .set('type', type.toString())
            .set('academicYearId', academicYearId.toString());
  
        return await this.http.get<Group[]>('api/Group/GetGroups', { params: params } ).toPromise();
    }

    async createGroup(group: Group, academicYearId: number) {
        const params = new HttpParams()
            .set('academicYearId', academicYearId.toString());
  
        return await this.http.post('api/Group/CreateGroup', group, { params: params }).toPromise();
    }

}