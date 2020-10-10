import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Services/user.service';
import { User } from 'src/app/Models/User';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  usersFromSearch: User[] = null;
  searchString: string = null;

  constructor(private router: Router, private authService: AuthService, public userService: UserService) { }

  ngOnInit(): void {
  }

  searchUser() {
    if (this.searchString !== '') {
      this.userService.searchUser(this.searchString).subscribe(users => this.usersFromSearch = users);
    }  
    this.usersFromSearch = null
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['login']);
  }
}
