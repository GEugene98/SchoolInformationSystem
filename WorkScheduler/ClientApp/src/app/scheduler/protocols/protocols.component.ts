import { Component, OnInit } from '@angular/core';
import { ScheduleService } from '../services/schedule.service';
import { ProtocolInfo } from '../../shared/models/protocol-info.model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-protocols',
  templateUrl: './protocols.component.html',
  styleUrls: ['./protocols.component.css']
})
export class ProtocolsComponent implements OnInit {

  selectedYear: number = 2020;
  protocols: ProtocolInfo[];

  constructor(private scheduleServise: ScheduleService, private messageService: MessageService) { }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    try {
      this.protocols = await this.scheduleServise.getMyProtocols(this.selectedYear);
    }
    catch(e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

}
