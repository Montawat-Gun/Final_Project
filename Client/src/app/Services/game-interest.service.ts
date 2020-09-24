import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Game } from '../Models/Game';
import { Interest } from '../Models/Interest';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class GameInterestService {

  url: string = environment.url;

  constructor(private http: HttpClient, private userService: UserService) { }

  getGames(): Observable<Game[]> {
    return this.http.get<Game[]>(this.url + 'game/');
  }

  getInterests() {
    const userId = this.userService.getUserId();
    return this.http.get(this.url + 'interest/' + userId);
  }

  addInterests(interests: Interest[]) {
    return this.http.post(this.url + 'interest/', interests);
  }
}
