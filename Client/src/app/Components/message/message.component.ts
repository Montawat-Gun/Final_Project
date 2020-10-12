import { Message } from 'src/app/Models/Message';
import { AfterViewChecked, Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { MessageService } from 'src/app/Services/message.service';
import { UserService } from 'src/app/Services/user.service';
import { NotificationService } from 'src/app/Services/notification.service';
import { ViewChild } from '@angular/core';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})

export class MessageComponent implements OnInit, AfterViewChecked, OnDestroy {

  @ViewChild('scroll') scroll;
  @ViewChild('closeModal') closeModal;
  messages: Message[] = null;
  content: string = '';
  usersFromSearch: User[] = null;
  searchString: string = ''
  isLoading: boolean = false;
  userFromParam: User;

  constructor(public messageService: MessageService, public userService: UserService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.messageService.createHubConnection();
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
    this.isLoading = true;
    const contact = this.messageService.contacts.find(u => u.id === user.id)
    if (contact === undefined)
      this.messageService.contacts.unshift(user);
    this.messageService.currentContact = user;
    this.messageService.getMessages(this.userService.getUserId(), user.id).
      subscribe(messages => {
        this.messageService.messageThreadSource.next(messages);
        this.isLoading = false;
      });
    this.markAsRead();
    this.closeModal.nativeElement.click();
  }

  markAsRead() {
    if (this.messageService.currentContact) {
      this.messageService.markAsRead(this.messageService.currentContact.id).subscribe(() => {
        this.messageService.currentContact.messageUnReadCount = 0;
      });
    }
  }

  searchUser() {
    if (this.searchString !== '') {
      this.userService.searchUser(this.searchString).subscribe(users => this.usersFromSearch = users);
    }
    this.usersFromSearch = null;
  }

  sendMessage() {
    if (this.messageService.currentContact.id === null || this.content === '')
      return;
    const message = {
      senderId: this.userService.getUserId(),
      recipientId: this.messageService.currentContact.id,
      content: this.content
    };
    this.messageService.sendMessage(message).then(() => {
      this.content = '';
    });
    this.notificationService.sendNotification(this.messageService.currentContact.id,
      this.userService.user.username + ' Message: ' + message.content, 'message');
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

}
