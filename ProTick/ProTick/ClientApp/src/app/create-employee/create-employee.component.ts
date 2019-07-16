import { Component, Inject, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


export interface CreateEmployeeDialogData {
  firstName: string,
  lastName: string,
  dateOfBirth: string,
  hireDate: string,
  username: string,
  password: string,
  addressID: number,
  teamID: number
}

@Component({
  selector: 'app-create-employee',
  templateUrl: './create-employee.component.html',
  styleUrls: ['./create-employee.component.css'],
  providers: [TeamService],
})
export class CreateEmployeeComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CreateEmployeeComponent>, private _teamService: TeamService,
    @Inject(MAT_DIALOG_DATA) public data: CreateEmployeeDialogData) { }

  allTeams: any = [];

  ngOnInit() {
    this.getTeams();
    this.data.teamID = 1;
  }


  getTeams() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
  }

  onNoClickEmp(): void {
    this.dialogRef.close();
  }
}
