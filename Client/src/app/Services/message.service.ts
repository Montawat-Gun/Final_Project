import { HttpClient } from '@angular/common/http';
import { Message } from 'src/app/Models/Message';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../Models/User';
import * as signalR from "@microsoft/signalr";
import { UserService } from './user.service';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  url: string = environment.url + 'message/';
  hubUrl: string = environment.hubUrl + 'message';
  private hubConnetion: signalR.HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient, private userService: UserService) { }

  createHubConnection(userId: string, otherUserId: string) {
    this.hubConnetion = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl + '?currentUserId=' + userId + '&otherUserId=' + otherUserId, {
        accessTokenFactory: () => this.userService.getToken()
      })
      .withAutomaticReconnect().build();
    this.hubConnetion.start().catch(error => console.log(error));

    this.hubConnetion.on('ReceiveMessages', messages => {
      this.messageThreadSource.next(messages);
    })

    this.hubConnetion.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this.messageThreadSource.next([...messages, message]);
      })
    })
  }

  stopHubConnection() {
    if (this.hubConnetion) {
      this.hubConnetion.stop();
    }
  }

  getContacts(userId: string): Observable<User[]> {
    return this.http.get<User[]>(this.url + 'contact/' + userId);
  }

  getMessages(userId: string, otherUser: string): Observable<Message[]> {
    return this.http.get<Message[]>(this.url + userId + '/' + otherUser);
  }

  async sendMessage(message: any) {
    return this.hubConnetion.invoke('SendMessage', message).catch(error => console.log(error));
  }
}
