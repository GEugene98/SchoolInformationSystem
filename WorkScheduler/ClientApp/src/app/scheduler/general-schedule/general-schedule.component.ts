import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-general-schedule',
  templateUrl: './general-schedule.component.html',
  styleUrls: ['./general-schedule.component.css']
})
export class GeneralScheduleComponent implements OnInit {

  constructor(private titleService: Title) {
    this.titleService.setTitle('Сводный план');
  }

  academicYears: any[];
  selectedAcademicYearId: any;
  selectedOptopn: number = 1;

  ngOnInit() {
    this.academicYears = [{ id: 0, name: '2018-2019' }, { id: 0, name: '2019-2020' }];
  }

}
