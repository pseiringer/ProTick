import { Component, OnInit } from '@angular/core';
import { ProcessDataService } from '../core/process/process-data.service';
import { CreateTicketComponent } from '../create-ticket/create-ticket.component';
//import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-processes',
  templateUrl: './processes.component.html',
  styleUrls: ['./processes.component.css'],
  providers: [ProcessDataService]
})
export class ProcessesComponent implements OnInit {

  allProcesses: any = [];

  constructor(private _processDataService: ProcessDataService/*, private dialog: MatDialog*/) { }

  ngOnInit() {
    this._processDataService.getProcesses()
      .subscribe(data => this.allProcesses = data);
  }

  //private openDialog() {
  //  let dialogRef = this.dialog.open(CreateTicketComponent, {
  //    height: '400px',
  //    width: '600px',
  //  });
  //}
}
