import { Component, OnInit, Inject } from '@angular/core';
import { Ticket } from '../../../classes/Ticket';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Subprocess } from '../../../classes/Subprocess';
import { Validators, FormBuilder } from '@angular/forms';
import { ProcessService } from '../../core/process/process.service';
import { ParentChildRelationService } from '../../core/parent-child-relation/parent-child-relation.service';

@Component({
  selector: 'app-finish-ticket',
  templateUrl: './finish-ticket.component.html',
  styleUrls: ['./finish-ticket.component.css'],
  providers: [ProcessService, ParentChildRelationService]

})
export class FinishTicketComponent implements OnInit {


  constructor(public dialogRef: MatDialogRef<Ticket>,
    @Inject(MAT_DIALOG_DATA) public ticket: Ticket,
    private fb: FormBuilder,
    private processService: ProcessService,
    private parentChildService: ParentChildRelationService) {
  }


  ticketID: number = undefined;
  description: string = undefined;
  
  allSubprocesses: Subprocess[] = [];
  selectedSubprocess: number = undefined;
  

  finishTicketForm = this.fb.group({
    note: [''],
    subprocessID: ['', Validators.compose([Validators.required, Validators.min(0)])]  })

  error: string = 'Bitte geben Sie einen gültigen Wert ein!';

  ngOnInit() {
    if (this.ticket !== undefined) {
      if (this.ticket.ticketID !== undefined)
        this.ticketID = this.ticket.ticketID;

      if (this.ticket.ticketID !== undefined)
        this.description = this.ticket.description;

      this.finishTicketForm.patchValue({
        'note': this.ticket.note
      });

      this.parentChildService.getParentChildRelations()
        .subscribe(data => console.log(data));
    }

    this.processService
      .getSubprocesses() //TODO get next subprocess
      .subscribe(data => {
        this.allSubprocesses = data;
        if (data !== undefined && data.length > 0)
          this.selectedSubprocess = data[0].subprocessID;
        else
          this.selectedSubprocess = -1;
      });
  }

  onCancelClicked(): void {
    this.dialogRef.close();
  }

  onSaveClicked(): void {
    if (this.finishTicketForm.valid) {
      let result = this.finishTicketForm.value;

      result.ticketID = this.ticketID;
      result.description = this.description;

      this.dialogRef.close(result);
    }
  }

}
