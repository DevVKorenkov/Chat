import {Component, OnDestroy, OnInit} from '@angular/core';
import {UserService} from "../services/user/user.service";
import {IUser} from "../chat/chat.component";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy{
  user!: IUser | null;
  userSub!: Subscription;

  constructor(private userService: UserService){}

  ngOnInit(): void {
    this.userSub = this.userService.userChange.subscribe(user => this.user = user);
    this.user = this.userService.getLogginedUser();
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }
}
