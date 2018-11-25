import { Component, OnInit } from '@angular/core';
import { Action } from '../../shared/models/action.model';
import { ScheduleService } from '../services/schedule.service';
import { MessageService } from 'primeng/api';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ActionStatus } from '../../shared/enums/action-status.enum';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent implements OnInit {

  selectedAll: boolean;
  actions: Action[];

  constructor(private schedule: ScheduleService,
    private messageService: MessageService,
    private ngxService: NgxUiLoaderService,
    private titleService: Title) {
    this.titleService.setTitle('Планы для согласования');
  }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.ngxService.start();
    try {
      this.actions = await this.schedule.getActionsToMake(ActionStatus.Confirmed);
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

  async confirm() {
    let actionIdsToConfirm = this.actions.filter(a => a.selected).map(a => a.id);
    if (actionIdsToConfirm.length == 0) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }

    this.ngxService.start();

    try {
      await this.schedule.confirm(actionIdsToConfirm);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Выбраные мероприятия согласованы", life: 5000 });
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
    let actionIdsToCancel = this.actions.filter(a => a.selected).map(a => a.id);
    if (actionIdsToCancel.length == 0) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: 'Необходимо выбрать хотя бы одно мероприятие', life: 5000 });
      return;
    }

    this.ngxService.start();

    try {
      await this.schedule.cancelConfirming(actionIdsToCancel);
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

