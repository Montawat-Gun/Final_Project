import { User } from './User';

export interface PostComment {
    commentId: number,
    content: string,
    user: User,
    timeComment: Date,
    
}