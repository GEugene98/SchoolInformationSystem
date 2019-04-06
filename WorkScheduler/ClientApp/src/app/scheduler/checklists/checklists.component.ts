import { Component, OnInit } from '@angular/core';
import { Checklist } from '../../shared/models/checklist.model';
import { BsModalService } from 'ngx-bootstrap';
import { DictionaryService } from '../../shared/services/dictionary.service';
import { MessageService } from 'primeng/api';
import { Title } from '@angular/platform-browser';
import { ScheduleService } from '../services/schedule.service';

@Component({
  selector: 'app-checklists',
  templateUrl: './checklists.component.html',
  styleUrls: ['./checklists.component.css']
})
export class ChecklistsComponent implements OnInit {

  checklists: Checklist[];

  constructor(private modalService: BsModalService,
    private dictionary: DictionaryService,
    private messageService: MessageService,
    private titleService: Title,
    private schedule: ScheduleService) { 
      this.titleService.setTitle("Мои чек-листы");
    }

  ngOnInit() {
    this.loadData();
  }

  async loadData() {
    this.checklists = await this.schedule.getMyChecklists();
    this.checklists.forEach(c => {
      c.chartData =
        {
          labels: ['Назначенные', 'Принятые', 'Готовые'],
          datasets: [
            {
              data: [c.assignedCount, c.acceptedCount, c.doneCount],
              backgroundColor: [
                "#36A2EB",
                "#FFCE56",
                "#DCF753"
              ],
              hoverBackgroundColor: [
                "#36A2EB",
                "#FFCE56",
                "#DCF753"
              ]
            }
          ],
          options: {
            legend: {
              display: false,
              position: 'left'
            }
          }
        }
    });
  }


}
