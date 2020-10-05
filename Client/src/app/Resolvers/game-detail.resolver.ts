import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { GameInterestService } from '../Services/game-interest.service';
import { Game } from '../Models/Game';
import { GameDetail } from '../Models/GameDetail';

@Injectable({ providedIn: 'root' })
export class GameDetailResolver implements Resolve<GameDetail> {
    constructor(private gameService: GameInterestService, private router: Router) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<GameDetail> {
        return this.gameService.getGame(route.params['gameId']).pipe(
            catchError(() => {
                this.router.navigate(['']);
                return of(null);
            }
            )
        );
    }
}