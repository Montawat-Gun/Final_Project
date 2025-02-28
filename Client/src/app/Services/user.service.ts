import { Injectable, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { User } from '../Models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url: string = environment.url + "user/";
  jwtHelper = new JwtHelperService();

  user: User;

  constructor(private http: HttpClient) { }

  getToken() {
    const token = localStorage.getItem("token");
    return token;
  }

  getUserId() {
    const token = localStorage.getItem("token");
    if (token) {
      return this.jwtHelper.decodeToken(token).nameid;
    } else return null;
  }

  getCurrentUser() {
    const token = localStorage.getItem("token");
    if (token) {
      const id = this.jwtHelper.decodeToken(token).nameid;
      this.http.get<User>(this.url + 'id/' + id).subscribe(user => {
        this.user = user;
      });
    }
  }

  getAllUsers() {
    return this.http.get<User[]>(this.url + 'all/' + this.getUserId());
  }

  getUserRoles() {
    const token = localStorage.getItem("token");
    if (token) {
      const roles = this.jwtHelper.decodeToken(token).role;
      return roles;
    }
  }

  searchUser(searchString: string): Observable<User[]> {
    return this.http.get<User[]>(this.url + 'search/' + this.getUserId() + '/' + searchString);
  }

  getUser(username: string): Observable<User> {
    return this.http.get<User>(this.url + 'username/' + username);
  }

  updateUser(userToEdit: (any)): Observable<User> {
    return this.http.put<User>(this.url + userToEdit.id, userToEdit);
  }

  updateUserPassword(passwordToEdit: (any)): Observable<string> {
    return this.http.put<string>(this.url + 'password/' + this.getUserId(), passwordToEdit);
  }

  getSuggestUsers(): Observable<User[]> {
    const token = localStorage.getItem("token");
    const userId = this.jwtHelper.decodeToken(token).nameid;
    return this.http.get<User[]>(this.url + 'suggest/' + userId);
  }

  uploadUserImage(file: FormData) {
    return this.http.post(this.url + this.getUserId() + '/image', file, { reportProgress: true, observe: 'events' });
  }
}
