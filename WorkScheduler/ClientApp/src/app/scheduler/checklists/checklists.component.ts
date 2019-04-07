import { Component, OnInit } from '@angular/core';
import { Checklist } from '../../shared/models/checklist.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
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

  bsConfig: any;
  modalRef: BsModalRef;
  checklists: Checklist[];
  newChecklist: Checklist;

  constructor(private modalService: BsModalService,
    private dictionary: DictionaryService,
    private messageService: MessageService,
    private titleService: Title,
    private schedule: ScheduleService) { 
      this.titleService.setTitle("Мои чек-листы");
      this.bsConfig = { dateInputFormat: 'DD.MM.YYYY', locale: 'ru' };
    }

  ngOnInit() {
    this.loadData();
    this.newChecklist = new Checklist();
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

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

  closeModal() {
    this.modalRef.hide();
  }
  
  copy(checklist: Checklist) {
    this.newChecklist = Object.assign({}, checklist);
    this.newChecklist.deadline = new Date(this.newChecklist.deadline.toString()); //Костыль для ngx-datepicker'а
  }

  async addChecklist(){
    try {
      await this.schedule.addChecklist(this.newChecklist);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Чек-лист создан", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
      this.newChecklist = undefined;
      this.newChecklist = new Checklist();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async saveChecklist(){
    try {
      await this.schedule.editChecklist(this.newChecklist);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Чек-лист изменен", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
      this.newChecklist = undefined;
      this.newChecklist = new Checklist();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async deleteChecklist(){
    try {
      await this.schedule.deleteChecklist(this.newChecklist);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Чек-лист удален", life: 5000 });
      await this.loadData();
      this.modalRef.hide();
      this.newChecklist = undefined;
      this.newChecklist = new Checklist();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  clear(){
    this.newChecklist = new Checklist();
  }
}
