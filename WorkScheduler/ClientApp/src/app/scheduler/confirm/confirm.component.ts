import { Component, OnInit } from '@angular/core';
import { Action } from '../../shared/models/action.model';
import { ScheduleService } from '../services/schedule.service';
import { MessageService } from 'primeng/api';
   
import { ActionStatus } from '../../shared/enums/action-status.enum';
import { Title } from '@angular/platform-browser';
import { WorkSchedule } from '../../shared/models/work-schedule.model';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent implements OnInit {

  schedules: WorkSchedule[];

  constructor(private schedule: ScheduleService,
    private messageService: MessageService,
     
    private titleService: Title) {
    this.titleService.setTitle('Планы для согласования');
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
     
    try {
      this.schedules = await this.schedule.getActionsToMake(ActionStatus.Confirmed);
    } catch (e) {
    }
    finally {
        
    }
  }

  async confirm(actionIdsToConfirm: number[]) {
    if (actionIdsToConfirm.length == 0) {
      this.messageService.add({ severity: 'info', summary: 'Предупреждение', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }
     
    try {
      await this.schedule.confirm(actionIdsToConfirm);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия согласованы", life: 5000 });
      await this.loadData();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
        
    }
  }

  async cancel(actionIdsToCancel) {
    if (actionIdsToCancel.length == 0) {
      this.messageService.add({ severity: 'info', summary: 'Предупреждение', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }
     
    try {
      await this.schedule.cancelConfirming(actionIdsToCancel);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия отклонены", life: 5000 });
      await this.loadData();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
        
    }
  }
}

