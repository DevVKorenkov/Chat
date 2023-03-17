import {Clan} from "./clan";

export class User{
  constructor(public id: string, public name: string, public userClan: Clan) {}
}
