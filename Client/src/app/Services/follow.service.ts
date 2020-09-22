import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Follow } from '../Models/Follow';

@Injectable({
  providedIn: 'root'
})
export class FollowService {

  url: string = environment.url + 'follow/'

  constructor(private http: HttpClient) { }

  followUser(follow: Follow) {
    return this.http.post(this.url, follow);
  }
}
