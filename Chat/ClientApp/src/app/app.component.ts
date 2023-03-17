import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from "./services/auth/auth.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy{
  isAuthenticated = false;
  private userSub!: Subscription;
  constructor(private authService: AuthService) {}
  ngOnInit(): void {
    this.authService.autoLogin();
    this.userSub = this.authService.userAuthEvent.subscribe(user => {
      this.isAuthenticated = !!user;
    });
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }
}
