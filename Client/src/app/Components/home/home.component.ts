import { Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { NotificationService } from 'src/app/Services/notification.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(public userService: UserService, private notification: NotificationService) { }

  ngOnInit(): void {
    if (!this.userService.user)
      this.userService.getCurrentUser();
    if (!this.notification.isHubConnected())
      this.notification.createHubConnection();
  }



}
