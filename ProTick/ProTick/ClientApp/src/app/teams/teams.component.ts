import { Component, ViewChild, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { Team } from '../../classes/Team';
import { Employee } from '../../classes/Employee';
import { EmployeeTeam } from '../../classes/EmployeeTeam';
import { MatSort, MatDialog, MatTab, MatSelectModule, MatDatepickerModule} from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CreateTeamComponent } from '../create-team/create-team.component';
import { CreateEmployeeComponent } from '../create-employee/create-employee.component';
import { EmployeeService } from '../core/employee/employee.service';
import { EmployeeTeamService } from '../core/employee-team/employee-team.service';
import { MAT_DATE_FORMATS } from '@angular/material';
import { FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common'


@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css'],
  providers: [TeamService, EmployeeService, EmployeeTeamService],
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
  _teamID: number;
  date = Date.now();

  team: Team = {
    teamID: undefined,
    description: undefined,
    abbreviation: undefined
  }

  emp: Employee = {
    employeeID: undefined,
    firstName: undefined,
    lastName: undefined,
    dateOfBirth: undefined,
    hireDate: undefined,
    username: undefined,
    password: undefined,
    addressID: undefined
  }

  et: EmployeeTeam = {
    employeeTeamID: undefined,
    employeeID: undefined,
    roleID: undefined,
    teamID: undefined
  }

  displayedColumns: string[] = ['teamID', 'description', 'abbreviation', 'options'];
  displayedColumnsEmp: string[] = ['employeeID', 'firstName', 'lastName', 'dateOfBirth', 'hireDate', 'username', 'options'];
  
  expandedElement: Team | null;
  

  //@ViewChild(MatSort) sort: MatSort;

  constructor(public datepipe: DatePipe, private _employeeTeamService: EmployeeTeamService,
    private _teamService: TeamService,
    private _employeeService: EmployeeService, public dialog: MatDialog) { }
  
  ngOnInit() {
    this.getTeams();
    this.getEmployees();
    this._teamID = 0;
  }

  getTeams() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
  }

  getEmployees() {
    this._employeeService.getEmployees()
      .subscribe(data => this.allEmployees = data);
  }

  onAddTeam(): void {
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

  onAddEmp(): void {
    const dialogRef = this.dialog.open(
     
      CreateEmployeeComponent, {
      data: {
        firstName: this.emp.firstName,
        lastName: this.emp.lastName,
        dateOfBirth: this.emp.dateOfBirth,
        hireDate: this.emp.hireDate
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');

      if (result !== undefined) {
        this.emp.firstName = result.firstName;
        this.emp.lastName = result.lastName;
        this.emp.dateOfBirth = this.datepipe.transform(result.dateOfBirth, "yyyy-MM-dd hh:mm:ss");
        this.emp.hireDate = this.datepipe.transform(result.hireDate, "yyyy-MM-dd hh:mm:ss");
        this.emp.username = (this.emp.firstName.substr(0, 1) + "" + this.emp.lastName).toLowerCase();
        this.emp.password = (this.emp.firstName.substr(0, 1) + "" + this.emp.lastName).toLowerCase();
        this.emp.addressID = 1;

        this._employeeService.postEmployee(this.emp)
          .subscribe(data => {
            console.log(data);
            this.emp = data;
            
            if (result.selTeams.length > 0) {
              console.log("teams nicht leer");
              result.selTeams.forEach((team) => {
                this.et.employeeID = this.emp.employeeID;
                this.et.teamID = team.teamID;
                this.et.roleID = 1;

                this._employeeTeamService.postEmployeeTeam(this.et)
                  .subscribe(data => this.et = data);
              });
            }

            this.clearEmp();
            this.getEmployees();
          }); 
      }
    }
    );
  }

  onEditTeam(team: Team) {

  }

  onEditEmp(emp: Employee) {
   
  }

  onDeleteTeam(id: number) {
    this._teamService.deleteTeam(id)
      .subscribe(data => { this.getTeams(); });

    console.log('Team deleted.');
  }

  onDeleteEmp(id: number) {
    this._employeeService.deleteEmployee(id)
      .subscribe(data => { this.getEmployees(); });
  }

  clearTeam() {
    this.team.teamID = undefined;
    this.team.description = undefined;
    this.team.abbreviation = undefined;

    console.log('Team Properties cleared.');
  }

  clearEmp() {
    this.emp.employeeID = undefined;
    this.emp.firstName = undefined;
    this.emp.lastName = undefined;
    this.emp.dateOfBirth = undefined;
    this.emp.hireDate = undefined;
    this.emp.username = undefined;
    this.emp.password = undefined;
    this.emp.addressID = undefined;

    console.log('Emp Properties cleared.');
  }

  getEmployeesByTeamID(_teamID) {
    if (_teamID == 0) {
      this.getEmployees();
    }
    else {
      this._teamService.getEmployeesByTeamID(_teamID)
        .subscribe(data => this.allEmployees = data);
    }
    
  }
}
