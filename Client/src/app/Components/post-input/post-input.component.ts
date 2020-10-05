import { HttpEventType } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Game } from 'src/app/Models/Game';
import { User } from 'src/app/Models/User';
import { GameInterestService } from 'src/app/Services/game-interest.service';
import { PostService } from 'src/app/Services/post.service';

@Component({
  selector: 'app-post-input',
  templateUrl: './post-input.component.html',
  styleUrls: ['./post-input.component.css']
})
export class PostInputComponent implements OnInit {
  @Input() user: User;
  @Input() selectedGame: Game = null;

  content: string = ''
  games: Game[] = [];
  isNoGameFound: Boolean = false;
  selectedFile = null;

  constructor(private postService: PostService, private gameService: GameInterestService) { }

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
    const model = {
      content: this.content,
      userId: this.user.id,
      gameId: this.selectedGame.gameId
    }
    console.log(model);
    this.postService.createPost(model).subscribe(response => {
      const post = response
      console.log(response);
      if (post) {
        if (this.selectedFile) {
          const fd = new FormData();
          fd.append('File', this.selectedFile);
          this.postService.uploadPostImage(post.postId, fd).subscribe(events => {
            if (events.type === HttpEventType.UploadProgress) {
              console.log('Upload Progress: ' + Math.round(events.loaded / events.total * 100));
            }
          });
        }
      }
      this.content = '';
      this.selectedFile = null;
      this.selectedGame = null;
      this.showPreview();
    }, error => console.log(error));
  }

  initialGame() {
    if (!this.games.length) {
      this.gameService.getGames().subscribe(response => {
        this.games = response;
        this.games.sort((a, b) => a.name.localeCompare(b.name));
      });
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
