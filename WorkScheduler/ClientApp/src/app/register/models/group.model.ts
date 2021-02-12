import { guid } from "../../shared/guid";
import { AcademicYear } from "../../shared/models/academic-year.model";
import { Dictionary } from "../../shared/models/dictionary.model";
import { Student } from "../../shared/models/student";
import { AssociationType } from "./enums/association-type.enum";

export class Group extends Dictionary<number> {
    students: Student[];
    academicYear: AcademicYear;
    associationType: AssociationType;

    //frontOnly
    creationId: string = guid();
}