import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { User } from '../Models/User'
import { UserService } from '../Services/user.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class UsersSuggestResolver implements Resolve<User> {
    constructor(private userService: UserService, private router: Router) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<User> {
        return this.userService.getSuggestUsers().pipe(
            catchError(error => {
                return of(null);
            }
            )
        );
    }
}