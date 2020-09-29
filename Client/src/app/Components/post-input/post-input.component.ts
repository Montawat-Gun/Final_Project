import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { PostService } from '../../Services/post.service'

@Component({
  selector: 'app-post-input',
  templateUrl: './post-input.component.html',
  styleUrls: ['./post-input.component.css']
})
export class PostInputComponent implements OnInit {

  @Input() user: User;
  content: string = ''
  selectedFile = null;

  constructor(private postService: PostService) { }

  ngOnInit(): void {
  }

  onSelectFile(event) {
    if (event.target.files.length > 0) {
      this.selectedFile = event.target.files[0];
    } else {
      this.selectedFile = null;
    }
    this.showPreview();
  }

  showPreview() {
      var preview = (<HTMLInputElement>document.getElementById("preview"));
      if (this.selectedFile !== null) {
      const src = URL.createObjectURL(this.selectedFile);
      preview.src = src;
    } else {
      preview.src = '';
    }
  }

  removeFile() {
    this.selectedFile = null;
    this.showPreview();
  }

  onCreatePost() {
    const model = {
      content: this.content,
      userId: this.user.id,
      gameId: 2
    }
    this.postService.createPost(model).subscribe(next => {
      this.content = '';
      this.selectedFile = null;
      this.showPreview();
    }, error => console.log(error))
  }

}
