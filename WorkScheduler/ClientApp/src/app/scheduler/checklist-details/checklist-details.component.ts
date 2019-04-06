import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Checklist } from '../../shared/models/checklist.model';
import { ScheduleService } from '../services/schedule.service';

@Component({
  selector: 'app-checklist-details',
  templateUrl: './checklist-details.component.html',
  styleUrls: ['./checklist-details.component.css']
})
export class ChecklistDetailsComponent implements OnInit {

  checklistId: number;
  checklist: Checklist;

  constructor(private activateRoute: ActivatedRoute,
    private schedule: ScheduleService) { 
    this.checklistId = this.activateRoute.snapshot.params['id'];
  }

  ngOnInit() {
    this.loadData();
  }

  async loadData(){
    this.checklist = await this.schedule.getChecklist(this.checklistId);
  }

}
