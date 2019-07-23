import { Component, OnInit } from '@angular/core';
import { ProcessService } from '../core/process/process.service';
import { Process } from '../../classes/Process';
import { Subprocess } from '../../classes/Subprocess';
import { CreateProcessComponent } from '../processes/create-process/create-process.component';
import { MatDialog } from '@angular/material';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-processes',
  templateUrl: './processes.component.html',
  styleUrls: ['./processes.component.css'],
  providers: [ProcessService],
})

export class ProcessesComponent implements OnInit {

  private processes: Process[];
  private subprocesses: Subprocess[];

  private _processID: number;

  process: Process = {
    processID: undefined,
    description: undefined
  };

  constructor(private _processService: ProcessService, public dialog: MatDialog) { }

  ngOnInit() {
    this.getProcesses();

    this._processID = 1;
    this.getSubprocessesByProcessID(this._processID);
  }

  getProcesses(): void {
    this._processService.getProcesses()
      .subscribe(data => this.processes = data);
  }

  getSubprocessesByProcessID(_processID): void {
    this._processService.getSubprocessesByProcessID(_processID)
      .subscribe(data => this.subprocesses = data);
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.subprocesses, event.previousIndex, event.currentIndex);
    console.log(this.subprocesses);
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(CreateProcessComponent, {
      data: { description: this.process.description }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.process.description = result;
      this._processService.postProcess(this.process)
        .subscribe();
    });
  }
}
