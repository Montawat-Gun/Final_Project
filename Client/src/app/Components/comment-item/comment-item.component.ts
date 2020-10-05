import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
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
  }

  isEnableDelete() {
    return (this.userService.getUserId() === this.comment.user.id || this.isOwnPost);
  }

  onDeleteComment() {
    this.deleteComment.emit(this.comment);
  }
}
