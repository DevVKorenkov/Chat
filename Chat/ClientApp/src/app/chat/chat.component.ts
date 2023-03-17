import {ChangeDetectionStrategy, Component, OnDestroy, OnInit} from '@angular/core';
import {User} from "../models/user";
import * as signalr from '@microsoft/signalr'
import {environment} from "../../environments/environment";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserService} from "../services/user/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {map, take} from "rxjs/operators";
import {ClanService} from "../services/clan/clan.service";
import {Message} from "../models/message";

export interface IUser{
  token: string,
  user: User
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy{
  private hubConnection = new signalr.HubConnectionBuilder()
    .withUrl(`${environment.baseAddress}chat`)
    .build();
  private startConnection = async () => {
    await this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log(`Connection error: ${err}`));
  }
  private messageListener = () => {
    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      this.messages.push(new Message(user, message));
      console.log(`Got message "${message}"`);
    });
  }

  private userEnteringListener = () => {
    this.hubConnection.on('UserItRoom', (user: string, message: string) => {
      alert(`${user} says:\n${message}`);
      this.router.navigate(['/home']);
    });
  }

  private leaveRoomListener = () => {
     this.hubConnection.on('LeaveRoom', (user: string, message: string) => {
      this.messages.push(new Message(user, message))
      console.log(this.messages)
    });
  }

  messages: Message[] = [];
  user!: IUser;
  chatRoomName!: string;
  messageForm!: FormGroup;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private clanService: ClanService,
    private router: Router) {}
  async ngOnInit(){
    this.messageForm = new FormGroup({
      message: new FormControl(null, Validators.required)
    });

    this.user = this.userService.getLogginedUser() as IUser;

    await this.startConnection();
    this.messageListener();
    this.userEnteringListener();
    this.leaveRoomListener();

    this.route.params.pipe(
      map(params => params['chatname'])
    )
      .subscribe(
        async chatName => {
          this.chatRoomName = chatName;
          await this.joinRoom(this.chatRoomName)
            .then(() => {
              this.clanService.getMessages(this.chatRoomName)
                .pipe(take(1))
                .subscribe(response =>{
                  if(response.messages) {
                    this.messages = response.messages
                  }
                });
            });
        }
      );
  }

  async joinRoom(roomName: string){
    await this.hubConnection.invoke('JoinClanRoom', {
      name: this.user.user.name,
      room: this.chatRoomName
    });
  }

  async sendMessage(){
    if(this.messageForm.valid)
    {
      await this.hubConnection.invoke('SendMessage', this.messageForm.value.message);
      this.clanService.addMessage(
        this.chatRoomName,
        new Message(this.user.user.name, this.messageForm.value.message))
        .pipe(take(1))
        .subscribe();
    }
  }

  async leaveChat(){
    await this.hubConnection.invoke('LeaveRoom');
    this.router.navigate(['/home']);
  }

  async ngOnDestroy() {
    await this.hubConnection.stop();
  }
}
