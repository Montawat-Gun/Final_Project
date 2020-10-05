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

  getGame(gameId: number): Observable<Game> {
    return this.http.get<Game>(this.url + 'game/' + gameId);
  }

  getInterests() {
    const userId = this.userService.getUserId();
    return this.http.get(this.url + 'interest/' + userId);
  }

  getIsInterest(gameId: number) {
    const userId = this.userService.getUserId();
    return this.http.get(this.url + 'interest/' + userId + '/' + gameId);
  }

  addInterests(interests: Interest[]) {
    return this.http.post(this.url + 'interest/', interests);
  }

  removeInterest(gameId: number) {
    const userId = this.userService.getUserId();
    return this.http.delete(this.url + 'interest/' + userId + '/' + gameId);
  }
}
