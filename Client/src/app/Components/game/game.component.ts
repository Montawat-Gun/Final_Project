import { Component, OnInit, ViewChild } from '@angular/core';
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

  error: string = '';
  games: Game[]
  interests: Interest[] = []
  hasInterests: boolean = true;

  constructor(private gameinterestService: GameInterestService, private router: Router) { }

  @ViewChild('showModal') showModal;
  @ViewChild('closeModal') closeModal;

  ngOnInit(): void {

    this.gameinterestService.getInterests().subscribe(next => {
      if (Array.isArray(next) && !next.length) {
        this.gameinterestService.getGames().subscribe(response => {
          this.games = response;
        });
        this.hasInterests = false;
        this.showModal.nativeElement.click()
      }
    })
  }

  addInterest(interest: Interest) {
    if (this.interests.some(e => e.gameId === interest.gameId)) {
      this.interests = this.interests.filter(({ gameId }) => gameId !== interest.gameId);
    } else {
      this.interests.push(interest);
    }
  }

  submit() {
    if (this.interests.length >= 5) {
      console.log(this.interests);
      this.gameinterestService.addInterests(this.interests).subscribe(next => {
        this.hasInterests = true;
        this.closeModal.nativeElement.click();
      });
    } else {
      this.error = 'Please select at least 5 games.'
    }
  }


}
