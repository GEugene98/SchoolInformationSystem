import { Component, OnInit } from '@angular/core';
import { ScheduleService } from '../services/schedule.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { MessageService } from 'primeng/api';
import { ActionStatus } from '../../shared/enums/action-status.enum';
import { Action } from '../../shared/models/action.model';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-accept',
  templateUrl: './accept.component.html',
  styleUrls: ['./accept.component.css']
})
export class AcceptComponent implements OnInit {

  selectedAll: boolean;
  actions: Action[];

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
      this.actions = await this.schedule.getActionsToMake(ActionStatus.Accepted);
      this.selectedAll = false;
    } catch (e) {

    }
    finally {
      this.ngxService.stop();
    }

  }

  selection() {
    this.selectedAll = !this.selectedAll;

    if (this.selectedAll) {
      this.actions.forEach(a => {
        a.selected = true;
      });
    }
    else {
      this.actions.forEach(a => a.selected = false);
    }
  }

  select() {
    for (var i = 0; i < this.actions.length; i++) {
      if (this.actions[i].selected) {
        this.selectedAll = true;
      }
      else {
        this.selectedAll = false;
        break;
      }
    }
  }

  async canacel() {
    try {

    } catch (e) {

    }
  }

  async accept() {
    let actionIdsToAccept = this.actions.filter(a => a.selected).map(a => a.id);
    if (actionIdsToAccept.length == 0) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }

    this.ngxService.start();

    try {
      await this.schedule.accept(actionIdsToAccept);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия утверждены", life: 5000 });
      await this.loadData();
      this.selectedAll = false;
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
      this.ngxService.stop();
    }
  }

  async cancel() {
    let actionIdsToAccept = this.actions.filter(a => a.selected).map(a => a.id);
    if (actionIdsToAccept.length == 0) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }

    this.ngxService.start();

    try {
      await this.schedule.cancelAcepting(actionIdsToAccept);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия отклонены", life: 5000 });
      await this.loadData();
      this.selectedAll = false;
    } catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    finally {
      this.ngxService.stop();
    }
  }
}
