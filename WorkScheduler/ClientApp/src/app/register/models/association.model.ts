import { AcademicYear } from "../../shared/models/academic-year.model";
import { Dictionary } from "../../shared/models/dictionary.model";
import { AssociationType } from "./enums/association-type.enum";
import { Group } from "./group.model";

export class Association extends Dictionary<number>{
    academicYear: AcademicYear;
    type: AssociationType;
    groups: Group[];
}