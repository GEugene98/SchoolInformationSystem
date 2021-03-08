import { Student } from "../../shared/models/student";
import { ClarifyFamilyСomposition } from "./clarifyfamilycomposition.model";
import { FamilyСomposition } from "./familycomposition.model";
import { FamilyNumberChildren } from "./familynumberchildren.model";

export class Family{

    constructor(){

    }
    id: number;
    student: Student;
    passportNumber: string;
    birthCertificate: string;
    registrAddres: string;
    residAddres: string;
    fullNameMather: string;
    phoneMother: number;
    workMother: string;
    fullNameFather: string;
    phoneFather: number;
    workFather: string;
    composition: FamilyСomposition;
    clarifyСomposition: ClarifyFamilyСomposition;
    numberChildren: FamilyNumberChildren;
}