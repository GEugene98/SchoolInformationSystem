import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Student } from "../../shared/models/student";

@Injectable()
export class StudentService {
    
    constructor(private http: HttpClient) {

    }

    async GetStudents() {
        return await this.http.get<Student[]>('api/Student/GetStudents').toPromise();
    }

}