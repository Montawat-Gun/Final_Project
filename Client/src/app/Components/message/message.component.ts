import { Message } from 'src/app/Models/Message';
import { AfterViewChecked, Component, OnDestroy, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { MessageService } from 'src/app/Services/message.service';
import { UserService } from 'src/app/Services/user.service';
import { NotificationService } from 'src/app/Services/notification.service';
import { ViewChild } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})

export class MessageComponent implements OnInit, AfterViewChecked, OnDestroy {

  @ViewChild('scroll') scroll;
  @ViewChild('closeModal') closeModal;
  @ViewChild('closeDeleteModal') closeDeleteModal;
  messages: Message[] = null;
  content: string = '';
  usersFromSearch: User[] = null;
  searchString: string = ''
  isLoading: boolean = false;
  isDeleting: boolean = false;
  isSendingMessage: boolean = false;
  userFromParam: User;
  userToDelete: User = null;

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

  setMessagesToDelete(user: User) {
    this.userToDelete = user;
  }

  onDeleteMessages() {
    this.isDeleting = true;
    if (this.userToDelete === null)
      return
    this.messageService.deleteMessage(this.userToDelete.id).subscribe(next => {
      this.isDeleting = false;
      this.messageService.removeToContact(this.userToDelete);
      this.closeDeleteModal.nativeElement.click();
    });
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
    if (this.messageService.currentContact.id === null || this.content === '' || this.isSendingMessage)
      return;
    this.isSendingMessage = true;
    const message = {
      senderId: this.userService.getUserId(),
      recipientId: this.messageService.currentContact.id,
      content: this.content
    };
    this.messageService.sendMessage(message).then(() => {
      this.content = '';
      this.isSendingMessage = false;
    }, error => this.isSendingMessage = false);
    this.notificationService.sendNotification(this.messageService.currentContact.id,
      this.userService.user.username + ' Message: ' + message.content, 'message');
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

}
