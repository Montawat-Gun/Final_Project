import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Services/user.service';
import { User } from 'src/app/Models/User';
import { NotificationService } from 'src/app/Services/notification.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  usersFromSearch: User[] = [];
  searchString: string = null;
  isLoading: boolean = false;
  isNotificationLoading: boolean = false;

  constructor(private router: Router, private authService: AuthService, public userService: UserService,
    public notification: NotificationService) { }

  ngOnInit(): void {
  }

  isAdmin() {
    return this.userService.getUserRoles().includes('Administrator')
  }

  searchUser() {
    if (this.searchString !== '') {
      this.isLoading = true;
      this.userService.searchUser(this.searchString).subscribe(users => {
        this.usersFromSearch = users;
        this.isLoading = false;
      });
    }
  }

  hasNotification() {
    let notification = document.getElementById("notification");
    return (notification.childElementCount > 0)
  }

  onNotificationClick() {
    this.isNotificationLoading = true;
    this.notification.getNotifications(this.userService.getUserId()).subscribe(next => {
      this.isNotificationLoading = false;
      this.notification.notificationThreadSource.next(next);
    }, error => this.isNotificationLoading = false);
    this.notification.markAsRead();
  }

  onDeleteNotificationClick() {
    this.notification.deleteNotifications().subscribe();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['login']);
  }
}
