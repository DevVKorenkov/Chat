import {Component, OnDestroy, OnInit} from '@angular/core';
import {User} from "../models/user";
import {UserService} from "../services/user/user.service";
import {map, take, tap} from "rxjs/operators";
import {ActivatedRoute} from "@angular/router";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit, OnDestroy{
  users!: User[] | null
  clanName!: string;
  routeSub!: Subscription
  usersSub!: Subscription;
  constructor(private userService: UserService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.routeSub = this.routeSub = this.route.params
      .subscribe( params => {
        this.clanName = params['name'];
        this.userService.getUsersFromClan(this.clanName)
          .pipe(take(1))
          .subscribe({
          next: response => {
            this.users = response.users
          },
          error: err => {
            this.users = null
          }
        });
      });

    this.usersSub = this.userService.usersChange
      .subscribe(users => {
      this.users = users;
    })
  }

  ngOnDestroy(): void {
    this.routeSub.unsubscribe();
  }
}
