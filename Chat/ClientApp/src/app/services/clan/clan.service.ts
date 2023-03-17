import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Clan} from "../../models/clan";
import {BehaviorSubject, tap} from "rxjs";
import {Message} from "../../models/message";

export interface ClansResponseData{
  message: string,
  clans: Clan[]
}

export interface ClanResponseData{
  message: string,
  clan: Clan
}

@Injectable({
  providedIn: 'root'
})
export class ClanService{
  clansChange = new BehaviorSubject<Clan | null>(null);
  constructor(private http: HttpClient) {}

  getClans(){
    return this.http.get<ClansResponseData>(environment.baseAddress + 'clan/getAllClans');
  }

  getClan(clanName: string){
    let params = new HttpParams().set('name', clanName);
    return this.http.get<ClanResponseData>(`${environment.baseAddress}clan/getClan`,{params: params});
  }

  addClan(clanName: string){
    let params = new HttpParams().set('name', clanName);
    return this.http.get<ClanResponseData>(`${environment.baseAddress}clan/addClan`, {params: params})
      .pipe(
        tap(data => {
          this.clansChange.next(data.clan);
        })
      );
  }

  addMessage(clanName: string, message: Message){
    return this.http.post(`${environment.baseAddress}clan/setMessage?clanName=${clanName}`, message)
  }

  getMessages(clanName: string){
    let params = new HttpParams().set('clanName', clanName);
    return this.http.get<{messages: Message[]}>(`${environment.baseAddress}clan/getMessages`, {params: params})
  }
}
