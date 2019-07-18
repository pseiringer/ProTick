import { Component, Inject, OnInit } from '@angular/core';
import { TeamService } from '../core/team/team.service';
import { Team } from '../../classes/Team';
import { MatDialogRef, MAT_DIALOG_DATA, MatListModule } from '@angular/material';



export interface CreateEmployeeDialogData {
  firstName: string,
  lastName: string,
  dateOfBirth: string,
  hireDate: string,
  username: string,
  password: string,
  addressID: number,
  teamID: number,
  selTeams: Team[]
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
  //selTeams: Team[] = [];

  selTeam: Team;

  ngOnInit() {
    this.getTeams();
    this.data.teamID = 1;
    this.data.selTeams = [];
  }


  getTeams() {
    this._teamService.getTeams()
      .subscribe(data => this.allTeams = data);
  }

  onAddTeam() {
    this._teamService.getTeam(this.data.teamID)
      .subscribe(data => {
        this.selTeam = data;

        if (this.data.selTeams.some(e => e.teamID === this.selTeam.teamID) == false) {
          this.data.selTeams.push(this.selTeam);
        }
      }
      );
}

onNoClickEmp(): void {
  this.dialogRef.close();
}

  onRemoveTeam(t: Team) {
    const index = this.data.selTeams.findIndex(i => i.teamID === t.teamID);
    this.data.selTeams.splice(index, 1);
}
  
}
