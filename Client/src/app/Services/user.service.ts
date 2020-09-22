import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { User } from '../Models/User'
import { Follow } from '../Models/Follow'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url: string = environment.url + "user/";
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) { }

  getUserId() {
    const token = localStorage.getItem("token");
    return this.jwtHelper.decodeToken(token).nameid;
  }

  getCurrentUser(): Observable<User> {
    const token = localStorage.getItem("token");
    const username = this.jwtHelper.decodeToken(token).unique_name;
    return this.http.get<User>(this.url + username);
  }

  getUser(username: string): Observable<User> {
    return this.http.get<User>(this.url + username);
  }

  getSuggestUsers(): Observable<User[]> {
    const token = localStorage.getItem("token");
    const userId = this.jwtHelper.decodeToken(token).nameid;
    return this.http.get<User[]>(this.url + 'suggest/' + userId);
  }
}
