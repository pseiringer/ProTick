import { Component, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';

//import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css'],
  providers: [TeamService]
})
export class TeamsComponent implements OnInit {

  allTeams: any = [];

  constructor(private _teamService: TeamService) { }

  ngOnInit() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
  }

}
