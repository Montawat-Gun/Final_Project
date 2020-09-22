import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Genre } from 'src/app/Models/genre';
import { Interest } from 'src/app/Models/Interest';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-genre-item',
  templateUrl: './genre-item.component.html',
  styleUrls: ['./genre-item.component.css']
})
export class GenreItemComponent implements OnInit {

  jwtHelperService = new JwtHelperService;
  interest: Interest = new Interest

  @Input() genre: Genre;
  @Output() selected = new EventEmitter<any>();

  constructor() { }

  ngOnInit(): void {
  }

  select() {
    const token = localStorage.getItem('token');
    this.interest.userId = this.jwtHelperService.decodeToken(token).nameid;
    this.interest.genreId = this.genre.genreId;
    this.selected.emit(this.interest);
  }

}
