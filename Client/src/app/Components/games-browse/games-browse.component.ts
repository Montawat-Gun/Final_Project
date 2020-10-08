import { Component, OnInit } from '@angular/core';
import { Game } from 'src/app/Models/Game';
import { GameInterestService } from 'src/app/Services/game-interest.service';

@Component({
  selector: 'app-games-browse',
  templateUrl: './games-browse.component.html',
  styleUrls: ['./games-browse.component.css']
})
export class GamesBrowseComponent implements OnInit {

  games: Game[];

  constructor(private gameService: GameInterestService) { }

  ngOnInit(): void {
    this.gameService.getGames().subscribe(games => this.games = games);
  }

}
