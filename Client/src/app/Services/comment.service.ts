import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PostComment } from '../Models/PostComment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  url: string = environment.url + 'comment/'

  constructor(private http: HttpClient) { }

  createComment(model: any): Observable<PostComment> {
    return this.http.post<PostComment>(this.url, model);
  }

  deleteComment(commentId: number) {
    return this.http.delete(this.url + commentId);
  }
}
