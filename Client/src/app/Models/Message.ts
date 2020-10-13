import { User } from './User';

export interface Message {
    messageId: number;
    content: number;
    timeSend: string;
    senderId: string;
    sender: User;
    recipientId: string;
}