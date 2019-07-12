import { Component, OnInit, ViewChild } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { MatTableModule } from '@angular/material';
import { MatTableDataSource, MatSort } from '@angular/material';


//import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css'],
  providers: [TeamService]
})
export class TeamsComponent implements OnInit {

  allTeams: any = [];
  displayedColumns: string[] = ['teamID', 'description', 'abbreviation', 'options'];
  dataSource;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private _teamService: TeamService) { }

  



  ngOnInit() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
    this.dataSource = new MatTableDataSource(this.allTeams);
    this.dataSource.sort = this.sort;
  }
}

