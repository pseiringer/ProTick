import { Component, ViewChild, ChangeDetectorRef } from '@angular/core';
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
export class TeamsComponent {

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
    const dialogRef = this.dialog.open(CreateTeamComponent, {
      data: { description: this.team.description }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.team.description = result.description;
      this.team.abbreviation = result.abbreviation;
      this._teamService.postTeam(this.team)
        .subscribe((x, y) => { console.log(x); console.log(y) });
      this.refresh();
    });
  }

  refresh() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
      this.changeDetectorRefs.detectChanges();
    });
  }
}

