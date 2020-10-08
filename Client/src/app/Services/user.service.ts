import { Injectable } from '@angular/core';
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

  constructor(private http: HttpClient) { }

  getToken() {
    const token = localStorage.getItem("token");
    return token;
  }

  getUserId() {
    const token = localStorage.getItem("token");
    return this.jwtHelper.decodeToken(token).nameid;
  }

  getCurrentUser(): Observable<User> {
    const token = localStorage.getItem("token");
    const id = this.jwtHelper.decodeToken(token).nameid;
    return this.http.get<User>(this.url + 'id/' + id);
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
