import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Follow } from '../Models/Follow';
import { User } from '../Models/User';

@Injectable({
  providedIn: 'root'
})
export class FollowService {

  url: string = environment.url + 'follow/'

  constructor(private http: HttpClient) { }

  isFollowing(fromUserId: string, userId: string): Observable<any> {
    return this.http.get<any>(this.url + 'isfollowing/' + fromUserId + '/' + userId);
  }

  getFollowing(fromUserId: string, userId: string): Observable<User[]> {
    return this.http.get<User[]>(this.url + 'following/' + fromUserId + '/' + userId);
  }

  getFollower(fromUserId: string, userId: string): Observable<User[]> {
    return this.http.get<User[]>(this.url + 'follower/' + fromUserId + '/' + userId);
  }

  followUser(follow: Follow) {
    return this.http.post(this.url, follow);
  }

  unFollowUser(follow: Follow,) {
    return this.http.delete(this.url + 'unfollow/' + follow.followerId + '/' + follow.followingId);
  }
}
