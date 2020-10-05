import { BrowserModule, HammerModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import 'hammerjs';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './Components/register/register.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavComponent } from './Components/nav/nav.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { HomeComponent } from './Components/home/home.component';
import { SuggestComponent } from './Components/suggest/suggest.component';
import { JwtModule } from '@auth0/angular-jwt';
import { UsersSuggestResolver } from './Resolvers/users-suggest.resolver';
import { GameComponent } from './Components/game/game.component';
import { GameItemComponent } from './Components/game-item/game-item.component';
import { EditProfileComponent } from './Components/edit-profile/edit-profile.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { PostComponent } from './Components/post/post.component';
import { PostItemComponent } from './Components/post-item/post-item.component';
import { PostDetailComponent } from './Components/post-detail/post-detail.component';
import { CommentItemComponent } from './Components/comment-item/comment-item.component';
import { PostInputComponent } from './Components/post-input/post-input.component';
import { UserItemComponent } from './Components/user-item/user-item.component';
import { GameDetailComponent } from './Components/game-detail/game-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NavComponent,
    ProfileComponent,
    HomeComponent,
    SuggestComponent,
    GameComponent,
    GameItemComponent,
    EditProfileComponent,
    PostComponent,
    PostItemComponent,
    PostDetailComponent,
    CommentItemComponent,
    PostInputComponent,
    UserItemComponent,
    GameDetailComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    ImageCropperModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return localStorage.getItem("token");
        },
        allowedDomains: ['localhost:8080'],
        disallowedRoutes: ['localhost:8080/api/auth']
      }
    }),
  ],
  providers: [
    UsersSuggestResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
