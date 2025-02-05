import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { TeamService } from '../../core/team/team.service';
import { Team } from '../../../classes/Team';
import * as moment from 'moment';
import { MatDialogRef, MAT_DIALOG_DATA, MatListModule, MatStepperModule } from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { RoleService } from '../../core/role/role.service';

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
    roleID: number,
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
    providers: [TeamService, RoleService],
})
export class CreateEmployeeComponent implements OnInit {

    constructor(private _formBuilder: FormBuilder,
        public dialogRef: MatDialogRef<CreateEmployeeComponent>,
        private _teamService: TeamService,
        private _roleService: RoleService,
        @Inject(MAT_DIALOG_DATA)
        public data: CreateEmployeeDialogData) { }

    isLinear = false;
    personalFormGroup: FormGroup;
    addressFormGroup: FormGroup;
    teamFormGroup: FormGroup;

    hireDate = new FormControl(new Date());
    birthDate = new FormControl(new Date());

    allTeams: Team[] = [];
    allRoles: any = [];
    selTeam: Team;

    error: string = 'Feld darf nicht leer sein!';

    _header: string;
    _buttonText: string;

    ngOnInit() {


        if (this.data.employeeID !== undefined) {
            this._header = "Mitarbeiter bearbeiten";
            this._buttonText = "Bestätigen"
            this.birthDate = new FormControl(new Date(moment(this.data.dateOfBirth, 'DD.MM.YYYY').format('MM.DD.YYYY')));
            this.hireDate = new FormControl(new Date(moment(this.data.hireDate, 'DD.MM.YYYY').format('MM.DD.YYYY')));
            this.data.hireDate = this.hireDate.value;
            this.data.dateOfBirth = this.birthDate.value;
        }
        else {
            this._header = "Mitarbeiter erstellen";
            this._buttonText = "Erstellen";
            this.data.selTeams = [];
            this.data.hireDate = this.hireDate.value;
            this.data.dateOfBirth = this.birthDate.value;
        }

        this.getTeams();
        this.getRoles();
        const d = new Date();

        this.personalFormGroup = this._formBuilder.group({
            firstNameCtrl: ['', Validators.required],
            lastNameCtrl: ['', Validators.required],
            phoneNumberCtrl: ['', Validators.required],
            emailCtrl: ['', Validators.compose([Validators.email, Validators.required])],
            birthDateCtrl: [''],
            hireDateCtrl: ['', Validators.compose([Validators.minLength(8), Validators.maxLength(10)])]
        });
        this.addressFormGroup = this._formBuilder.group({
            streetCtrl: [''],
            streetNumberCtrl: [''],
            postalCodeCtrl: [''],
            cityCtrl: [''],
            countryCtrl: ['']
        });
        this.teamFormGroup = this._formBuilder.group(
            {

            });

    }

    getRoles() {
        this._roleService.getRoles()
            .subscribe(data => {
                this.allRoles = data;
                this.data.roleID = this.allRoles[0].roleID;
            })
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
        this.selTeam = this.allTeams.filter(x => x.teamID === this.data.teamID)[0];

        if (this.data.selTeams.some(e => e.teamID === this.selTeam.teamID) == false) {
            this.data.selTeams.push(this.selTeam);
        }
    }

    onNoClickEmp(): void {
        this.dialogRef.close();
    }

    onRemoveTeam(t: Team) {
        const index = this.data.selTeams.findIndex(i => i.teamID === t.teamID);
        this.data.selTeams.splice(index, 1);
    }

}
