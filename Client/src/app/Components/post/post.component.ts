import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { PostToList } from 'src/app/Models/PostToList';
import { User } from 'src/app/Models/User';
import { PostService } from 'src/app/Services/post.service';
import { ToastifyService } from 'src/app/Services/toastify.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {

  @Input() user: User;
  @ViewChild('closeModal') closeModal;


  posts: PostToList[];
  postToDelete: PostToList;

  constructor(private postService: PostService, private userService: UserService,private toastify: ToastifyService) { }

  ngOnInit(): void {
    this.postService.getPosts(this.userService.getUserId()).subscribe(response => this.posts = response);
  }

  setPostToDelete(post: PostToList) {
    this.postToDelete = post;
  }

  removePostToDelete() {
    this.postToDelete = null;
    this.closeModal.nativeElement.click()
  }

  onDeletePost() {
    this.postService.deletePost(this.postToDelete.postId).subscribe(next => {
      this.posts = this.posts.filter(obj => obj !== this.postToDelete);
      this.toastify.show("Deleted post.")
      this.removePostToDelete();
    })
  }

}
