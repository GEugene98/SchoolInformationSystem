import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Student } from "../../shared/models/student";
import { Class } from "../models/class.model";

@Injectable()
export class StudentService {
    
    constructor(private http: HttpClient) {

    }

    async getStudents() {
        return await this.http.get<Student[]>('api/Student/GetStudents').toPromise();
    }

    async getStudentsByClasses(academicYearId: number) {
        const params = new HttpParams()
            .set('academicYearId', academicYearId.toString());

        return await this.http.get<Class[]>('api/Student/GetStudentsByClasses', { params: params }).toPromise();
    }

    async putStudentsToClass(studentIds, classId, academicYearId) {
        const params = new HttpParams()
            .set('classId', classId.toString())
            .set('academicYearId', academicYearId.toString());

        return await this.http.post('api/Student/PutStudentsToClass', studentIds, { params: params } ).toPromise();
    }

}