import { Component, ViewChild, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { Team } from '../../classes/Team';
import { MatSort, MatDialog, MatTab } from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CreateTeamComponent } from '../create-team/create-team.component';
import { EmployeeService } from '../core/employee/employee.service';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css'],
  providers: [TeamService, EmployeeService],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class TeamsComponent implements OnInit {

  allTeams: any = [];
  allEmployees: any = [];
  _processID: number;

  team: Team = {
    teamID: undefined,
    description: undefined,
    abbreviation: undefined
  }

  displayedColumns: string[] = ['teamID', 'description', 'abbreviation', 'options'];
  displayedColumnsEmp: string[] = ['employeeID', 'firstName', 'lastName', 'dateOfBirth', 'hireDate', 'username', 'options'];
  
  expandedElement: Team | null;
  

  //@ViewChild(MatSort) sort: MatSort;

  constructor(private _teamService: TeamService, private _employeeService: EmployeeService, public dialog: MatDialog) { }
  
  ngOnInit() {
    this.getTeams();
    this.getEmployees();
  }

  getTeams() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
  }

  getEmployees() {
    this._employeeService.getEmployees()
      .subscribe(data => this.allEmployees = data);
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

  getEmployeesByTeamID(_teamID) {
    this._teamService.getEmployeesByTeamID(_teamID)
      .subscribe(data => this.allEmployees = data);
  }
}
