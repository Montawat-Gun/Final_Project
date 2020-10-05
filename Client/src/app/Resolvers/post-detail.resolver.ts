import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { PostToList } from '../Models/PostToList'
import { PostService } from '../Services/post.service';
import { catchError } from 'rxjs/operators';
import { PostDetail } from '../Models/PostDetail';
import { UserService } from '../Services/user.service';

@Injectable({ providedIn: 'root' })
export class PostDetailResolver implements Resolve<PostDetail> {
    constructor(private postService: PostService, private router: Router, private userService: UserService) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<PostDetail> {
        return this.postService.getPostDetail(route.params['postId'], this.userService.getUserId()).pipe(
            catchError(error => {
                this.router.navigate(['']);
                return of(null);
            }
            )
        );
    }
}