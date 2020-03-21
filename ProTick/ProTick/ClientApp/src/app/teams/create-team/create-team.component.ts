import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EmployeeService } from '../../core/employee/employee.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Employee } from '../../../classes/Employee';

export interface CreateTeamDialogData {
  teamID: number;
  description: string;
  abbreviation: string;
  employeeID: number;
  selEmps: Employee[],
}

@Component({
  selector: 'app-create-team',
  templateUrl: './create-team.component.html',
  styleUrls: ['./create-team.component.css'],
  providers: [EmployeeService],

})
export class CreateTeamComponent implements OnInit {


  constructor(private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<CreateTeamComponent>,
    private _employeeService: EmployeeService,
    @Inject(MAT_DIALOG_DATA) public data: CreateTeamDialogData) { }

  allEmps: Employee[] = [];
  selEmp: Employee;

  _header: string;
  _buttonText: string;

  teamDataFormGroup: FormGroup;

  ngOnInit() {
    if (this.data.teamID !== undefined) {
      console.log(this.data);
      this._header = "Team bearbeiten";
      this._buttonText = "Bestätigen"
    }
    else {
      this._header = "Team erstellen";
      this._buttonText = "Erstellen";
      this.data.selEmps = [];
    }

    this.getEmps();

    this.teamDataFormGroup = this._formBuilder.group({
      descCtrl: ['', Validators.required],
      abbrCtrl: ['', Validators.required]
    });
  }

  getEmps() {
    this._employeeService.getEmployees()
      .subscribe(data => {
        this.allEmps = data;
        this.data.employeeID = this.allEmps[0].employeeID;
        console.log(this.data.employeeID);
      });
  }

  onAddEmp() {
    console.log(this.data.employeeID);

    this.selEmp = this.allEmps.filter(x => x.employeeID === this.data.employeeID)[0];
    console.log(this.selEmp);

    if (this.data.selEmps.some(e => e.employeeID === this.selEmp.employeeID) == false) {
      this.data.selEmps.push(this.selEmp);
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onRemoveEmp(e: Employee) {
    const index = this.data.selEmps.findIndex(i => i.employeeID === e.employeeID);
    this.data.selEmps.splice(index, 1);
  }
}
