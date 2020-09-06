import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CookieService } from 'ngx-cookie-service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url: string = "http://localhost:8080/user/";
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient, private cookieService: CookieService) { }

  getUser() {
    const token = this.cookieService.get("token");
    const username = this.jwtHelper.decodeToken(token).unique_name;
    const httpOption = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + token
      })
    }
    return this.http.get(this.url + "GetUser/?username=" + username, httpOption);
  }
}
