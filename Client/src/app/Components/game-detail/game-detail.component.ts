import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { from } from 'rxjs';
import { GameDetail } from 'src/app/Models/GameDetail';
import { PostToList } from 'src/app/Models/PostToList';
import { PostService } from 'src/app/Services/post.service';
import { Interest } from 'src/app/Models/Interest'
import { GameInterestService } from 'src/app/Services/game-interest.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-game-detail',
  templateUrl: './game-detail.component.html',
  styleUrls: ['./game-detail.component.css']
})
export class GameDetailComponent implements OnInit {

  game: GameDetail;
  posts: PostToList[];
  isInterest: boolean;
  interest: Interest[] = [];

  constructor(private route: ActivatedRoute, private postService: PostService,
    private gameService: GameInterestService, private userService: UserService) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.route.data.subscribe(data => {
      this.game = data['game'];
      this.postService.getPostsGame(this.game.gameId, this.userService.getUserId()).subscribe(posts => this.posts = posts);
      this.gameService.getIsInterest(this.game.gameId).subscribe(response => {
        if (response === null)
          this.isInterest = false;
        else
          this.isInterest = true;
      })
      this.interest = [];
      this.interest.push({
        gameId: this.game.gameId,
        userId: this.userService.getUserId()
      })
    })
  }

  onAddtoInterest() {
    this.gameService.addInterests(this.interest).subscribe(next => {
      this.isInterest = !this.isInterest;
    });
  }

  onRemovetoInterest() {
    this.gameService.removeInterest(this.game.gameId).subscribe(next => {
      this.isInterest = !this.isInterest;
    });
  }

}
