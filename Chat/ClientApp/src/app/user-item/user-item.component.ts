import {Component, Input} from '@angular/core';
import {User} from "../models/user";

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css']
})
export class UserItemComponent {

  @Input() item!: User;

}
