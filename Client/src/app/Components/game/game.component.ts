import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Game } from 'src/app/Models/Game';
import { Interest } from 'src/app/Models/Interest';
import { GameInterestService } from '../../Services/game-interest.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {

  games: Game[]
  interests: Interest[] = []
  hasInterests: boolean = true;

  constructor(private gameinterestService: GameInterestService, private router: Router) { }

  ngOnInit(): void {
    this.gameinterestService.getGames().subscribe(response => this.games = response);
    this.gameinterestService.getInterests().subscribe(next => {
      if (Array.isArray(next) && !next.length) {
        this.hasInterests = false;
      }
    })
  }

  addInterest(interest: Interest) {
    if (this.interests.includes(interest)) {
      const index = this.interests.indexOf(interest, 0);
      delete this.interests[index];
    } else {
      this.interests.push(interest);
    }
  }

  submit() {
    if (this.interests.length >= 3) {
      console.log(this.interests);
      this.gameinterestService.addInterests(this.interests).subscribe(next => {
        this.hasInterests = true;
      });
    }
  }


}
