import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import * as moment from 'moment';
import { utc } from 'moment';
import { NotificationService } from 'src/app/Services/notification.service';
import { PostService } from 'src/app/Services/post.service';
import { UserService } from 'src/app/Services/user.service';
import { PostToList } from '../../Models/PostToList';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrls: ['./post-item.component.css']
})
export class PostItemComponent implements OnInit {

  @Input() post: PostToList
  @Output() deletePost = new EventEmitter<PostToList>();

  @ViewChild('closeModal') closeModal;

  constructor(private postService: PostService, private userService: UserService, private notification: NotificationService) { }

  ngOnInit(): void {
    this.post.timePost = moment.parseZone(this.post.timePost).local().format();
  }

  onLike() {
    this.postService.like(this.post.postId, this.userService.getUserId()).subscribe(resqonse => {
      this.post.isLike = !this.post.isLike;
      if (this.post.isLike) {
        this.post.likeCount++;
        if (this.userService.getUserId() !== this.post.user.id)
          this.notification.sendNotification(this.post.user.id, this.userService.user.username + ' liked your post',
            'post/' + this.post.postId)
      } else {
        this.post.likeCount--;
      }
    });
  }

  isEnableDelete() {
    return this.userService.getUserId() === this.post.user.id;
  }

  onDeletePost() {
    this.deletePost.emit(this.post);
  }

}
