import { Component, ViewChild, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { Team } from '../../classes/Team';
import { Employee } from '../../classes/Employee';
import { EmployeeTeam } from '../../classes/EmployeeTeam';
import { Address } from '../../classes/Address';
import { MatSort, MatDialog, MatTab, MatSelectModule, MatDatepickerModule, MatTooltipModule, TooltipPosition, MatTable } from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CreateTeamComponent } from '../create-team/create-team.component';
import { CreateEmployeeComponent } from '../create-employee/create-employee.component';
import { YesNoComponent } from '../yes-no/yes-no.component';
import { EmployeeService } from '../core/employee/employee.service';
import { EmployeeTeamService } from '../core/employee-team/employee-team.service';
import { AddressService } from '../core/address/address.service';
import { MAT_DATE_FORMATS } from '@angular/material';
import { FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';

export interface EmployeeAddress {
  employeeID: number,
  firstName: string,
  lastName: string,
  dateOfBirth: string,
  hireDate: string,
  username: string,
  password: string,
  addressID: number,
  street: string,
  streetNumber: string,
  postalCode: string,
  country: string,
  city: string
}

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css'],
  providers: [TeamService, EmployeeService, EmployeeTeamService, AddressService],
  animations: [
    trigger('detailExpand', [
      state('collapsed, void', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})



export class TeamsComponent implements OnInit {

  @ViewChild(MatTable, { static: false }) table: MatTable<any>;

  allTeams: any = [];
  allEmployees: any = [];
  allEmployeeAddresses: any = [];

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

  address: Address = {
    addressID: undefined,
    street: undefined,
    streetNumber: undefined,
    postalCode: undefined,
    country: undefined,
    city: undefined
  }

  displayedColumns: string[] = ['teamID', 'abbreviation', 'options'];
  displayedColumnsEmp: string[] = ['employeeID', 'firstName', 'lastName', 'hireDate', 'username', 'options'];


  expandedElement: Team | null;
  expandedElementEmp: Employee | null;


  //@ViewChild(MatSort) sort: MatSort;

  constructor(public datepipe: DatePipe, private _employeeTeamService: EmployeeTeamService,
    private _teamService: TeamService,
    private _addressService: AddressService,
    private _employeeService: EmployeeService, public dialog: MatDialog) { }

  ngOnInit() {
    console.log("onInit");

    this.getTeams();
    this.getEmployees();
    console.log(this.allEmployeeAddresses);
    this._teamID = 0;
  }

  changeTab() {
    console.log("changeTab");
  }

  getTeams() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
  }

  getEmployees() {
    this._employeeService.getEmployees()
      .subscribe(data => {
        this.allEmployees = data;
        this.getEmployeeAddresses();
        this.table.renderRows();
        console.log("reloaded rows");
      });
  }

  getEmployeesByTeamID(_teamID) {
    if (_teamID == 0) {
      this.getEmployees();
    }
    else {
      this._teamService.getEmployeesByTeamID(_teamID)
        .subscribe(data => {
          this.allEmployees = data;
          this.getEmployeeAddresses();
          console.log("getEmpsByTeamID");

          console.log(this.allEmployeeAddresses);

          this.table.renderRows();
          console.log("reloaded rows");
        });
    }
  }

  getEmployeeAddresses() {
    this.allEmployees.forEach(emp => {
      this.allEmployeeAddresses = [];
      let empAdd: EmployeeAddress = {
        employeeID: undefined,
        firstName: undefined,
        lastName: undefined,
        dateOfBirth: undefined,
        hireDate: undefined,
        username: undefined,
        password: undefined,
        addressID: undefined,
        street: undefined,
        streetNumber: undefined,
        postalCode: undefined,
        country: undefined,
        city: undefined
      };
      empAdd.dateOfBirth = emp.dateOfBirth;
      empAdd.employeeID = emp.employeeID;
      empAdd.firstName = emp.firstName;
      empAdd.lastName = emp.lastName;
      empAdd.hireDate = emp.hireDate;
      empAdd.username = emp.username;
      empAdd.password = emp.password;
      empAdd.addressID = emp.addressID;

      this._addressService.getAddress(emp.addressID)
        .subscribe(data => {
          empAdd.street = data.street;
          empAdd.city = data.city;
          empAdd.country = data.country;
          empAdd.postalCode = data.postalCode;
          empAdd.streetNumber = data.streetNumber;

          this.allEmployeeAddresses.push(empAdd);

        });
    });
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
            this.team = data;

            if (result.selEmps.length > 0) {
              console.log("emps nicht leer");
              result.selEmps.forEach((emp) => {
                this.et.employeeID = emp.employeeID;
                this.et.teamID = this.team.teamID;
                this.et.roleID = 1;

                this._employeeTeamService.postEmployeeTeam(this.et)
                  .subscribe(data => this.et = data);
              });
            }

            this.clearTeam();
            this.clearET();
            this.getTeams();
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

        this.address.city = result.city;
        this.address.country = result.country;
        this.address.postalCode = result.postalCode;
        this.address.street = result.street;
        this.address.streetNumber = result.streetNumber;

        this._addressService.postAddress(this.address)    //add address
          .subscribe(data => {
            console.log(data);
            this.address = data;

            this.emp.addressID = this.address.addressID;  //add new addressID to emp

            this._employeeService.postEmployee(this.emp)  //add emp
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
                this.clearET();
                this.getEmployees();
              });


          })
      }
    }
    );
  }

  onEditTeam(ev: Event, team: Team) {
    ev.stopPropagation();
  }

  onEditEmp(ev: Event, emp: Employee) {
    ev.stopPropagation();
  }

  onDeleteTeam(ev: Event, id: number) {
    ev.stopPropagation();

    const dialogRef = this.dialog.open(YesNoComponent, {
      data: {
        title: "Löschen",
        text: "Wollen Sie das Team mit der ID " + id + " wirklich löschen?",
        no: "Nein",
        yes: "Ja"
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this._teamService.deleteTeam(id)
          .subscribe(data => { this.getTeams(); });

        console.log('Team deleted.');
      }
    });
  }

  onDeleteEmp(ev: Event, id: number) {
    ev.stopPropagation();


    const dialogRef = this.dialog.open(YesNoComponent, {
      data: {
        title: "Löschen",
        text: "Wollen Sie den Employee mit der ID " + id + " wirklich löschen?",
        no: "Nein",
        yes: "Ja"
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this._employeeService.deleteEmployee(id)
          .subscribe(data => { this.getEmployees(); });
      }
    });


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

  clearET() {
    this.et.employeeID = undefined;
    this.et.roleID = undefined;
    this.et.teamID = undefined;
    this.et.employeeTeamID = undefined;
  }
  
}
