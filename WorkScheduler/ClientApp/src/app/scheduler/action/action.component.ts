import { Component, OnInit, Input } from '@angular/core';
import { Action } from '../../shared/models/action.model';

@Component({
  selector: 'app-action',
  templateUrl: './action.component.html',
  styleUrls: ['./action.component.css']
})
export class ActionComponent implements OnInit {

  @Input() action: Action;

  constructor() { }

  ngOnInit() {
  }

}
