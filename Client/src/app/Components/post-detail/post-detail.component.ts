import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostDetail } from 'src/app/Models/PostDetail';
import { UserService } from 'src/app/Services/user.service';
import { CommentService } from 'src/app/Services/comment.service';
import { PostComment } from 'src/app/Models/PostComment';
import { User } from 'src/app/Models/User';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent implements OnInit {

  user: User;
  post: PostDetail
  comment: string = '';
  commentToDelete: PostComment = null;
  isOwnPost: boolean;

  @ViewChild('closeModal') closeModal;

  constructor(private route: ActivatedRoute, private commentService: CommentService, private userService: UserService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.post = data['post'];
      this.userService.getCurrentUser().subscribe(user => this.user = user);
      this.isOwnPost = (this.userService.getUserId() === this.post.user.id);
    })
  }

  isDisableCommentButton() {
    if (this.comment === '')
      return true;
    else
      return false
  }

  onComment() {
    if (this.comment === '')
      return;
    const model = {
      userId: this.userService.getUserId(),
      postId: this.post.postId,
      content: this.comment
    };

    this.commentService.createComment(model).subscribe(response => {
      this.comment = ''
      this.post.comments.push(response);
      this.post.commentCount++;
    }, error => console.log(error));
  }

  setCommentToDelete(comment: PostComment) {
    this.commentToDelete = comment;
  }

  removeCommentToDelete() {
    this.commentToDelete = null;
    this.closeModal.nativeElement.click();
  }

  onDeleteComment() {
    this.commentService.deleteComment(this.commentToDelete.commentId).subscribe(() => {
      this.post.comments = this.post.comments.filter(obj => obj !== this.commentToDelete);
      this.removeCommentToDelete();
      this.post.commentCount--;
    });
  }

}
