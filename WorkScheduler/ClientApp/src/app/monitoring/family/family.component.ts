import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-family',
  templateUrl: './family.component.html',
  styleUrls: ['./family.component.css']
})
export class FamilyComponent implements OnInit {

  displayModal: boolean = false;

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

  constructor() { }

  ngOnInit() {
  }
  
  showModalDialog() {
      this.displayModal = true;
  }

  getVisibleColumns(){
    return this.columns.filter(i => i.visibility);
  }
}
