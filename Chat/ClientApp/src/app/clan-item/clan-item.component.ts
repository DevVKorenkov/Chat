import {Component, Input} from "@angular/core";
import {Clan} from "../models/clan"
import {UserService} from "../services/user/user.service";
import {take} from "rxjs/operators";
import {IUser} from "../chat/chat.component";

@Component({
  selector: "app-clan-item",
  templateUrl: "./clan-item.component.html",
  styleUrls: ['./clan-item.component.html'],
})
export class ClanItemComponent{
  @Input() item!: Clan;

  constructor(private userService: UserService) {}

  onApply(){
    this.userService.setClan(this.item.name).pipe(take(1)).subscribe(() => {
      let user = this.userService.getLogginedUser() as IUser;
      user.user.userClan = this.item;
      localStorage.setItem('user', JSON.stringify(user));
      this.userService.userChange.next(user);
    })
  }
}
