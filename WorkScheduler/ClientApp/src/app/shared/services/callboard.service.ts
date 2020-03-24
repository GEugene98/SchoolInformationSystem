import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Post } from "../models/post.model";

@Injectable()
export class CallboardService {

  constructor(private http: HttpClient) {

  }

  async sendPost(post: Post) {
    return await this.http.post('api/Callboard/SendPost', post).toPromise();
  }

  async getPosts() {
    return await this.http.get<Post[]>('api/Callboard/GetAvailablePosts').toPromise();
  }

  async deletePost(id: number) {
    const params = new HttpParams()
      .set('id', id.toString());

    return await this.http.delete('api/Callboard/DeletePost', { params: params }).toPromise();
  }

}
