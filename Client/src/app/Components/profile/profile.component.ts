import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Follow } from 'src/app/Models/Follow';
import { PostToList } from 'src/app/Models/PostToList';
import { User } from 'src/app/Models/User';
import { FollowService } from 'src/app/Services/follow.service';
import { PostService } from 'src/app/Services/post.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  user: User;
  posts: PostToList[];
  following: User[];
  followers: User[];
  isFollowing: boolean;

  constructor(private route: ActivatedRoute, private userService: UserService, private followService: FollowService,
    private postService: PostService) { }

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
      this.postService.getPostsFromUser(this.user.id, this.userService.getUserId()).subscribe(posts => {
        posts.sort((a, b) => new Date(b.timePost).getTime() - new Date(a.timePost).getTime());
        this.posts = posts;

        this.followService.isFollowing(this.userService.getUserId(), this.user.id).subscribe(response => {
          if (response.isFollowing)
            this.isFollowing = true;
          else
            this.isFollowing = false;
        })
      })
    })
  }

  isCurrentUser() {
    return this.user.id === this.userService.getUserId();
  }

  onGetFollowing() {
    this.followService.getFollowing(this.userService.getUserId(), this.user.id).subscribe(following => this.following = following);
  }

  onGetFollower() {
    this.followService.getFollower(this.userService.getUserId(), this.user.id).subscribe(followers => this.followers = followers);
  }

  onFollow() {
    const follow: Follow = {
      followerId: this.userService.getUserId(),
      followingId: this.user.id
    };
    this.followService.followUser(follow).subscribe(next => {
      this.isFollowing = !this.isFollowing;
    });
  }

  onUnfollow() {
    const follow: Follow = {
      followerId: this.userService.getUserId(),
      followingId: this.user.id
    };
    this.followService.unFollowUser(follow).subscribe(next => {
      this.isFollowing = !this.isFollowing;
    });
  }

  removeFromFollowing(user: User) {
    this.following.forEach(e => {
      if (e.id === user.id)
        e.isFollowing = user.isFollowing;
    });
  }

  removeFromFollower(user: User) {
    this.followers.forEach(e => {
      if (e.id === user.id)
        e.isFollowing = user.isFollowing;
    });
  }

}
