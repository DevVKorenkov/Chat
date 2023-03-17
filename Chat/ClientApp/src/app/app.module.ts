import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule, Routes} from '@angular/router';

import { AppComponent } from './app.component';
import { ChatComponent } from './chat/chat.component';
import { HomeComponent } from './home/home.component';
import { ClanListComponent } from './clan-list/clan-list.component';
import {ClanItemComponent} from "./clan-item/clan-item.component";
import { AuthComponent } from './auth/auth.component';
import {AuthInterceptorService} from "./services/auth/auth-interceptor.service";
import {AuthGuard} from "./services/auth/auth.guard";
import { HeaderComponent } from './header/header.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserItemComponent } from './user-item/user-item.component';
import { MessageItemComponent } from './message-item/message-item.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: ':name',
        component: UserListComponent
      }
    ]},
  {
    path: 'auth',
    component: AuthComponent
  },
  {
    path: 'chat/:chatname',
    component: ChatComponent,
    canActivate: [AuthGuard]
  },
  {
    path:'**',
    redirectTo: 'home'
  }
]

@NgModule({
  declarations: [
    AppComponent,
    ChatComponent,
    HomeComponent,
    ClanListComponent,
    ClanItemComponent,
    AuthComponent,
    HeaderComponent,
    HeaderComponent,
    UserListComponent,
    UserItemComponent,
    MessageItemComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    }
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
