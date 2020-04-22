import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ScheduleService } from '../../../services/schedule.service';
import { Protocol } from '../../../../shared/models/protocol.model';

@Component({
  selector: 'app-protocol-details',
  templateUrl: './protocol-details.component.html',
  styleUrls: ['./protocol-details.component.css']
})
export class ProtocolDetailsComponent implements OnInit {

  actionId: number;
  protocol: Protocol;

  constructor(private activateRoute: ActivatedRoute, private scheduleService: ScheduleService) { }

  async ngOnInit() {
    this.actionId = this.activateRoute.snapshot.params['id'];
    await this.loadData();
  }

  async loadData() {
    this.protocol = await this.scheduleService.getOrCreateProtocol(this.actionId);
  }

}
