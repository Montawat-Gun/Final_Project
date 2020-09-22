import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Follow } from 'src/app/Models/Follow';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/Services/user.service';
import { FollowService } from '../../Services/follow.service'

@Component({
  selector: 'app-suggest-item',
  templateUrl: './suggest-item.component.html',
  styleUrls: ['./suggest-item.component.css']
})
export class SuggestItemComponent implements OnInit {

  @Input() user: User
  @Output() followUser: EventEmitter<User> = new EventEmitter();

  constructor(private http: HttpClient, private userService: UserService, private followService: FollowService) { }

  ngOnInit(): void {
  }

  onFollow() {
    let follow: Follow = new Follow;
    follow.followerId = this.userService.getUserId();
    follow.followingId = this.user.id;
    this.followService.followUser(follow).subscribe(next => {
      this.followUser.emit(this.user);
    });
  }

}
