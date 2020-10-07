import { User } from './User';

export interface Message {
    messageId: number;
    content: number;
    timeSend: Date;
    sender:User;
}