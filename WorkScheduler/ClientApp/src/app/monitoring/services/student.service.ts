import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Student } from "../../shared/models/student";
import { Class } from "../models/class.model";

@Injectable()
export class StudentService {
    
    public students: Student[];

    constructor(private http: HttpClient) {

    }

    async getStudents() {
        return await this.http.get<Student[]>('api/Student/GetStudents').toPromise();
    }

    async loadStudents() {
        this.students = await this.http.get<Student[]>('api/Student/GetStudents').toPromise();
    }

    async getStudentsByClasses(academicYearId: number) {
        const params = new HttpParams()
            .set('academicYearId', academicYearId.toString());

        return await this.http.get<Class[]>('api/Student/GetStudentsByClasses', { params: params }).toPromise();
    }

    async excludeFromClass(studentId: number, classId: number) {
        const params = new HttpParams()
            .set('studentId', studentId.toString())
            .set('classId', classId.toString());

        return await this.http.get('api/Student/ExcludeFromClass', { params: params }).toPromise();
    }

    async putStudentsToClass(studentIds, classId, academicYearId) {
        const params = new HttpParams()
            .set('classId', classId.toString())
            .set('academicYearId', academicYearId.toString());

        return await this.http.post('api/Student/PutStudentsToClass', studentIds, { params: params } ).toPromise();
    }

    async createStudent(student: Student) {
        return await this.http.post('api/Student/CreateStudent', student).toPromise();
    }    

}