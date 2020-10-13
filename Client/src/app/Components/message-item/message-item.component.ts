import { Message } from 'src/app/Models/Message';
import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/Models/User';
import * as moment from 'moment';

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
    this.message.timeSend = moment.parseZone(this.message.timeSend).local().format();
  }

}
