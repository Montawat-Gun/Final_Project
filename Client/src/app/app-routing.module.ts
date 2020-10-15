import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { AuthGuard } from './Guards/auth.guard'
import { UnAuthGuard } from './Guards/un-auth.guard'
import { ProfileComponent } from './Components/profile/profile.component';
import { HomeComponent } from './Components/home/home.component';
import { UserDetailResolver } from './Resolvers/user-detail.resolver'
import { PostDetailResolver } from './Resolvers/post-detail.resolver'
import { GameDetailResolver } from './Resolvers/game-detail.resolver'
import { PostDetailComponent } from './Components/post-detail/post-detail.component';
import { GameDetailComponent } from './Components/game-detail/game-detail.component';
import { MessageComponent } from './Components/message/message.component';
import { GamesBrowseComponent } from './Components/games-browse/games-browse.component';
import { AdminComponent } from './Components/admin/admin.component';
import { AdminGuard } from './Guards/admin.guard'

const routes: Routes = [
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'profile/:username', component: ProfileComponent, resolve: { user: UserDetailResolver } },
      { path: 'post/:postId', component: PostDetailComponent, resolve: { post: PostDetailResolver } },
      { path: 'game/:gameId', component: GameDetailComponent, resolve: { game: GameDetailResolver } },
      { path: 'browse', component: GamesBrowseComponent },
      { path: 'message', component: MessageComponent },
      { path: '', component: HomeComponent },
      { path: 'admin', component: AdminComponent, canActivate: [AdminGuard] }
    ]
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [UnAuthGuard],
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
    ]
  },

  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
