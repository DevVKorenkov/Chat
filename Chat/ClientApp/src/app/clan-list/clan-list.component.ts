import {Component, OnDestroy, OnInit} from '@angular/core';
import {Clan} from "../models/clan";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ClanService} from "../services/clan/clan.service";
import {take, tap} from "rxjs/operators";
import {pipe, Subscription} from "rxjs";

@Component({
  selector: 'app-clan-list',
  templateUrl: './clan-list.component.html',
  styleUrls: ['./clan-list.component.css']
})
export class ClanListComponent implements OnInit, OnDestroy{
  clanSub!: Subscription;
  newClanGroup!: FormGroup
  allClans!: Clan[];

  constructor(private clanService: ClanService) {}
  async ngOnInit() {
    this.newClanGroup = new FormGroup({
      name: new FormControl(null, Validators.required)
    });

    await this.clanService.getClans()
      .pipe(
        take(1),
        tap(data => {
          this.allClans = data.clans;
        })
      )
      .subscribe();

    this.clanSub = this.clanService.clansChange
      .subscribe(data => {
      if(data){
        this.allClans.push(data);
      }
    });
  }

  onNewClan(){
    this.clanService.addClan(this.newClanGroup.value.name)
      .pipe(
        take(1)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this.clanSub.unsubscribe();
  }
}
