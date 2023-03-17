import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {ResponseStatus} from "../../models/responseStatus";
import {User} from "../../models/user";
import {BehaviorSubject, catchError, tap, throwError} from "rxjs";
import {UserService} from "../user/user.service";

export interface AuthResponseData{
  status: ResponseStatus,
  token: string,
  user: User
}

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  readonly baseAddress = 'http://localhost:5099/auth/'

  userAuthEvent = new BehaviorSubject<{ token: string, user: User } | null>(null)
  constructor(
    private http: HttpClient,
    private userService: UserService) {}

  signup(creds: {name: string, password: string}){
    return this.http.post<AuthResponseData>(this.baseAddress + 'signup', creds)
      .pipe(
        catchError(this.errorHandler),
        tap(this.authHandler())
      );
  }

  login(creds: { name: string, password: string }) {
    return this.http.post<AuthResponseData>('http://localhost:5099/auth/login', creds)
      .pipe(
        catchError(this.errorHandler),
        tap(this.authHandler())
      );
  }

  logout() {
    return  this.http.get<AuthResponseData>('http://localhost:5099/auth/logout')
      .pipe(
        catchError(this.errorHandler),
        tap(this.authHandler())
      );
  }

  autoLogin(){
    const loadedUser = this.userService.getLogginedUser();

    if (loadedUser && loadedUser.token) {
      this.userAuthEvent.next(loadedUser);
    }
  }

  private authHandler() {
    return {
      next: (responseData: AuthResponseData) => {
        const user = new User(
          responseData.user.id,
          responseData.user.name,
          responseData.user.userClan);

        let authUser = {
          token: responseData.token,
          user: user
        }

        localStorage.setItem('user', JSON.stringify(authUser));
      }
    }
  }

  private errorHandler(err: HttpErrorResponse){
    let errorMessage = `Status: ${err.status}. Message: ${err.error.message}`;

    return throwError(() => errorMessage);
  }
}
