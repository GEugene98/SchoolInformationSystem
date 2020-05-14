import { Component, OnInit } from '@angular/core';
import { ScheduleService } from '../services/schedule.service';
import { ProtocolInfo } from '../../shared/models/protocol-info.model';
import { MessageService } from 'primeng/api';
import { isUserInRole, User } from '../../shared/models/user';
import { UserState } from '../../shared/states/user.state';

@Component({
  selector: 'app-protocols',
  templateUrl: './protocols.component.html',
  styleUrls: ['./protocols.component.css']
})
export class ProtocolsComponent implements OnInit {

  selectedYear: number = 2020;
  protocols: ProtocolInfo[];
  allProtocols: ProtocolInfo[];

  constructor(private scheduleService: ScheduleService, private messageService: MessageService, private userState: UserState) { }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    try {
      this.protocols = await this.scheduleService.getMyProtocols(this.selectedYear);
      this.allProtocols = await this.scheduleService.getAllProtocols(this.selectedYear);
    }
    catch(e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async deleteProtocol(protocolId: number) {
    await this.scheduleService.deleteProtocol(protocolId);
    await this.loadData();
  }

  checkRole(user: User, role: string) {
    return isUserInRole(user, role);
  }

}
