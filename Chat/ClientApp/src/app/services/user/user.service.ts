import {User} from "../../models/user";
import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {IUser} from "../../chat/chat.component";
import {Subject, tap} from "rxjs";
import {take} from "rxjs/operators";
import {AuthService} from "../auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class UserService{

  userChange = new Subject<IUser>();
  usersChange = new Subject<User[]>();

  constructor(private http: HttpClient) {}
  getLogginedUser(): IUser | null{
    const userData: {
      token: string,
      user: User
    } = JSON.parse(localStorage.getItem('user') as string);
    if(!userData){
      return null;
    }

    return {
      token: userData.token,
      user: userData.user
    };
  }

  removeLogginedUser(){
    localStorage.clear();
  }

  setClan(clanName: string){
    return this.http.post(`${environment.baseAddress}user/setClan?clanName=${clanName}`, {})
      .pipe(
      tap(response => {
        this.getUsersFromClan(clanName)
          .pipe(take(1))
          .subscribe(response => {
            this.usersChange.next(response.users)
            }
          )
      })
    );
  }

  getUsersFromClan(clanName: string){
    return this.http.get<{message: string, users: User[]}>(`${environment.baseAddress}user/getAllUsersFromClan?clanName=${clanName}`)
  }
}
