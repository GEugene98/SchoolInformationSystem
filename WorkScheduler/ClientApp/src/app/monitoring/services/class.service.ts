import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Student } from "../../shared/models/student";

@Injectable()
export class ClassService {
    
    constructor(private http: HttpClient) {

    }

    async getClasses() {
        return await this.http.get<Student[]>('api/Student/GetStudents').toPromise();
    }

}