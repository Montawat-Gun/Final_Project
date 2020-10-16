import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PostToList } from '../Models/PostToList';
import { PostDetail } from '../Models/PostDetail';
import { ImageResponse } from '../Models/ImageResponse';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  url: string = environment.url + 'post/';

  constructor(private http: HttpClient) { }

  getPosts(userId: string): Observable<PostToList[]> {
    return this.http.get<PostToList[]>(this.url + 'user/' + userId);
  }

  getPostsFromUser(userId: string, fromUserId: string): Observable<PostToList[]> {
    return this.http.get<PostToList[]>(this.url + userId + '/' + fromUserId);
  }

  getPostDetail(postId: number, userId: string): Observable<PostDetail> {
    return this.http.get<PostDetail>(this.url + 'detail/' + postId + '/' + userId);
  }

  getPostsGame(gameId: number, userId: string): Observable<PostToList[]> {
    return this.http.get<PostToList[]>(this.url + 'game/' + gameId + '/' + userId);
  }

  createPost(model: any): Observable<PostToList> {
    return this.http.post<PostToList>(this.url, model);
  }

  uploadPostImage(postId: number, file: FormData): Observable<ImageResponse> {
    return this.http.post<ImageResponse>(this.url + postId + '/image', file);
  }

  like(postId: number, userId: string) {
    return this.http.post(this.url + 'like', { postId, userId });
  }

  deletePost(postId: number) {
    return this.http.delete(this.url + postId);
  }
}
