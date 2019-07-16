import { Component, ViewChild, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { Team } from '../../classes/Team';
import { MatSort, MatDialog } from '@angular/material';
import { CreateTeamComponent } from '../create-team/create-team.component';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css'],
  providers: [TeamService]
})

export class TeamsComponent implements OnInit {

  allTeams: any = [];

  team: Team = {
    teamID: undefined,
    description: undefined,
    abbreviation: undefined
  }

  displayedColumns: string[] = ['teamID', 'description', 'abbreviation', 'options'];

  dataSource;

  //@ViewChild(MatSort) sort: MatSort;

  constructor(private _teamService: TeamService, public dialog: MatDialog) { }
  
  ngOnInit() {
    this.getTeams();
  }

  getTeams() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
  }

  onAdd(): void {
    const dialogRef = this.dialog.open(CreateTeamComponent, {
      data: { description: this.team.description, abbreviation: this.team.abbreviation }
    });
    
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');

      if (result !== undefined) {
        this.team.description = result.description;
        this.team.abbreviation = result.abbreviation;

        this._teamService.postTeam(this.team)
          .subscribe(data => {
            this.team = data,
            this.clearTeam(),
            this.getTeams()
          });
      }
    });
  }

  onEdit(team: Team) {

  }

  onDelete(id: number) {
    this._teamService.deleteTeam(id)
      .subscribe(data => { this.getTeams(); });

    console.log('Team deleted.');
  }

  clearTeam() {
    this.team.teamID = undefined;
    this.team.description = undefined;
    this.team.abbreviation = undefined;

    console.log('Team Properties cleared.');
  }
}
