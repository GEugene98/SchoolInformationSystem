import { StudentService } from './../services/student.service';
import { Family } from './../models/family.model';
import { Component, OnInit } from '@angular/core';
import { AcademicYear } from '../../shared/models/academic-year.model';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { Class } from '../models/class.model';
import { ClassService } from '../services/class.service';
import { FamilyService } from '../services/family.service';
import { DatePipe } from '@angular/common';
import * as _ from 'lodash';
import { Student } from '../../shared/models/student';
import { compositions } from '../models/familycomposition.model';
import { clarifyСompositions } from '../models/clarifyfamilycomposition.model';
import { numbersChildren } from '../models/familynumberchildren.model';
import { healthGroups } from '../models/healthgroup.model';
import { physicalGroups } from '../models/physicalgroup.model';
import { registrations } from '../models/registration.model';
import { qualityLifes } from '../models/familyqualitylife.model';
import { MessageService } from 'primeng/api';


@Component({
  selector: 'app-family',
  templateUrl: './family.component.html',
  styleUrls: ['./family.component.css']
})
export class FamilyComponent implements OnInit {

  bsConfig: any;
  displayModalColumns = false;
  displayModalEdit = false;
  addModalVisibility = false;
  editModalVisibility = false;
  selectedAcademicYear: AcademicYear;
  allAcademicYears: AcademicYear[];
  classes: Class[];
  students: Student[];
  selectedClass: Class;
  newFamily: Family = new Family();
  families: Family[];
  modalColumns: any[];

  compositions = compositions;
  clarifycompositions = clarifyСompositions;
  numbersChildren = numbersChildren;
  healthGroups = healthGroups;
  physicalGroups = physicalGroups;
  registrations = registrations;
  qualityLifes = qualityLifes;

  columns = [
    {
      name: "ФИО ребенка",
      visibility: true,
      fieldInfo: "student.fullName"
    },
    {
      name: "Дата рождения",
      visibility: true,
      fieldInfo: "student.birthday"
    },
    {
      name: "Номер свидетельства о рождении (паспорта)",
      visibility: false,
      fieldInfo: [
        "passportNumber",
        "birthCertificate"
      ]
    },
    {
      name: "Адрес регистрации",
      visibility: true,
      fieldInfo: "registrAddres"
    },
    {
      name: "Адрес проживания",
      visibility: true,
      fieldInfo: "residAddres"
    },
    {
      name: "ФИО матери, телефон, место работы",
      visibility: true,
      fieldInfo: [
        "fullNameMather",
        "phoneMother",
        "workMother"
      ]
    },
    {
      name: "ФИО отца, телефон, место работы",
      visibility: false,
      fieldInfo: [
        "fullNameFather",
        "phoneFather",
        "workFather"
      ]
    },
    {
      name: "Категория семьи по составу",
      visibility: false,
      fieldInfo: "clarifyFamilycomposition"
    },
    {
      name: "Категория семьи по количеству детей",
      visibility: false,
      fieldInfo: "familyNumberChildren"
    },
    {
      name: "Категория семьи по качеству жизни",
      visibility: false,
      fieldInfo: "familyQualityLife"
    },
    {
      name: "Группа здоровья",
      visibility: false,
      fieldInfo: "healthGroup"
    },
    {
      name: "Физкультурная группа",
      visibility: false,
      fieldInfo: "physicalGroup"
    },
    {
      name: "Учет",
      visibility: false,
      fieldInfo: "registration"
    },
  ]

  constructor(private dictionary: DictionaryService,
    private datePipe: DatePipe,
    private familyService: FamilyService,
    private studentService: StudentService,
    private messageService: MessageService) {
      this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
     }

  async ngOnInit() {
    await this.loadData();
  }
  
  hideModal(){
    this.columns = _.cloneDeep(this.modalColumns);
    this.displayModalColumns = false;
  }

  showModalDialogColumns() {
      this.modalColumns = _.cloneDeep(this.columns);
      this.displayModalColumns = true;
  }

  showModalDialogEdit(family: Family) {
    this.newFamily = _.cloneDeep(family);
    this.editModalVisibility = true;
  }

