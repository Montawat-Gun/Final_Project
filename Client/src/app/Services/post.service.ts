import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  url: string = environment.url;

  constructor(private http: HttpClient) { }

  getPosts() {
    return this.http.get(this.url + 'post');
  }

  createPost(model: any) {
    return this.http.post(this.url + 'post', model);
  }
}
