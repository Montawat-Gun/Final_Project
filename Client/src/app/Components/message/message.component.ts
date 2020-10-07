import { Message } from 'src/app/Models/Message';
import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { MessageService } from 'src/app/Services/message.service';
import { UserService } from 'src/app/Services/user.service';
import { ViewChild } from '@angular/core';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit, AfterViewChecked {

  contacts: User[];
  messages: Message[];
  otherId: string = null;
  content: string = '';

  constructor(private messageService: MessageService, private userService: UserService) { }

  @ViewChild('scroll') scroll;

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

  onContactClick(id: string) {
    this.otherId = id;
    this.messageService.getMessages(this.userService.getUserId(), id)
      .subscribe(messages => {
        this.messages = messages;
      });
  }

  sendMessage() {
    if (this.otherId === null)
      return;
    const message = {
      senderId: this.userService.getUserId(),
      recipientId: this.otherId,
      content: this.content
    }
    this.messageService.postMessage(message).subscribe(response => {
      this.content = '';
    });
  }

}
