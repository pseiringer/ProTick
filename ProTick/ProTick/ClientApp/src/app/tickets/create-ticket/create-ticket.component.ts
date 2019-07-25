import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Ticket } from '../../../classes/Ticket';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Subprocess } from '../../../classes/Subprocess';
import { State } from '../../../classes/State';
import { Process } from '../../../classes/Process';
import { ProcessService } from '../../core/process/process.service';
import { StateService } from '../../core/state/state.service';


@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrls: ['./create-ticket.component.css'],
  providers: [ProcessService, StateService]
})

export class CreateTicketComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<Ticket>,
    @Inject(MAT_DIALOG_DATA) public data: CreateTicketDialogOptions,
    private fb: FormBuilder,
    private processService: ProcessService,
    private stateService: StateService) {
  }

  ticketID: number = undefined;

  isEdit: boolean = false;

  allProcesses: Process[] = [];
  selectedProcess: number = undefined;
  allSubprocesses: Subprocess[] = [];
  selectedSubprocess: number = undefined;
  allStates: State[] = [];
  selectedState: number = undefined;

  ticketForm = this.fb.group({
    description: ['', Validators.required],
    note: [''],
    processID: ['', Validators.compose([Validators.required, Validators.min(0)])],
    subprocessID: [{ value: '', disabled: !this.isEdit }, Validators.compose([Validators.required, Validators.min(0)])],
    stateID: [{ value: '', disabled: !this.isEdit }, Validators.compose([Validators.required, Validators.min(0)])],
  })

  error: string = 'Bitte geben Sie einen gültigen Wert ein!';  

  ngOnInit() {
    if (this.data.ticket !== undefined) {
      if (this.data.ticket.ticketID !== undefined)
        this.ticketID = this.data.ticket.ticketID;

      this.ticketForm.patchValue({
        'description': this.data.ticket.description,
        'note': this.data.ticket.note
      });
    }

    if (this.data.isEdit !== undefined) {
      this.isEdit = this.data.isEdit;
      if (this.isEdit === true) {
        this.ticketForm.controls['subprocessID'].enable();
        this.ticketForm.controls['stateID'].enable();

        if (this.data.ticket.subprocessID !== -1)
          this.selectedSubprocess = this.data.ticket.subprocessID;

        this.selectedSubprocess = this.data.ticket.subprocessID;
      }
    }
    

    this.processService.getProcessesWithSubprocess(true).subscribe(data => {
      this.allProcesses = data;
      if (data !== undefined && data.length > 0)
        this.selectedProcess = data[0].processID;
      else
        this.selectedProcess = undefined;
      this.reloadSubprocesses();
    });

    this.stateService.getStates().subscribe(data => {
      this.allStates = data;
      if (data !== undefined && data.length > 0) 
        this.selectedState = (data.sort((x, y) => x.stateID - y.stateID))[0].stateID;
      else 
        this.selectedState = undefined;
      
    });
  }

  reloadSubprocesses() {
    this.processService
      .getSubprocessesByProcessID(this.selectedProcess)
      .subscribe(data => {
        this.allSubprocesses = data;
        if (data !== undefined && data.length > 0) 
          this.selectedSubprocess = data[0].subprocessID;
        else
          this.selectedSubprocess = -1;
      })
  }

  onCancelClicked(): void {
    this.dialogRef.close();
  }

  onSaveClicked(): void {
    if (this.ticketForm.valid) {
      let result = this.ticketForm.value;

      if (this.ticketID !== undefined) result.ticketID = this.ticketID;

      if (!this.isEdit) {
        result.subprocessID = this.selectedSubprocess;
        result.stateID = this.selectedState;
      }

      this.dialogRef.close(result);
    }
  }

}

export interface CreateTicketDialogOptions {
  isEdit: boolean,
  ticket: Ticket
}
