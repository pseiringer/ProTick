import { Component, OnInit } from '@angular/core';
import { ProcessService } from '../core/process/process.service';
import { ParentChildRelationService } from '../core/parent-child-relation/parent-child-relation.service';
import { Process } from '../../classes/Process';
import { Subprocess } from '../../classes/Subprocess';

import { ParentChildRelation } from '../../classes/ParentChildRelation';
import { CreateProcessComponent } from '../create-process/create-process.component';
import { CreateSubprocessComponent } from '../create-subprocess/create-subprocess.component';

import { MatDialog } from '@angular/material';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-processes',
  templateUrl: './processes.component.html',
  styleUrls: ['./processes.component.css'],
  providers: [ProcessService, ParentChildRelationService],
})

export class ProcessesComponent implements OnInit {

  private processes: Process[] = [];
  private subprocesses: Subprocess[] = [];
  private parentChildRelations: ParentChildRelation[] = [];

  private _processID: number;

  process: Process = {
    processID: undefined,
    description: undefined
  };

  subprocess: Subprocess = {
    subprocessID: undefined,
    description: undefined,
    teamID: undefined,
    processID: undefined
  };

  parentChildRelation: ParentChildRelation = {
    parentChildRelationID: undefined,
    parentID: undefined,
    childID: undefined
  }

  constructor(private _processService: ProcessService, private _parentChildRelationService: ParentChildRelationService, public dialog: MatDialog) { }

  ngOnInit() {
    this.getProcesses();
  }

  getProcesses(): void {
    this._processService.getProcesses()
      .subscribe(data => this.processes = data);
  }

  getSubprocessesByProcessID(_processID): void {
    this._processService.getSubprocessesByProcessID(_processID)
      .subscribe(data => this.subprocesses = data);

    console.log(this.subprocesses);
  }

  getParentChildRelationsByProcessID(_processID): void {
    this._parentChildRelationService.getParentChildRelationByProcessID(_processID)
      .subscribe(data => this.parentChildRelations = data);

    console.log(this.parentChildRelations);
  }

  deleteSubprocess(subprocessID: number): void {
    this._processService.deleteSubprocess(subprocessID)
      .subscribe(x => { this.getSubprocessesByProcessID(this._processID); });
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.subprocesses, event.previousIndex, event.currentIndex);
    console.log(this.subprocesses);
    console.log(this.parentChildRelations);
  }

  saveSubprocesses(): void {
    
  }

  openCreateProcessDialog(): void {
    const dialogRef = this.dialog.open(CreateProcessComponent, {
      data: { description: this.process.description }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');

      this.process.description = result;

      this._processService.postProcess(this.process)
        .subscribe(x => { this.getProcesses(); });

      this.process.description = undefined;
    });
  }

  openCreateSubprocessDialog(): void {
    const dialogRef = this.dialog.open(CreateSubprocessComponent, {
      data: { description: this.subprocess.description, teamID: this.subprocess.teamID, processID: this.subprocess.processID }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');

      this.subprocess.description = result.description;
      this.subprocess.teamID = result.teamID;
      this.subprocess.processID = result.processID;

      this._processService.postSubprocess(this.subprocess)
        .subscribe(x => { this.getSubprocessesByProcessID(this._processID); });

      this.subprocess.description = undefined;
      this.subprocess.teamID = undefined;
      this.subprocess.processID = undefined;
    });
  }

  deleteSubprocessWhenDropped(event: CdkDragDrop<string[]>) {
    this.deleteSubprocess(event.container.data[event.previousIndex].subprocessID);
  }
}
