import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  user

  constructor(private router: Router, private authService: AuthService, private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getUser().subscribe(response => this.user = response);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['login']);
  }
}