  showAddModal() {
    this.newFamily = new Family();
    this.addModalVisibility = true;
  }

  async delete(id: number) {
    await this.familyService.deleteFamily(id);
    await this.loadFamilies();
  }

  async loadData(){
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    this.students = await this.studentService.getStudents();
    this.selectedAcademicYear = this.allAcademicYears[0];
    await this.loadClasses();
    this.selectedClass = this.classes[0];
    await this.loadFamilies();
  }

  async loadClasses() {
    this.classes = await this.studentService.getStudentsByClasses(this.selectedAcademicYear?.id);
  }

  async loadFamilies() {
    this.families = await this.familyService.getFamilies(this.selectedClass?.id);
  }

  async addFamily() {
    try {
      await this.familyService.createFamily(this.newFamily);
    }
    catch (e) {
      console.log(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 10000 });
      return;
    }
    
    await this.loadFamilies();
    this.addModalVisibility = false;
  }

  async editFamily(){
    await this.familyService.updateFamily(this.newFamily);
    await this.loadFamilies();
    this.editModalVisibility = false;
  }

  getPropertyValue(object, fieldInfo:any) {
    let result = '';
    if(Array.isArray(fieldInfo)){
      fieldInfo.forEach((i:string) => {
        let itemResult;
        itemResult = i.split('.')
        .reduce(function(o, k) {
          return o && o[k];
        }, object);
        if(i == "student.birthday"){
          itemResult = this.datePipe.transform(itemResult, "dd.MM.yyyy");
        }
        result += itemResult + '\n';
      });
    }
    else {
      let itemResult;
      itemResult = fieldInfo.split('.')
      .reduce(function(o, k) {
        return o && o[k];
      }, object);
      if(fieldInfo == "student.birthday"){
        itemResult = this.datePipe.transform(itemResult, "dd.MM.yyyy");
      }

      if (fieldInfo == "familycomposition") {
        if (result == undefined) {
          itemResult = " "
        }
        itemResult = this.compositions.filter(s => s.id == result)[0].name;
      }
      if (fieldInfo == "clarifyFamilycomposition") {
        if (result == undefined) {
          itemResult = " "
        }
        itemResult = this.clarifycompositions.filter(s => s.id == result)[0].name;
      }
      if (fieldInfo == "familyNumberChildren") {
        if (result == undefined) {
          itemResult = " "
        }
        itemResult = this.numbersChildren.filter(s => s.id == result)[0].name;
      }
      if (fieldInfo == "healthGroup") {
        if (result == undefined) {
          itemResult = " "
        }
        itemResult = this.healthGroups.filter(s => s.id == result)[0].name;
      }
      if (fieldInfo == "physicalGroup") {
        if (result == undefined) {
          itemResult = " "
        }
        itemResult = this.physicalGroups.filter(s => s.id == result)[0].name;
      }
      if (fieldInfo == "registration") {
        if (result == undefined) {
          itemResult = " "
        }
        itemResult = this.registrations.filter(s => s.id == result)[0].name;
      }
      if (fieldInfo == "qualityLife") {
        if (result == undefined) {
          itemResult = " "
        }
        itemResult = this.qualityLifes.filter(s => s.id == result)[0].name;
      }

      result += ' ' + itemResult;
    }
    return result;
    
  //   result = fieldName.split('.')
  //   .reduce(function(o, k) {
  //     return o && o[k];
  //   }, object)
    

  //   if(secondFieldName){
  //       let secondResult;
  //       secondResult = secondFieldName.split('.')
  //       .reduce(function(o, k) {
  //         return o && o[k];
  //       }, object);
  //       result += ' ' + secondResult;
  //   }

  //   if(thirdVariable){
  //     let thirdResult;
  //     thirdResult = thirdVariable.split('.')
  //     .reduce(function(o, k) {
  //       return o && o[k];
  //     }, object);
  //     result += ' ' + thirdResult;
  // }
  //   return result;
  }

  getVisibleColumns(){
    return this.columns.filter(i => i.visibility);
  }
}
