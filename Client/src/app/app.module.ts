import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './Components/register/register.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavComponent } from './Components/nav/nav.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { HomeComponent } from './Components/home/home.component';
import { GenreComponent } from './Components/genre/genre.component';
import { GenreItemComponent } from './Components/genre-item/genre-item.component';
import { SuggestComponent } from './Components/suggest/suggest.component';
import { JwtModule } from '@auth0/angular-jwt';
import { UsersSuggestResolver } from './Resolvers/users-suggest.resolver';
import { SuggestItemComponent } from './Components/suggest-item/suggest-item.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NavComponent,
    ProfileComponent,
    HomeComponent,
    GenreComponent,
    GenreItemComponent,
    SuggestComponent,
    SuggestItemComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
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
