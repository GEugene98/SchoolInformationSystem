import { Student } from "../../shared/models/student";
import { RegisterRecord } from "./register-record.model";

export class RegisterRow {
    student: Student;
    cells: RegisterRecord[];
}