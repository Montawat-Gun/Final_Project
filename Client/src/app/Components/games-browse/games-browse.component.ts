import { Component, OnInit } from '@angular/core';
import { Game } from 'src/app/Models/Game';
import { GameDetail } from 'src/app/Models/GameDetail';
import { GameInterestService } from 'src/app/Services/game-interest.service';

@Component({
  selector: 'app-games-browse',
  templateUrl: './games-browse.component.html',
  styleUrls: ['./games-browse.component.css']
})
export class GamesBrowseComponent implements OnInit {

  games: GameDetail[];

  constructor(private gameService: GameInterestService) { }

  ngOnInit(): void {
    this.gameService.getGames().subscribe(games => {
      this.games = games;
      this.games.sort((a, b) => a.name.localeCompare(b.name));
    });
  }

  searchGame() {
    let input, filter, a, i;
    input = document.getElementById("searchGame");
    filter = input.value.toUpperCase();
    const menu = document.getElementById("menu");
    a = menu.getElementsByTagName("a");
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
    }
  }

}
