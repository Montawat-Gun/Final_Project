import { User } from './User';

export interface Message {
    messageId: number;
    content: number;
    timeSend: Date;
    senderId: string;
    sender: User;
    recipientId: string;
}