import { Component, OnInit } from '@angular/core';
import Debounce from 'debounce-decorator';
import { CallboardService } from '../../../shared/services/callboard.service';
import { Post } from '../../../shared/models/post.model';
import { UserState } from '../../../shared/states/user.state';
import { MessageService } from 'primeng/api';
import { User, isUserInRole } from '../../../shared/models/user';

@Component({
  selector: 'app-callboard',
  templateUrl: './callboard.component.html',
  styleUrls: ['./callboard.component.css']
})
export class CallboardComponent implements OnInit {

  posts: Post[];

  futureCommentColor = 'c-blue';
  futureComment: string;
  notify: boolean;

  constructor(private callboardService: CallboardService, public userState: UserState, private messageService: MessageService,) { }

  async ngOnInit() {
    this.futureComment = window.localStorage.getItem('commentToBeSent');

    setTimeout(async () => {
      if (this.userState.currentUser.state) {
        await this.loadPosts();
      } 
    }, 2000);
  }

  async loadPosts() {
    try {
      this.posts = await this.callboardService.getPosts();
    }
    catch(e) {
      console.log(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
    }
  }

  changeColor(target: string) {
    this.futureCommentColor = target;
  }

  @Debounce(500)
  save() {
    window.localStorage.setItem('commentToBeSent', this.futureComment);
  }

  async sendPost() {
    let post = new Post();
    post.text = this.futureComment;
    post.color = this.futureCommentColor;
    post.notifyAfterPosting = this.notify;

    try {
      await this.callboardService.sendPost(post);
    }
    catch (e) {
      console.log(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
      return;
    }

    await this.loadPosts();

    window.localStorage.removeItem('commentToBeSent');
    this.notify = false;
    this.futureComment = undefined;
    this.futureCommentColor = 'c-blue';
  }

  async deletePost(id: number) {
    try {
      await this.callboardService.deletePost(id);
    }
    catch (e) {
      console.log(e);
      this.messageService.add({ severity: 'error', summary: 'Ошибка', detail: e.error, life: 5000 });
      return;
    }

    await this.loadPosts();
  }

  checkRole(user: User, role: string) {
    return isUserInRole(user, role);
  }
}
