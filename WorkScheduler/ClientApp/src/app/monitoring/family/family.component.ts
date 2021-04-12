import { Component, OnInit } from '@angular/core';
import { AcademicYear } from '../../shared/models/academic-year.model';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { Class } from '../models/class.model';
import { ClassService } from '../services/class.service';
import { FamilyService } from '../services/family.service';

@Component({
  selector: 'app-family',
  templateUrl: './family.component.html',
  styleUrls: ['./family.component.css']
})
export class FamilyComponent implements OnInit {

  displayModal: boolean = false;
  selectedAcademicYear: AcademicYear;
  allAcademicYears: AcademicYear[];
  allClasses: Class[];
  selectedClassses: Class;

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
      variable: ""
    },
    {
      name: "Дата рождения",
      visibility: false,
      variable: ""
    },
    {
      name: "Номер свидетельства о рождении (паспорта)",
      visibility: false,
      variable: ""
    },
    {
      name: "Кем выдан",
      visibility: false,
      variable: ""
    },
    {
      name: "Когда выдан",
      visibility: false,
      variable: ""
    },
    {
      name: "Адрес регистрации",
      visibility: false,
      variable: ""
    },
    {
      name: "Адрес проживания",
      visibility: false,
      variable: ""
    },
    {
      name: "ФИО матери (полностью), телефон, место работы",
      visibility: false,
      variable: ""
    },
    {
      name: "ФИО отца (полностью), телефон, место работы",
      visibility: false,
      variable: ""
    },
    {
      name: "Категория семьи по составу",
      visibility: false,
      variable: ""
    },
    {
      name: "Категория семьи по количеству детей",
      visibility: false,
      variable: ""
    },
    {
      name: "Категория семьи по качеству жизни",
      visibility: false,
      variable: ""
    },
    {
      name: "Группа здоровья",
      visibility: true,
      variable: ""
    },
    {
      name: "Физкультурная группа",
      visibility: false,
      variable: ""
    },
    {
      name: "Учет",
      visibility: true,
      variable: ""
    },
  ]

  constructor(private dictionary: DictionaryService,
    private classService: ClassService, private familyService: FamilyService) { }

  async ngOnInit() {
    await this.loadData();
  }
  
  showModalDialog() {
      this.displayModal = true;
  }

  async loadData(){
    this.allAcademicYears = await this.dictionary.getAcademicYears();
    this.selectedAcademicYear = this.allAcademicYears[0];
    this.familyService.getFamilies();
    // this.allClasses = await this.classService.
  }


  getVisibleColumns(){
    return this.columns.filter(i => i.visibility);
  }
}
