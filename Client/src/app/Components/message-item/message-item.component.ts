import { Message } from 'src/app/Models/Message';
import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';

@Component({
  selector: 'app-message-item',
  templateUrl: './message-item.component.html',
  styleUrls: ['./message-item.component.css']
})
export class MessageItemComponent implements OnInit {

  @Input() message: Message;
  @Input() user: User;
  
  constructor() { }

  ngOnInit(): void {
  }

}
