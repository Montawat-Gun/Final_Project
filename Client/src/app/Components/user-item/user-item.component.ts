import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Follow } from 'src/app/Models/Follow';
import { User } from 'src/app/Models/User';
import { FollowService } from 'src/app/Services/follow.service';
import { NotificationService } from 'src/app/Services/notification.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css']
})
export class UserItemComponent implements OnInit {

  @Input() user: User
  @Output() removeFromList: EventEmitter<User> = new EventEmitter();

  constructor(public userService: UserService, private followService: FollowService, private notification: NotificationService) { }

  ngOnInit(): void {
  }

  isCurrentUser() {
    return this.userService.getUserId() === this.user.id;
  }

  onFollow() {
    const follow: Follow = {
      followerId: this.userService.getUserId(),
      followingId: this.user.id
    };
    this.followService.followUser(follow).subscribe(next => {
      this.user.isFollowing = true;
      this.removeFromList.emit(this.user);
      this.notification.sendNotification(follow.followingId, this.userService.user.username + ' has following you.',
        'profile/' + this.userService.user.username);
    });
  }

  onUnfollow() {
    const follow: Follow = {
      followerId: this.userService.getUserId(),
      followingId: this.user.id
    };
    this.followService.unFollowUser(follow).subscribe(next => {
      this.user.isFollowing = false;
      this.removeFromList.emit(this.user);
    });
  }

}
