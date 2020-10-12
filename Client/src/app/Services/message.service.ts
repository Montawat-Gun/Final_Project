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
  currentContact: User = null;
  contacts: User[];
  private hubConnetion: signalR.HubConnection;
  messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient, private userService: UserService) { }

  createHubConnection() {
    this.hubConnetion = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl + '?userId=' + this.userService.getUserId(), {
        accessTokenFactory: () => this.userService.getToken()
      })
      .withAutomaticReconnect().build();
    this.hubConnetion.start().catch(error => console.log(error));

    this.http.get<User[]>(this.url + 'contact/' + this.userService.getUserId())
      .subscribe(contacts => {
        this.contacts = contacts
      });

    this.hubConnetion.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this.addToContact(message.sender);
        if (this.isCurrentContact(message)) {
          this.messageThreadSource.next([...messages, message]);
        }
      })
    })
  }

  stopHubConnection() {
    if (this.hubConnetion) {
      this.hubConnetion.stop();
      this.messageThreadSource.next([]);
      this.currentContact = null;
    }
  }

  isCurrentContact(message: Message) {
    if (!this.currentContact)
      return false;
    return !(this.currentContact.id !== message.senderId && this.currentContact.id !== message.recipientId);
  }

  addToContact(user: User) {
    let contact = this.contacts.find(u => u.id == user.id);
    if (user.id === this.userService.getUserId()) {
      return;
    }
    if (contact === undefined) {
      user.messageUnReadCount = 1;
      this.contacts.unshift(user);
    } else {
      contact.messageUnReadCount++;
    }

  }

  getContacts(userId: string): Observable<User[]> {
    return this.http.get<User[]>(this.url + 'contact/' + userId);
  }

  getMessages(userId: string, otherUser: string) {
    return this.http.get<Message[]>(this.url + userId + '/' + otherUser);
  }

  async sendMessage(message: any) {
    return this.hubConnetion.invoke('SendMessage', message).catch(error => console.log(error));
  }

  markAsRead(otherUserId: string) {
    return this.http.get(this.url + 'markasread/' + this.userService.getUserId() + '/' + otherUserId);
  }
}
