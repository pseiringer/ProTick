import { Component } from '@angular/core';
import { ProcessService } from '../core/process/process.service';
import { Process } from '../../classes/Process';
import { CreateProcessComponent } from '../create-process/create-process.component';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-processes',
  templateUrl: './processes.component.html',
  styleUrls: ['./processes.component.css'],
  providers: [ProcessService]
})

export class ProcessesComponent {

  process: Process = {
    processID: undefined,
    description: undefined
  };

  constructor(private _processService: ProcessService, public dialog: MatDialog) { }

  openDialog(): void {
    const dialogRef = this.dialog.open(CreateProcessComponent, {
      data: { description: this.process.description }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.process.description = result;
      this._processService.postProcess(this.process);
    });
  }
}
