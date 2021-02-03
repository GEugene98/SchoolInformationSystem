import { AcademicYear } from "../../shared/models/academic-year.model";
import { Dictionary } from "../../shared/models/dictionary.model";
import { School } from "../../shared/models/school";
import { Student } from "../../shared/models/student";

export class Class extends Dictionary<number> {
    schoolId: number;
    school: School;

    academicYearId: number;
    academicYear: AcademicYear;

    students: Student[];
}