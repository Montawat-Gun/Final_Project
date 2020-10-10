import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { ToastifyService } from './toastify.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  hubUrl: string = environment.hubUrl + 'notification';
  private hubConnetion: signalR.HubConnection;

  constructor(private userService: UserService, private toastifyService: ToastifyService) { }

  createHubConnection() {
    this.hubConnetion = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl + '?userId=' + this.userService.getUserId(), {
        accessTokenFactory: () => this.userService.getToken()
      })
      .withAutomaticReconnect().build();
    this.hubConnetion.start().catch(error => console.log(error));

    this.hubConnetion.on('ReceiveNotification', message => {
      this.toastifyService.show(message);
    })
  }

  isHubConnected() {
    return this.hubConnetion;
  }

  stopHubConnection() {
    if (this.hubConnetion) {
      this.hubConnetion.stop();
    }
  }

  sendNotification(otherUserId: string, message: string) {
    if (this.isHubConnected)
      this.hubConnetion.invoke("SendNotification", otherUserId, message).then();
  }
}
