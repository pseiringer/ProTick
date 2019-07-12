import { Component, ViewChild, ChangeDetectorRef, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { Team } from '../../classes/Team';
import { MatTableModule, MatTableDataSource, MatSort, MatDialog } from '@angular/material';
import { CreateTeamComponent } from '../create-team/create-team.component';


//import { MatInputModule } from '@angular/material/input';

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
  @ViewChild(MatSort) sort: MatSort;

  constructor(private _teamService: TeamService, public dialog: MatDialog,
    private changeDetectorRefs: ChangeDetectorRef) { }
  
  ngOnInit() {
    this.refresh();
  }

  onAdd(): void {

    this.clear();

    const dialogRef = this.dialog.open(CreateTeamComponent, {
      data: { description: this.team.description, abbreviation: this.team.abbreviation }
    });
    
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      if (result !== undefined) {
        this.team.description = result.description;
        this.team.abbreviation = result.abbreviation;
        this._teamService.postTeam(this.team)
          .subscribe(data => this.team = data);

        this.refresh();
      }
    });
  }

  onEdit(team: Team) {

  }

  onDelete(id: number) {
    this._teamService.deleteTeam(id)
      .subscribe(data => { this.refresh(); });
    console.log('Team deleted.');
  }

  refresh() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
    
    console.log('Team List refreshed.');
  }

  clear() {
    this.team.teamID = undefined;
    this.team.description = undefined;
    this.team.abbreviation = undefined;

    console.log('Team Properties cleared.');
  }
  
}

