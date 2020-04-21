import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-protocol-details',
  templateUrl: './protocol-details.component.html',
  styleUrls: ['./protocol-details.component.css']
})
export class ProtocolDetailsComponent implements OnInit {

  protocolId: number;

  constructor(private activateRoute: ActivatedRoute) { }

  ngOnInit() {
    this.protocolId = this.activateRoute.snapshot.params['id'];
  }

}
