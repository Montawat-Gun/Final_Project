import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  user: User

  constructor(private route: ActivatedRoute, private userServices: UserService) { }

  ngOnInit(): void {
    this.loadUser();
  }

  isCurrentUser() {
    return this.user.id === this.userServices.getUserId();
  }

  loadUser() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    })
  }

}
