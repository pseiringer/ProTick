import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { TeamService } from '../../core/team/team.service';
import { Team } from '../../../classes/Team';
import * as moment from 'moment';
import { MatDialogRef, MAT_DIALOG_DATA, MatListModule, MatStepperModule } from '@angular/material';



export interface CreateEmployeeDialogData {
  employeeID: number,
  firstName: string,
  lastName: string,
  dateOfBirth: string,
  hireDate: string,
  phoneNumber: string,
  email: string,
  username: string,
  password: string,
  addressID: number,
  teamID: number,
  selTeams: Team[],
  street: string,
  streetNumber: string,
  postalCode: string,
  city: string,
  country: string
}

@Component({
  selector: 'app-create-employee',
  templateUrl: './create-employee.component.html',
  styleUrls: ['./create-employee.component.css'],
  providers: [TeamService],
})
export class CreateEmployeeComponent implements OnInit {

  constructor(private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<CreateEmployeeComponent>,
    private _teamService: TeamService,
    @Inject(MAT_DIALOG_DATA)
    public data: CreateEmployeeDialogData) { }

  isLinear = false;
  personalFormGroup: FormGroup;
  addressFormGroup: FormGroup;
  teamFormGroup: FormGroup;

  hireDate = new FormControl(new Date());
  birthDate = new FormControl(new Date());

  allTeams: any = [];
  selTeam: Team;

  error: string = 'Feld darf nicht leer sein!';

  _header: string;
  _buttonText: string;

  ngOnInit() {
    

    if (this.data.employeeID !== undefined) {
      this._header = "Mitarbeiter bearbeiten";
      this._buttonText = "Ändern"
      this.birthDate = new FormControl(new Date(moment(this.data.dateOfBirth, 'DD.MM.YYYY').format('MM.DD.YYYY')));
      this.hireDate = new FormControl(new Date(moment(this.data.hireDate, 'DD.MM.YYYY').format('MM.DD.YYYY')));
    }
    else {
      this._header = "Neuer Mitarbeiter";
      this._buttonText = "Erstellen";
      this.data.selTeams = [];
    }

    this.getTeams();
    const d = new Date();

    this.personalFormGroup = this._formBuilder.group({
      firstNameCtrl: ['', Validators.required],
      lastNameCtrl: ['', Validators.required],
      phoneNumberCtrl: ['', Validators.required],
      emailCtrl: ['', Validators.email],
      hireDateCtrl: ['', Validators.compose([Validators.minLength(8), Validators.maxLength(10)])],
      birthDateCtrl: ['', Validators.compose([Validators.minLength(8), Validators.maxLength(10)])]
    });
    this.addressFormGroup = this._formBuilder.group({
      streetCtrl: ['', Validators.required],
      streetNumberCtrl: ['', Validators.required],
      postalCodeCtrl: ['', Validators.required],
      cityCtrl: ['', Validators.required],
      countryCtrl: ['', Validators.required]
    });
    this.teamFormGroup = this._formBuilder.group(
      {

      });


    //this.personalFormGroup.patchValue({ hireDateControl: new Date() });
    //this.personalFormGroup.controls['hireDateCtrl'].patchValue({ year: d.getFullYear(), month: d.getMonth(), day: d.getDate() });
  }


  getTeams() {
    this._teamService.getTeams()
      .subscribe(data => {
        this.allTeams = data;
        this.data.teamID = this.allTeams[0].teamID;
      });
  }

  onAddTeam() {
    console.log(this.data.teamID);
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
