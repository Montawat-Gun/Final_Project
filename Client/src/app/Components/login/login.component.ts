import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  model: (any) = {}
  errors = []

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {

  }

  login() {
    this.errors = []
    this.authService.login(this.model).subscribe(next => {
      this.router.navigateByUrl('/favorite');
    }, error => {
      this.errors.push(error.error);
    });
  }
}
