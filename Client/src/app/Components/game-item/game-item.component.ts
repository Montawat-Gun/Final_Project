import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Game } from 'src/app/Models/Game';
import { Interest } from 'src/app/Models/Interest';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-game-item',
  templateUrl: './game-item.component.html',
  styleUrls: ['./game-item.component.css']
})
export class GameItemComponent implements OnInit {

  interest: Interest;

  @Input() game: (any);
  @Output() selected = new EventEmitter<any>();

  constructor(private userService: UserService) { }

  ngOnInit(): void {
  }

  select() {
    this.interest = {
      userId: this.userService.getUserId(),
      gameId: this.game.gameId
    }
    
    this.selected.emit(this.interest);
  }

}
