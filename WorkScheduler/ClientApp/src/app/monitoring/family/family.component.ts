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

@Component({
  selector: 'app-family',
  templateUrl: './family.component.html',
  styleUrls: ['./family.component.css']
})
export class FamilyComponent implements OnInit {

  bsConfig: any;
  displayModalColumns: boolean = false;
  displayModalEdit: boolean = false;
  selectedAcademicYear: AcademicYear;
  allAcademicYears: AcademicYear[];
  allClasses: Class[];
  // newFamily: Family;
  families: Family[];
  selectedClass: Class;
  modalColumns: any[];

  classes = [
    { id: 1, name: "1-А"},
    { id: 2, name: "2-А"},
    { id: 3, name: "3-А"},
    { id: 4, name: "4-А"},
    { id: 5, name: "5-А"},
    { id: 6, name: "6-А"},
    { id: 7, name: "7-А"},
    { id: 8, name: "8-А"},
    { id: 9, name: "9-А"},
    { id: 10, name: "10-А"},
    { id: 11, name: "11-А"}

  ]

  columns = [
    {
      name: "ФИО ребенка",
      visibility: false,
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
      name: "Кем выдан",
      visibility: false,
      fieldInfo: "issuedWhom"
    },
    {
      name: "Когда выдано",
      visibility: false,
      fieldInfo: "whenIssued"
    },
    {
      name: "Адрес регистрации",
      visibility: false,
      fieldInfo: "registrAddres"
    },
    {
      name: "Адрес проживания",
      visibility: false,
      fieldInfo: "residAddres"
    },
    {
      name: "ФИО матери (полностью), телефон, место работы",
      visibility: false,
      fieldInfo: "fullNameMather"
    },
    {
      name: "ФИО отца (полностью), телефон, место работы",
      visibility: false,
      fieldInfo: "fullNameFather"
    },
    {
      name: "Категория семьи по составу",
      visibility: false,
      fieldInfo: "clarifyСomposition"
    },
    {
      name: "Категория семьи по количеству детей",
      visibility: false,
      fieldInfo: "numberChildren"
    },
    {
      name: "Категория семьи по качеству жизни",
      visibility: false,
      fieldInfo: "qualityLife"
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
    private studentService: StudentService) {
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


  showModalDialogEdit(){
    this.displayModalEdit = true;
  }

  async loadData(){
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    this.selectedAcademicYear = this.allAcademicYears[0];
    await this.loadClasses();
    this.families = await this.familyService.getFamilies();
  }

  async loadClasses() {
    this.allClasses = await this.studentService.getStudentsByClasses(this.selectedAcademicYear.id);
  }

  // copy(family: Family){
  //   this.newFamily = _.cloneDeep(family);
  // }

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
        result += ' ' + itemResult;
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
