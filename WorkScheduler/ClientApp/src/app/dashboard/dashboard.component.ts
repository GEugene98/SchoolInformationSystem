import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  modalRef: BsModalRef;

  constructor(private titleService: Title, private modalService: BsModalService) {
    //this.titleService.setTitle('');
  }

  ngOnInit() {
  }

  openModal(modal) {
    this.modalRef = this.modalService.show(modal);
  }

}
