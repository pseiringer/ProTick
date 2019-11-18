import { Component, ViewChild, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { Team } from '../../classes/Team';
import { Employee } from '../../classes/Employee';
import { EmployeeTeam } from '../../classes/EmployeeTeam';
import { Address } from '../../classes/Address';
import { MatSort, MatDialog, MatTab, MatSelectModule, MatDatepickerModule, MatTooltipModule, TooltipPosition, MatTable } from '@angular/material';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CreateTeamComponent } from '../teams/create-team/create-team.component';
import { CreateEmployeeComponent } from '../teams/create-employee/create-employee.component';
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
  phoneNumber: string,
  email: string,
  dateOfBirth: string,
  hireDate: string,
  username: string,
  roleID: number,
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

  @ViewChild(MatTable, { static: false }) teamTable: MatTable<any>;
  @ViewChild('empTable', { read: MatTable, static: false }) empTable: MatTable<any>;

  allTeams: any = [];
  allEmployees: Employee[] = [];
  allEmployeeAddresses: EmployeeAddress[] = [];

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
    phoneNumber: undefined,
    email: undefined,
    dateOfBirth: undefined,
    hireDate: undefined,
    username: undefined,
    addressID: undefined,
    roleID: undefined
  }

  et: EmployeeTeam = {
    employeeTeamID: undefined,
    employeeID: undefined,
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

    this.getTeams();
    this.getEmployees();
    this._teamID = 0;
  }

  changeTab() {
    this.getEmployees();
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
        this.empTable.renderRows();
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

          this.empTable.renderRows();
        });
    }
  }

  getEmployeeAddresses() {
    this.allEmployeeAddresses = [];
    this.allEmployees.forEach(emp => {
      let empAdd: EmployeeAddress = {
        employeeID: undefined,
        firstName: undefined,
        lastName: undefined,
        phoneNumber: undefined,
        email: undefined,
        dateOfBirth: undefined,
        hireDate: undefined,
        username: undefined,
        roleID: undefined,
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
      empAdd.phoneNumber = emp.phoneNumber;
      empAdd.email = emp.email;
      empAdd.hireDate = emp.hireDate;
      empAdd.username = emp.username;
      empAdd.roleID = emp.roleID;
      empAdd.addressID = emp.addressID;
      if (empAdd.addressID > 0) {



        this._addressService.getAddress(emp.addressID)
          .subscribe(data => {
            empAdd.street = data.street;
            empAdd.city = data.city;
            empAdd.country = data.country;
            empAdd.postalCode = data.postalCode;
            empAdd.streetNumber = data.streetNumber;

            this.allEmployeeAddresses.push(empAdd);

            this.empTable.renderRows();

          });
      }
      else {
        this.allEmployeeAddresses.push(empAdd);
        this.empTable.renderRows();
      }

    });
  }

  onAddTeam(): void {
    const dialogRef = this.dialog.open(CreateTeamComponent, {
      data: { description: this.team.description, abbreviation: this.team.abbreviation }
    });

    dialogRef.afterClosed().subscribe(result => {

      if (result !== undefined) {
        this.team.description = result.description;
        this.team.abbreviation = result.abbreviation;

        this._teamService.postTeam(this.team)
          .subscribe(data => {
            this.team = data;

            if (result.selEmps.length > 0) {
              result.selEmps.forEach((emp) => {
                this.et.employeeID = emp.employeeID;
                this.et.teamID = this.team.teamID;

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

      if (result !== undefined) {
        this.emp.firstName = result.firstName;
        this.emp.lastName = result.lastName;
        this.emp.phoneNumber = result.phoneNumber;
        this.emp.email = result.email;
        this.emp.dateOfBirth = this.datepipe.transform(result.dateOfBirth, "yyyy-MM-dd hh:mm:ss");
        this.emp.hireDate = this.datepipe.transform(result.hireDate, "yyyy-MM-dd hh:mm:ss");
        this.emp.username = (this.emp.firstName.substr(0, 1) + "" + this.emp.lastName.substr(0, 15)).toLowerCase();
        this.emp.roleID = 1;


        if (result.city === undefined && result.country === undefined && result.postalCode === undefined && result.street === undefined && result.streetNumber === undefined) {
          this._employeeService.postEmployee(this.emp)  //add emp
            .subscribe(data => {
              this.emp = data;

              if (result.selTeams.length > 0) {
                result.selTeams.forEach((team) => {
                  this.et.employeeID = this.emp.employeeID;
                  this.et.teamID = team.teamID;

                  this._employeeTeamService.postEmployeeTeam(this.et)
                    .subscribe(data => this.et = data);
                });
              }

              this.clearEmp();
              this.clearET();
              this.getEmployees();
            });
        }
        else {
          this.address.city = result.city;
          this.address.country = result.country;
          this.address.postalCode = result.postalCode;
          this.address.street = result.street;
          this.address.streetNumber = result.streetNumber;


          this._addressService.postAddress(this.address)    //add address
            .subscribe(data => {
              this.address = data;

              this.emp.addressID = this.address.addressID;  //add new addressID to emp

              this._employeeService.postEmployee(this.emp)  //add emp
                .subscribe(data => {
                  this.emp = data;

                  if (result.selTeams.length > 0) {
                    result.selTeams.forEach((team) => {
                      this.et.employeeID = this.emp.employeeID;
                      this.et.teamID = team.teamID;

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
    }
    );
  }

  onEditTeam(ev: Event, team: Team) {
    ev.stopPropagation();

    let selectedEmps: Employee[];
    let temp: Employee[] = [];
    this._teamService.getEmployeesByTeamID(team.teamID).subscribe(data => {
      selectedEmps = data;
      selectedEmps.forEach(x => temp.push(x));
      

      const dialogRef = this.dialog.open(CreateTeamComponent, {
        data: {
          teamID: team.teamID,
          description: team.description,
          abbreviation: team.abbreviation,
          selEmps: selectedEmps
        }
      });

      dialogRef.afterClosed().subscribe(result => {

        if (result !== undefined) {

          this._teamService.putTeam(result.teamID, result)
            .subscribe(data => {
              this.team = data;
              

              temp.forEach(emp => {
                if (result.selEmps.some(e => e.employeeID === emp.employeeID) == false) {
                  this._employeeTeamService.deleteEmployeeTeamByTeamAndEmployeeID(result.teamID, emp.employeeID)
                    .subscribe();
                }
              });

              result.selEmps.forEach(emp => {
                if (temp.some(e => e.employeeID === emp.employeeID) == false) {
                  let et: EmployeeTeam = {
                    employeeTeamID: undefined,
                    employeeID: emp.employeeID,
                    teamID: result.teamID
                  }
                  this._employeeTeamService.postEmployeeTeam(et)
                    .subscribe(data => et = data);
                }
              });

              this.clearTeam();
              this.clearET();
              this.getTeams();
            });
        }
      });
    });
  }

  onEditEmp(ev: Event, emp: Employee) {
    ev.stopPropagation();

    let selectedTeams: Team[];
    let temp: Team[] = [];
    this._employeeService.getTeamsByEmployeeID(emp.employeeID).subscribe(data => {
      selectedTeams = data;
      selectedTeams.forEach(x => temp.push(x));
      
      if (emp.addressID <= 0) {
        const dialogRef = this.dialog.open(CreateEmployeeComponent, {
          data: {
            employeeID: emp.employeeID,
            firstName: emp.firstName,
            lastName: emp.lastName,
            phoneNumber: emp.phoneNumber,
            email: emp.email,
            dateOfBirth: emp.dateOfBirth,
            hireDate: emp.hireDate,
            username: emp.username,
            addressID: emp.addressID,
            street: undefined,
            streetNumber: undefined,
            postalCode: undefined,
            country: undefined,
            city: undefined,
            roleID: undefined,
            selTeams: selectedTeams
          }
        });

        dialogRef.afterClosed().subscribe(result => {

          if (result !== undefined) {
            if (result.city !== undefined || result.country !== undefined || result.postalCode !== undefined || result.street !== undefined || result.streetNumber !== undefined) {

              this.address.addressID = undefined;
              this.address.street = result.street;
              this.address.city = result.city;
              this.address.streetNumber = result.streetNumber;
              this.address.country = result.country;
              this.address.postalCode = result.postalCode;


              this._addressService.postAddress(this.address)
                .subscribe(add => {
                  this.address = add;
                  result.addressID = this.address.addressID;

                  this._employeeService.putEmployee(result.employeeID, result)
                    .subscribe(data => {
                      this.emp = data;
                      

                          temp.forEach(team => {
                            if (result.selTeams.some(t => t.teamID === team.teamID) == false) {
                              this._employeeTeamService.deleteEmployeeTeamByTeamAndEmployeeID(team.teamID, result.employeeID)
                                .subscribe();
                            }
                          });

                          result.selTeams.forEach(team => {
                            if (temp.some(t => t.teamID === team.teamID) == false) {
                              let et: EmployeeTeam = {
                                employeeTeamID: undefined,
                                employeeID: result.employeeID,
                                teamID: team.teamID
                              }
                              this._employeeTeamService.postEmployeeTeam(et)
                                .subscribe(data => et = data);
                            }
                          });

                          this.clearEmp();
                          this.clearET();
                          this.getEmployees();
                        
                    });
                })
            }
            else {
              this._employeeService.putEmployee(result.employeeID, result)
                .subscribe(data => {
                  this.emp = data;

                      temp.forEach(team => {
                        if (result.selTeams.some(t => t.teamID === team.teamID) == false) {
                          this._employeeTeamService.deleteEmployeeTeamByTeamAndEmployeeID(team.teamID, result.employeeID)
                            .subscribe();
                        }
                      });

                      result.selTeams.forEach(team => {
                        if (temp.some(t => t.teamID === team.teamID) == false) {
                          let et: EmployeeTeam = {
                            employeeTeamID: undefined,
                            employeeID: result.employeeID,
                            teamID: team.teamID
                          }
                          this._employeeTeamService.postEmployeeTeam(et)
                            .subscribe(data => et = data);
                        }
                      });

                      this.clearEmp();
                      this.clearET();
                      this.getEmployees();
                });
            }
          }
        });
      }
      else {
        this._addressService.getAddress(emp.addressID).subscribe(addData => {
          let add: Address = addData;

          const dialogRef = this.dialog.open(CreateEmployeeComponent, {
            data: {
              employeeID: emp.employeeID,
              firstName: emp.firstName,
              lastName: emp.lastName,
              phoneNumber: emp.phoneNumber,
              email: emp.email,
              dateOfBirth: emp.dateOfBirth,
              hireDate: emp.hireDate,
              username: emp.username,
              addressID: emp.addressID,
              roleID: emp.roleID,
              street: add.street,
              streetNumber: add.streetNumber,
              postalCode: add.postalCode,
              country: add.country,
              city: add.city,
              selTeams: selectedTeams
            }
          });

          dialogRef.afterClosed().subscribe(result => {

            if (result !== undefined) {

              this._employeeService.putEmployee(result.employeeID, result)
                .subscribe(data => {
                  this.emp = data;

                  this._addressService.putAddress(result.addressID, result)
                    .subscribe(addData => {

                      temp.forEach(team => {
                        if (result.selTeams.some(t => t.teamID === team.teamID) == false) {
                          this._employeeTeamService.deleteEmployeeTeamByTeamAndEmployeeID(team.teamID, result.employeeID)
                            .subscribe();
                        }
                      });

                      result.selTeams.forEach(team => {
                        if (temp.some(t => t.teamID === team.teamID) == false) {
                          let et: EmployeeTeam = {
                            employeeTeamID: undefined,
                            employeeID: result.employeeID,
                            teamID: team.teamID
                          }
                          this._employeeTeamService.postEmployeeTeam(et)
                            .subscribe(data => et = data);
                        }
                      });

                      this.clearEmp();
                      this.clearET();
                      this.getEmployees();
                    });
                });
            }
          });
        });
      }
    });
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
      if (result === true) {
        this._teamService.deleteTeam(id)
          .subscribe(data => { this.getTeams(); });
        
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
    
  }

  clearEmp() {
    this.emp.employeeID = undefined;
    this.emp.firstName = undefined;
    this.emp.lastName = undefined;
    this.emp.phoneNumber = undefined;
    this.emp.email = undefined;
    this.emp.dateOfBirth = undefined;
    this.emp.hireDate = undefined;
    this.emp.username = undefined;
    this.emp.roleID = undefined;
    this.emp.addressID = undefined;
  }

  clearET() {
    this.et.employeeID = undefined;
    this.et.teamID = undefined;
    this.et.employeeTeamID = undefined;
  }

}
