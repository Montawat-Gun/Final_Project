import { Game } from './Game';
import { PostToList } from './PostToList';

export interface User {
    id: string;
    username: string;
    email: string;
    gender: string;
    birthDate: string;
    description: string;
    imageUrl: string;
    followingCount: number;
    followerCount: number;
    isFollowing: boolean;
    posts: PostToList[];
    gameInterest: Game[];
    messageUnReadCount: number;
    roles: string[];
}