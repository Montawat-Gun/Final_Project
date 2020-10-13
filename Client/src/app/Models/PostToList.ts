import { User } from './User';
import { Game } from './Game';

export interface PostToList {
    postId: number,
    content: string,
    timePost: string,
    user: User,
    game: Game,
    imageUrl: string,
    likeCount: number,
    commentCount: number,
    isLike: boolean
}