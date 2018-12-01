import { Component, OnInit } from '@angular/core';
import { ScheduleService } from '../services/schedule.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { ActionStatus } from '../../shared/enums/action-status.enum';
import { Action } from '../../shared/models/action.model';
import { Title } from '@angular/platform-browser';
import { WorkSchedule } from '../../shared/models/work-schedule.model';

@Component({
  selector: 'app-accept',
  templateUrl: './accept.component.html',
  styleUrls: ['./accept.component.css']
})
export class AcceptComponent implements OnInit {

  schedules: WorkSchedule[];

  constructor(private schedule: ScheduleService,
    private messageService: MessageService,
    private ngxService: NgxUiLoaderService,
    private titleService: Title) {
    this.titleService.setTitle('Планы для утверждения');
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.ngxService.start();
    try {
      this.schedules = await this.schedule.getActionsToMake(ActionStatus.Accepted);
    } catch (e) {
    }
    finally {
      this.ngxService.stop();
    }
  }


  async accept(actionIdsToAccept: number[]) {
    if (actionIdsToAccept.length == 0) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }
    this.ngxService.start();
    try {
      await this.schedule.accept(actionIdsToAccept);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия утверждены", life: 5000 });
      await this.loadData();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
      this.ngxService.stop();
    }
  }

  async cancel(actionIdsToCancel: number[]) {
    if (actionIdsToCancel.length == 0) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }
    this.ngxService.start();
    try {
      await this.schedule.cancelAcepting(actionIdsToCancel);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия отклонены", life: 5000 });
      await this.loadData();
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
      this.ngxService.stop();
    }
  }
}
