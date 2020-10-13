import { User } from './User';

export interface Notification {
    notificationId: number;
    senderId: string;
    sender: User;
    content: string;
    destination: string;
    timeNotification: string;
    isRead: boolean;
}