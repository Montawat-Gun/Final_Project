import { Component, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { AuthService } from './Services/auth.service'
import { NotificationService } from './Services/notification.service';
import { UserService } from './Services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'SGamer';

  constructor(public authService: AuthService, private notification: NotificationService,
    public userService: UserService) { }


  ngOnInit(): void {
    if (!this.userService.user)
      this.userService.getCurrentUser();
    if (this.userService.getUserId())
      this.notification.createHubConnection();
  }

  ngOnDestroy(): void {
    this.notification.stopHubConnection();
  }

}
