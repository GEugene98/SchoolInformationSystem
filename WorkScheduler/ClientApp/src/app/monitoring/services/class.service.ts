import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Student } from "../../shared/models/student";
import { Class } from "../models/class.model";

@Injectable()
export class ClassService {
    
    constructor(private http: HttpClient) {
    }

    async createClass(newClass: Class) {
        return await this.http.post('api/Class/CreateClass', newClass).toPromise();
    }

    async deleteClass(classId) {
        const params = new HttpParams()
        .set('id', classId.toString());

        return await this.http.delete('api/Class/DeleteClass', { params: params }).toPromise();
    }

}