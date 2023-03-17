import {Component, OnDestroy, OnInit} from '@angular/core';
import {IUser} from "../chat/chat.component";
import {Subscription} from "rxjs";
import {UserService} from "../services/user/user.service";
import {AuthService} from "../services/auth/auth.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {
  user!: IUser | null;
  userSub!: Subscription;

  constructor(
    private userService: UserService,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userSub = this.userService.userChange.subscribe(user => this.user = user);
    this.user = this.userService.getLogginedUser();
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();
  }

  logout(){
    this.auth.logout();
    this.userService.removeLogginedUser();
    this.user = null;
    this.router.navigate(['/auth']);
  }
}
