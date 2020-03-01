import { Component, OnInit } from '@angular/core';
import Debounce from 'debounce-decorator';

@Component({
  selector: 'app-callboard',
  templateUrl: './callboard.component.html',
  styleUrls: ['./callboard.component.css']
})
export class CallboardComponent implements OnInit {

  futureCommentColor = 'c-blue';
  futureComment: string;

  constructor() { }

  ngOnInit() {
    this.futureComment = window.localStorage.getItem('commentToBeSent');
  }

  changeColor(target: string) {
    this.futureCommentColor = target;
  }

  @Debounce(500)
  save() {
    window.localStorage.setItem('commentToBeSent', this.futureComment);
    console.log(this.futureComment);
  }
}
