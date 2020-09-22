import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-suggest',
  templateUrl: './suggest.component.html',
  styleUrls: ['./suggest.component.css']
})
export class SuggestComponent implements OnInit {

  users: User[];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getSuggestUsers().subscribe(response => {
      this.users = response;
      console.log(this.users);
    });
  }

  follow(user:User){
    this.users =this.users.filter(u => u.username !== user.username)
  }

}
