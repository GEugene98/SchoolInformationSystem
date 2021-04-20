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
  displayModal: boolean = false;
  selectedAcademicYear: AcademicYear;
  allAcademicYears: AcademicYear[];
  allClasses: Class[];
  families: Family[];
  selectedClassses: Class;
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
      visibility: true,
      variable: "student.fullName"
    },
    {
      name: "Дата рождения",
      visibility: false,
      variable: "student.birthday"
    },
    {
      name: "Номер свидетельства о рождении (паспорта)",
      visibility: false,
      variable: "passportNumber",
      secondVariable: "birthCertificate"
    },
    {
      name: "Кем выдан",
      visibility: false,
      variable: "birthCertificate"
    },
    {
      name: "Когда выдан",
      visibility: false,
      variable: ""
    },
    {
      name: "Адрес регистрации",
      visibility: false,
      variable: "registrAddres"
    },
    {
      name: "Адрес проживания",
      visibility: true,
      variable: "residAddres"
    },
    {
      name: "ФИО матери (полностью), телефон, место работы",
      visibility: true,
      variable: "fullNameMather",
      secondVariable: "phoneMother",
      thirdVariable: "workMother"
    },
    {
      name: "ФИО отца (полностью), телефон, место работы",
      visibility: false,
      variable: "fullNameFather",
      secondVariable: "phoneFather",
      thirdVariable: "workFather"
    },
    {
      name: "Категория семьи по составу",
      visibility: false,
      variable: ""
    },
    {
      name: "Категория семьи по количеству детей",
      visibility: false,
      variable: "numberChildren"
    },
    {
      name: "Категория семьи по качеству жизни",
      visibility: false,
      variable: ""
    },
    {
      name: "Группа здоровья",
      visibility: false,
      variable: ""
    },
    {
      name: "Физкультурная группа",
      visibility: false,
      variable: ""
    },
    {
      name: "Учет",
      visibility: false,
      variable: ""
    },
  ]

  constructor(private dictionary: DictionaryService,
    private datePipe: DatePipe,
    private classService: ClassService, private familyService: FamilyService) {
      this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
     }

  async ngOnInit() {
    await this.loadData();
  }
  
  hideModal(){
    this.columns = _.cloneDeep(this.modalColumns);
    this.displayModal = false;
  }

  showModalDialog() {
      this.modalColumns = _.cloneDeep(this.columns);
      this.displayModal = true;
  }

  async loadData(){
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    this.selectedAcademicYear = this.allAcademicYears[0];
    this.families = await this.familyService.getFamilies();
    // this.allClasses = await this.classService.
  }

  getPropertyValue(object, fieldName, secondFieldName = undefined, thirdVariable = undefined) {
    let result;
    result = fieldName.split('.')
    .reduce(function(o, k) {
      return o && o[k];
    }, object)
    if(fieldName == "student.birthday"){
      return this.datePipe.transform(result, "dd.MM.yyyy");
    }

    if(secondFieldName){
        let secondResult;
        secondResult = secondFieldName.split('.')
        .reduce(function(o, k) {
          return o && o[k];
        }, object);
        result += ' ' + secondResult;
    }

    if(thirdVariable){
      let thirdResult;
      thirdResult = thirdVariable.split('.')
      .reduce(function(o, k) {
        return o && o[k];
      }, object);
      result += ' ' + thirdResult;
  }
    return result;
  }

  getVisibleColumns(){
    return this.columns.filter(i => i.visibility);
  }
}
