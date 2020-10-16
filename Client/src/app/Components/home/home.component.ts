import { Component, OnInit } from '@angular/core';
import { PostToList } from 'src/app/Models/PostToList';
import { NotificationService } from 'src/app/Services/notification.service';
import { PostService } from 'src/app/Services/post.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  posts: PostToList[]

  constructor(public userService: UserService, private notification: NotificationService, private postService: PostService) { }

  ngOnInit(): void {
    if (!this.userService.user)
      this.userService.getCurrentUser();
    if (!this.notification.isHubConnected())
      this.notification.createHubConnection();

    this.postService.getPosts(this.userService.getUserId()).subscribe(posts => this.posts = posts);
  }

}
