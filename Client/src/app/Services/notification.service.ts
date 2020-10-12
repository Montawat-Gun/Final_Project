import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { ToastifyService } from './toastify.service';
import { UserService } from './user.service';
import { Notification } from './../Models/Notification'
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  hubUrl: string = environment.hubUrl + 'notification';
  url: string = environment.url + 'notification/';
  private hubConnetion: signalR.HubConnection;

  notificationThreadSource = new BehaviorSubject<Notification[]>([]);
  notificationThread$ = this.notificationThreadSource.asObservable();
  count: number;

  constructor(private http: HttpClient, private userService: UserService, private toastifyService: ToastifyService) { }

  createHubConnection() {
    this.hubConnetion = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl + '?userId=' + this.userService.getUserId(), {
        accessTokenFactory: () => this.userService.getToken()
      })
      .withAutomaticReconnect().build();
    this.hubConnetion.start().catch(error => console.log(error));

    this.hubConnetion.on('ReceiveNotificationCount', notification => {
      this.count = notification.notificationCount;
    })

    this.hubConnetion.on('ReceiveNotification', notification => {
      this.count++;
      this.toastifyService.show(notification.content);
    })
  }

  getNotifications(userId: string) {
    return this.http.get<Notification[]>(this.url + userId);
  }

  markAsRead() {
    this.http.get(this.url + 'markasread/' + this.userService.getUserId()).subscribe(next => {
      this.count = 0
    }
    );
  }

  isHubConnected() {
    return this.hubConnetion;
  }

  stopHubConnection() {
    if (this.hubConnetion) {
      this.hubConnetion.stop();
    }
  }

  sendNotification(otherUserId: string, content: string, destination?: string) {
    if (this.isHubConnected)
      this.hubConnetion.send("SendNotification", otherUserId, this.userService.getUserId(), content, destination).then();
  }
}
