import { Component, OnInit } from '@angular/core';
import { GenreInterestService } from '../../Services/genre-interest.service'
import { Genre } from '../../Models/genre'
import { Interest } from '../../Models/Interest'
import { Router, RouterLink } from '@angular/router';


@Component({
  selector: 'app-genre',
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.css']
})
export class GenreComponent implements OnInit {

  genres: Genre[]
  interests: Interest[] = []
  hasInterests: boolean = true;

  constructor(private genreInterest: GenreInterestService, private router: Router) { }

  ngOnInit(): void {
    this.genreInterest.getGenres().subscribe(response => this.genres = response);
    this.genreInterest.getInterests().subscribe(next => {
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
      this.genreInterest.addInterests(this.interests).subscribe(next => {
        this.hasInterests = true;
      });
    }
  }

}
