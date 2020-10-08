import { Message } from 'src/app/Models/Message';
import { AfterViewChecked, Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { MessageService } from 'src/app/Services/message.service';
import { UserService } from 'src/app/Services/user.service';
import { ViewChild } from '@angular/core';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})

export class MessageComponent implements OnInit, AfterViewChecked, OnDestroy {

  @ViewChild('scroll') scroll;
  @ViewChild('closeModal') closeModal;

  contacts: User[] = null;
  messages: Message[] = null;
  otherUser: User = null;
  content: string = '';
  usersFromSearch: User[] = null;
  searchString: string = ''

  constructor(public messageService: MessageService, private userService: UserService) { }

  ngOnInit(): void {
    this.messageService.getContacts(this.userService.getUserId()).subscribe(contacts => {
      this.contacts = contacts
    });
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  scrollToBottom(): void {
    try {
      this.scroll.nativeElement.scrollTop = this.scroll.nativeElement.scrollHeight;
    } catch (err) { }
  }

  onContactClick(user: User) {
    if (this.contacts.find(u => u.id === user.id) === null) {
      this.contacts.unshift(user);
    }
    this.otherUser = user;
    this.messageService.createHubConnection(this.userService.getUserId(), user.id);
    this.closeModal.nativeElement.click();
  }

  searchUser() {
    if (this.searchString !== '') {
      this.userService.searchUser(this.searchString).subscribe(users => this.usersFromSearch = users);
    }
    this.usersFromSearch = null;
  }

  sendMessage() {
    if (this.otherUser.id === null)
      return;
    const message = {
      senderId: this.userService.getUserId(),
      recipientId: this.otherUser.id,
      content: this.content
    }
    this.messageService.sendMessage(message).then(() => {
      this.content = '';
    });
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

}
