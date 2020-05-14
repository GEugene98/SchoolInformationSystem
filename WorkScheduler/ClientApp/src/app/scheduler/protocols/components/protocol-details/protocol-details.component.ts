import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ScheduleService } from '../../../services/schedule.service';
import { Protocol } from '../../../../shared/models/protocol.model';
import { MessageService } from 'primeng/api';
import { Agenda, InnerContent } from '../../../../shared/models/agenda.model';
import { DictionaryService } from '../../../../shared/services/dictionary.service';

@Component({
  selector: 'app-protocol-details',
  templateUrl: './protocol-details.component.html',
  styleUrls: ['./protocol-details.component.css']
})
export class ProtocolDetailsComponent implements OnInit {

  actionId: number;
  protocol: Protocol;

  allResponsibles;

  protocolContent: Agenda[] = [new Agenda()];

  constructor(private activateRoute: ActivatedRoute, private scheduleService: ScheduleService, private messageService: MessageService, private dictionary: DictionaryService) { }

  async ngOnInit() {
    this.actionId = this.activateRoute.snapshot.params['id'];
    await this.loadData();
  }

  async loadData() {
    this.protocol = await this.scheduleService.getOrCreateProtocol(this.actionId);
    this.allResponsibles = this.dictionary.getResponsibles();
  }

  async saveProtocol() {
    try {
      this.protocol.protocolContentJSON = JSON.stringify(this.protocolContent);
      await this.scheduleService.saveProtocol(this.protocol);
      this.messageService.add({ severity: 'success', summary: 'Готово', detail: "Внесенные изменения сохранены", life: 5000 });
    }
    catch(e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  async getDocument() {
    try {
      await this.scheduleService.saveProtocol(this.protocol);
      window.open(`/api/Report/Protocol?` + `protocolId=${this.protocol.id}`);
    }
    catch (e) {
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
    
  }

  addAgenda() {
    this.protocolContent.push(new Agenda());
  }

  delAgenda(agenda) {
    this.protocolContent.splice(this.protocolContent.indexOf(agenda), 1);
  }

  addListen(agenda: Agenda) {
    agenda.listen.push(new InnerContent());
  }

  addSpeaked(agenda: Agenda) {
    agenda.speaked.push(new InnerContent());
  }

}
