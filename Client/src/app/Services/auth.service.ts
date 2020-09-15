import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url: string = environment.url + 'auth/';
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient, private cookieService: CookieService) { }

  login(model: any) {
    return this.http.post(this.url + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          this.cookieService.set("token", user.token);
        }
      })
    )
  }

  isAuthenticated() {
    const token = this.cookieService.get("token");
    return !this.jwtHelper.isTokenExpired(token);
  }

  register(model: any) {
    return this.http.post(this.url + 'register', model);
  }

  logout(){
    this.cookieService.delete("token")
  }
}
