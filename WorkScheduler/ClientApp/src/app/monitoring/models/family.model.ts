import { Student } from "../../shared/models/student";
import { ClarifyFamilyСomposition } from "./clarifyfamilycomposition.model";
import { FamilyСomposition } from "./familycomposition.model";
import { FamilyNumberChildren } from "./familynumberchildren.model";
import { FamilyQualityLife } from "./familyqualitylife.model";
import { HealthGroup } from "./healthgroup.model";
import { PhysicalGroup } from "./physicalgroup.model";
import { Registration } from "./registration.model";

export class Family{

    constructor(){

    }
    id: number; //id
    student: Student; // От сюда берем др и фио
    passportNumber: string; // Отвечает за паспорт
    birthCertificate: string; // Свидетельство о рождении
    issuedWhom: string; //Кем выдан
    whenIssued: string; //Когда выдано
    registrAddres: string; //Адресс регистрации
    residAddres: string;  //Адресс проживания
    fullNameMather: string; //ФИО матери
    phoneMother: number; //Телефон матери
    workMother: string; //Работа матери
    fullNameFather: string; //ФИО отца
    phoneFather: number; //Телефон отца
    workFather: string;  //Работа отца
    composition: FamilyСomposition; //Категория семьи по составу
    clarifyСomposition: ClarifyFamilyСomposition; //Подкатегории семьи по составу
    numberChildren: FamilyNumberChildren; //Кол-во детей
    qualityLife: FamilyQualityLife; //Качество жизни
    healthGroup: HealthGroup; //Группа здоровья
    physicalGroup: PhysicalGroup; //Физическая группа
    registration: Registration; // Учет
    date: Date; // Учет
}