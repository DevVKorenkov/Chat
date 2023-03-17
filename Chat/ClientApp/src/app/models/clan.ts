import {User} from "./user";

export class Clan{
  constructor(public id: number, public name: string, public clanMembers: User[]) {}
}
