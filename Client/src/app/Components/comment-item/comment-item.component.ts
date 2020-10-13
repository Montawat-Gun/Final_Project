import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import * as moment from 'moment';
import { PostComment } from 'src/app/Models/PostComment';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.css']
})
export class CommentItemComponent implements OnInit {

  @Input() comment: PostComment;
  @Input() isOwnPost: boolean;
  @Output() deleteComment = new EventEmitter();

  @ViewChild('closeModal') closeModal;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.comment.timeComment = moment.parseZone(this.comment.timeComment).local().format();
  }

  isEnableDelete() {
    return (this.userService.getUserId() === this.comment.user.id || this.isOwnPost);
  }

  onDeleteComment() {
    this.deleteComment.emit(this.comment);
  }
}
