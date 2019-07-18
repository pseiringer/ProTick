import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Ticket } from '../../../classes/Ticket';
import { FormGroup, FormControl } from '@angular/forms';


@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrls: ['./create-ticket.component.css']
})

export class CreateTicketComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<Ticket>,
    @Inject(MAT_DIALOG_DATA) public data: Ticket) {
  }

  ticketID: number = undefined;

  ticketForm = new FormGroup({
    description: new FormControl(''),
    stateID: new FormControl(''),
    subprocessID: new FormControl(''),
  });

  error: string = 'Bitte geben Sie einen gültigen Wert ein!';
    

  ngOnInit() {
    if (this.data.ticketID !== undefined)
      this.ticketID = this.data.ticketID;

    this.ticketForm.patchValue({
      'description': this.data.description,
      'stateID': this.data.stateID,
      'subprocessID': this.data.subprocessID,
    });
  }
    

  onCancelClicked(): void {
    this.dialogRef.close();
  }

  onSaveClicked(): void {
    if (this.ticketForm.valid) {
      let result = this.ticketForm.value;
      if (this.ticketID !== undefined) result.ticketID = this.ticketID;
      this.dialogRef.close(result);
    }
  }
}
