import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import { MessageService } from 'src/app/Services/message.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-message-contact',
  templateUrl: './message-contact.component.html',
  styleUrls: ['./message-contact.component.css']
})
export class MessageContactComponent implements OnInit {

  contacts: User[];

  constructor(private messageService: MessageService, private userService: UserService) { }

  ngOnInit(): void {
    
  }
}
