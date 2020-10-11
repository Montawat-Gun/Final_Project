import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth.service'
import { Router } from '@angular/router';
import { UserService } from 'src/app/Services/user.service';
import { NotificationService } from 'src/app/Services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  model: (any) = {}
  errors = []
  isLoading: boolean = false;

  constructor(private authService: AuthService, private router: Router, public userService: UserService,
    private notification: NotificationService) { }

  ngOnInit(): void {

  }

  login() {
    this.isLoading = true;
    this.errors = []
    this.authService.login(this.model).subscribe(next => {
      this.router.navigateByUrl('/');
    }, error => {
      this.errors.push(error.error);
      this.isLoading = false;
    });
  }
}
