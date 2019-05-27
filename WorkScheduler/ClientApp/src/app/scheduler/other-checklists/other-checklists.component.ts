import { Component, OnInit, Input } from '@angular/core';
import { Checklist } from '../../shared/models/checklist.model';

@Component({
  selector: 'app-other-checklists',
  templateUrl: './other-checklists.component.html',
  styleUrls: ['./other-checklists.component.css']
})
export class OtherChecklistsComponent implements OnInit {

  @Input() checklists: Checklist[];
  @Input() title: string;
  showAll: boolean;

  constructor() { }

  ngOnInit() {
    
  }

  showAllHandler(){
    this.showAll = !this.showAll; 
    // window.scrollTo(0, 1000);
  }

}
