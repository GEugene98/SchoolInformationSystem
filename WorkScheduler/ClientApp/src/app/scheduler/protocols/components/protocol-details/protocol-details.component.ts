import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ScheduleService } from '../../../services/schedule.service';
import { Protocol } from '../../../../shared/models/protocol.model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-protocol-details',
  templateUrl: './protocol-details.component.html',
  styleUrls: ['./protocol-details.component.css']
})
export class ProtocolDetailsComponent implements OnInit {

  actionId: number;
  protocol: Protocol;

  constructor(private activateRoute: ActivatedRoute, private scheduleService: ScheduleService, private messageService: MessageService) { }

  async ngOnInit() {
    this.actionId = this.activateRoute.snapshot.params['id'];
    await this.loadData();
  }

  async loadData() {
    this.protocol = await this.scheduleService.getOrCreateProtocol(this.actionId);
  }

  async saveProtocol() {
    try {
      await this.scheduleService.saveProtocol(this.protocol);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Внесенные изменения сохранены", life: 5000 });
    }
    catch(e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }
}
