import { User } from './User';
import { Game } from './Game';

export interface PostToList {
    postId: number,
    content: string,
    timePost: Date,
    user: User,
    game: Game,
    imageUrl: string,
    likeCount: number,
    commentCount: number,
    isLike: boolean
}