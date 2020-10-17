import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-suggest',
  templateUrl: './suggest.component.html',
  styleUrls: ['./suggest.component.css']
})
export class SuggestComponent implements OnInit {

  usersSuggest: User[];
  allUsers: User[];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getSuggestUsers().subscribe(response => {
      this.usersSuggest = response;
    });
  }

  removeFromSuggest(user: User) {
    this.usersSuggest = this.usersSuggest.filter(u => u.username !== user.username)
  }

  initialUsers() {
    this.userService.getAllUsers().subscribe(users => {
      this.allUsers = users;
    });
  }

}
