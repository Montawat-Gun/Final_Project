import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { Router } from '@angular/router';
import * as moment from 'moment';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model: (any) = {}
  birthDate: (any) = {}

  errors = []

  days = [];
  months = [
    "Jan", "Feb", "Mar",
    "Apr", "May", "June", "July",
    "Aug", "Sept", "Oct",
    "Nov", "Dec"
  ];

  years = [];

  isLoading: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    for (let i = 1; i <= 31; i++) {
      this.days.push(i);
    }
    for (let i = 2020; i >= 1930; i--) {
      this.years.push(i);
    }
  }

  register() {
    this.isLoading = true;
    this.errors = []
    let day = this.birthDate.day;
    let month = this.months.indexOf(this.birthDate.month) + 1;
    let year = this.birthDate.year;
    let date = day + "-" + month + "-" + year;
    if (!moment(date, "DD/MM/YYYY").isValid()) {
      this.isLoading = false;
      this.errors.push("Your birth of date is invalid.")
      return;
    }

    this.model.birthDate = new Date(this.birthDate.day + this.birthDate.month + this.birthDate.year);
    this.authService.register(this.model).subscribe(next => {
      this.router.navigateByUrl('/login');
    }, error => {
      this.isLoading = false;
      error.error.forEach(e => {
        this.errors.push(e.description);
      });
    });
  }

}
