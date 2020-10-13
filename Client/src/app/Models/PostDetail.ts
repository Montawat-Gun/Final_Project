import { User } from './User';
import { Game } from './Game';
import { PostComment } from './PostComment';

export interface PostDetail {
    postId: number,
    content: string,
    timePost: string,
    user: User,
    game: Game,
    imageUrl: string,
    likeCount: number,
    commentCount: number,
    isLike: boolean,
    likes: User[],
    comments: PostComment[],
}