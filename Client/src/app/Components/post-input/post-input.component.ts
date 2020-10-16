import { HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Game } from 'src/app/Models/Game';
import { PostToList } from 'src/app/Models/PostToList';
import { User } from 'src/app/Models/User';
import { GameInterestService } from 'src/app/Services/game-interest.service';
import { PostService } from 'src/app/Services/post.service';
import { ToastifyService } from 'src/app/Services/toastify.service';

@Component({
  selector: 'app-post-input',
  templateUrl: './post-input.component.html',
  styleUrls: ['./post-input.component.css']
})
export class PostInputComponent implements OnInit {
  @Input() user: User;
  @Output() posted = new EventEmitter<PostToList>();

  content: string = ''
  games: Game[] = [];
  isNoGameFound: Boolean = false;
  selectedGame: Game = null;
  selectedFile = null;
  isLoading: boolean = false;
  isGameLoading: boolean = false;

  constructor(private postService: PostService, private gameService: GameInterestService, private toastify: ToastifyService) { }

  ngOnInit(): void {
  }

  isDisabledButton() {
    return ((this.content === '' && !this.selectedFile) || !this.selectedGame);
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
    this.isLoading = true;
    const model = {
      content: this.content,
      userId: this.user.id,
      gameId: this.selectedGame.gameId
    }
    this.postService.createPost(model).subscribe(response => {
      const post = response
      post.user = this.user;
      post.game = this.selectedGame;
      post.likeCount = 0;
      post.commentCount = 0;
      if (post) {
        if (this.selectedFile) {
          const fd = new FormData();
          fd.append('File', this.selectedFile);
          this.postService.uploadPostImage(post.postId, fd).subscribe(events => {
            post.imageUrl = events.imageUrl;
            this.toastify.show('You posted with photo successfully');
            this.isLoading = false;
            this.posted.emit(post);
            this.resetInput();
          });
        } else {
          this.toastify.show('You posted successfully');
          this.isLoading = false;
          this.posted.emit(post);
          this.resetInput();
        }
      }
    }, error => console.log(error));
  }

  resetInput() {
    this.content = '';
    this.selectedFile = null;
    this.selectedGame = null;
    this.showPreview();
  }

  initialGame() {
    this.isGameLoading = true;
    if (!this.games.length) {
      this.gameService.getGames().subscribe(response => {
        this.isGameLoading = false;
        this.games = response;
        this.games.sort((a, b) => a.name.localeCompare(b.name));
      });
    } else {
      this.isGameLoading = false;
    }
  }

  searchGame() {
    let input, filter, a, i;
    input = document.getElementById("searchGame");
    filter = input.value.toUpperCase();
    const menu = document.getElementById("menu");
    a = menu.getElementsByTagName("button");
    let hidden = 0

    for (i = 0; i < a.length; i++) {
      const txtValue = a[i].textContent || a[i].innerText;
      if (txtValue.toUpperCase().indexOf(filter) > -1) {
        a[i].style.display = "";
        hidden--;
      } else {
        a[i].style.display = "none";
        hidden++;
      }
      if (hidden === a.length) {
        this.isNoGameFound = true;
      } else {
        this.isNoGameFound = false;
      }
    }
  }

  onSelectGame(game: Game) {
    this.selectedGame = game;
  }

}
