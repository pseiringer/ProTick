import { Component, OnInit, ViewChild } from '@angular/core';

import { ProcessService } from '../core/process/process.service';
import { TeamService } from '../core/team/team.service';
import { ParentChildRelationService } from '../core/parent-child-relation/parent-child-relation.service';

import { Process } from '../../classes/Process';
import { Subprocess } from '../../classes/Subprocess';
import { Team } from '../../classes/Team';
import { FullSubprocess } from '../../classes/FullSubprocess';
import { ParentChildRelation } from '../../classes/ParentChildRelation';

import { CreateProcessComponent } from '../processes/create-process/create-process.component';
import { CreateSubprocessComponent } from '../processes/create-subprocess/create-subprocess.component';

import { MatDialog, MatTable } from '@angular/material';
import { isNullOrUndefined } from 'util';
import { Observable, forkJoin } from 'rxjs';
import { EditChildSubprocessesComponent } from './edit-child-subprocesses/edit-child-subprocesses.component';

@Component({
    selector: 'app-processes',
    templateUrl: './processes.component.html',
    styleUrls: ['./processes.component.css'],
    providers: [ProcessService, ParentChildRelationService, TeamService],
})

export class ProcessesComponent implements OnInit {

    //@ViewChild(MatTable, { static: true, read: MatTable }) table: MatTable<any>;
    //@ViewChild(MatTable, { static: true, read: MatTable }) tableFirst: MatTable<any>;


    @ViewChild('table', { static: true, read: MatTable }) table: MatTable<any>;
    @ViewChild('tableFirst', { static: true, read: MatTable }) tableFirst: MatTable<any>;

    private processes: Process[] = [];
    private subprocesses: Subprocess[] = [];
    private parentChildRelations: ParentChildRelation[] = [];
    private displayedSubprocesses: FullSubprocess[] = [];
    private firstSubprocess: FullSubprocess[] = [];
    private teams: Team[] = [];

    private selectedProcess: number;

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

    fullSubprocess: FullSubprocess = {
        subprocessID: undefined,
        description: undefined,
        teamID: undefined,
        teamName: undefined,
        processID: undefined,
        processName: undefined
    }

    displayedColumns: string[] = ['subprocessID', 'description', 'teamName', 'childProcesses', 'childOptions', 'optionButtons'];

  firstDisplayedColumns: string[] = ['firstSubprocessID', 'firstDescription', 'firstTeamName', 'childProcesses', 'childOptions'];

    serviceWorking: boolean = false;

    constructor(private _processService: ProcessService,
        private _parentChildRelationService: ParentChildRelationService,
        private _teamService: TeamService,
        public dialog: MatDialog) { }

    ngOnInit() {
        this._teamService.getTeams().subscribe(data => { this.teams = data; this.getProcesses(); });
    }

    getProcesses(): void {
        this._processService.getProcesses()
            .subscribe(data => {
                this.processes = data;
                if (this.processes.length > 0) this.selectedProcess = this.processes[0].processID;
                this.getSubprocessesByProcessID(this.selectedProcess);
            });
    }

    getSubprocessesByProcessID(_processID): void {
        console.log('selectionChange');
        this._processService.getSubprocessesByProcessID(_processID)
          .subscribe(data => {
            this.subprocesses = data.sort((a, b) => a.subprocessID - b.subprocessID);
                this.getParentChildRelationsByProcessID(_processID);
            });
    }

    getParentChildRelationsByProcessID(_processID: number): void {
        this.parentChildRelations = [];
        this._parentChildRelationService.getParentChildRelationByProcessID(_processID)
            .subscribe(x => {
                this.parentChildRelations = x;
                this.reloadFullSubprocesses();
                this.serviceWorking = false;
            });
    }

    reloadFullSubprocesses() {
        console.log('reloadFullSubprocesses');
        this.displayedSubprocesses = [];
        this.firstSubprocess = [];

        this.subprocesses.forEach(x => {
            let teamName = '';
            let processName = '';

            this._teamService.getTeam(x.teamID).subscribe(y => {
                teamName = y.abbreviation;

                this._processService.getProcess(x.processID).subscribe(z => {
                    processName = z.description;

                    const fullSubprocess: FullSubprocess = {
                        subprocessID: undefined,
                        description: undefined,
                        teamID: undefined,
                        teamName: undefined,
                        processID: undefined,
                        processName: undefined
                    };

                    fullSubprocess.subprocessID = x.subprocessID;
                    fullSubprocess.description = x.description;
                    fullSubprocess.teamID = x.teamID;
                    fullSubprocess.teamName = teamName;
                    fullSubprocess.processID = x.processID;
                    fullSubprocess.processName = processName;

                    if (this.parentChildRelations.findIndex(a => a.parentID === -1 && a.childID === x.subprocessID) >= 0) {
                        this.firstSubprocess = [fullSubprocess];
                        this.tableFirst.renderRows();
                    } else {
                      this.displayedSubprocesses.push(fullSubprocess);
                      this.table.renderRows();
                      this.displayedSubprocesses.sort((a, b) => a.subprocessID - b.subprocessID);
                    }
                });
            });
        });
    }

    openCreateProcessDialog(): void {
        const dialogRef = this.dialog.open(CreateProcessComponent, {
            data: { description: this.process.description }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');

            if (isNullOrUndefined(result)) return;

            console.log(result);

            this.process.description = result.description;

            this._processService.postProcess(this.process)
                .subscribe(x => { this.getProcesses(); });

            this.process.description = undefined;
        });
    }

    openCreateSubprocessDialog(): void {
        const dialogRef = this.dialog.open(CreateSubprocessComponent, {
            data: { description: this.subprocess.description, teamID: this.subprocess.teamID, processID: this.selectedProcess }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');

            if (isNullOrUndefined(result)) return;

            this.subprocess.description = result.description;
            this.subprocess.teamID = result.teamID;
            this.subprocess.processID = result.processID;

            this._processService.postSubprocess(this.subprocess)
                .subscribe(x => {
                    this._parentChildRelationService.postParentChildRelation({ parentChildRelationID: 0, parentID: x.subprocessID, childID: -1 })
                        .subscribe(x => {
                            this.getSubprocessesByProcessID(this.selectedProcess);
                        });
                });

            this.subprocess.description = undefined;
            this.subprocess.teamID = undefined;
            this.subprocess.processID = undefined;
        });
    }

    attributeChanged(subprocessID: number, attribute: string, value: string): void {
        console.log('attChanged ' + subprocessID + value);
        let changeSubprocess = this.subprocesses.find(x => x.subprocessID === subprocessID);

        if (isNullOrUndefined(changeSubprocess)) return;

        changeSubprocess[attribute] = value;

        this._processService.putSubprocess(changeSubprocess, subprocessID)
            .subscribe(x => this.getSubprocessesByProcessID(this.selectedProcess));
    }

    useAsFirst(subprocessID: number): void {
        if (this.serviceWorking) return;
        this.serviceWorking = true;

        var first = this.parentChildRelations.find(x => x.parentID === -1);

        if (isNullOrUndefined(first)) {
            const newFirst: ParentChildRelation = {
                parentChildRelationID: undefined,
                parentID: undefined,
                childID: undefined
            };

            newFirst.parentID = -1;
            newFirst.childID = subprocessID;

            var children = this.parentChildRelations.filter(x => x.parentID === subprocessID);

            var newChild: ParentChildRelation = null;

            if (children.length <= 0 && this.subprocesses.length > 1) {
                newChild = {
                    parentChildRelationID: undefined,
                    parentID: undefined,
                    childID: undefined
                };

                newChild.parentID = subprocessID;
                newChild.childID = (this.subprocesses.filter(x => x.subprocessID !== subprocessID)[0]).subprocessID;
            }

            var oldParents = this.parentChildRelations.filter(x => x.parentID !== -1 && x.childID === subprocessID);


            // Delete Old Parents (ForkJoin), Post New First, If (newChild != null): Post, Finally: GetParentChildRelationsByProcessID

            const calls = [];
            oldParents.forEach(x => {
                calls.push(this._parentChildRelationService.deleteParentChildRelation(x.parentChildRelationID));
            });

            if (calls.length > 0) {
                forkJoin(calls).subscribe(x => {
                    this._parentChildRelationService.postParentChildRelation(newFirst).subscribe(x => {
                        if (!isNullOrUndefined(newChild)) {
                            this._parentChildRelationService.postParentChildRelation(newChild).subscribe(x => {
                                this.getParentChildRelationsByProcessID(this.selectedProcess);
                            });
                        } else {
                            this.getParentChildRelationsByProcessID(this.selectedProcess);
                        }
                    });
                });
            } else {
                this._parentChildRelationService.postParentChildRelation(newFirst).subscribe(x => {
                    if (!isNullOrUndefined(newChild)) {
                        this._parentChildRelationService.postParentChildRelation(newChild).subscribe(x => {
                            this.getParentChildRelationsByProcessID(this.selectedProcess);
                        });
                    } else {
                        this.getParentChildRelationsByProcessID(this.selectedProcess);
                    }
                });
            }
        } else {
            var previousFirstID = -1;
            var alreadyHasChildren = false;

            previousFirstID = first.childID;
            first.childID = subprocessID;

            const newChild: ParentChildRelation = {
                parentChildRelationID: undefined,
                parentID: undefined,
                childID: undefined
            };

            newChild.parentID = subprocessID;
            newChild.childID = previousFirstID;

            var oldParents = this.parentChildRelations.filter(x => x.parentID !== -1 && x.childID === subprocessID);

            // Delete Old Parents (ForkJoin), Update First, Create New Child, Finally: GetParentChildRelationsByProcessID

            const calls = [];
            oldParents.forEach(x => {
                calls.push(this._parentChildRelationService.deleteParentChildRelation(x.parentChildRelationID));
            });

            if (calls.length > 0) {
                forkJoin(calls).subscribe(x => {
                    this._parentChildRelationService.putParentChildRelation(first.parentChildRelationID, first).subscribe(x => {
                        this._parentChildRelationService.postParentChildRelation(newChild).subscribe(x => {
                            this.getParentChildRelationsByProcessID(this.selectedProcess);
                        });
                    });
                });
            } else {
                this._parentChildRelationService.putParentChildRelation(first.parentChildRelationID, first).subscribe(x => {
                    this._parentChildRelationService.postParentChildRelation(newChild).subscribe(x => {
                        this.getParentChildRelationsByProcessID(this.selectedProcess);
                    });
                });
            }
        }
    }

    getFormatedChildrenOf(fullSubprocess: FullSubprocess): string {
        return this.getChildrenOfSubprocess(fullSubprocess.subprocessID)
            .sort((a, b) => (b.childID > 0) ? a.childID - b.childID : -1)
            .map(x => (x.childID < 0) ? 'Ende' : x.childID)
            .join(', ');
    }

    getChildrenOfSubprocess(subprocessID: number): ParentChildRelation[] {
        return this.parentChildRelations.filter(x => x.parentID === subprocessID);
    }

    openEditChildSubprocessesDialog(fullSubprocess: FullSubprocess): void {
        console.log(fullSubprocess);

        var children = this.getChildrenOfSubprocess(fullSubprocess.subprocessID).map(x => x.childID);

        const dialogRef = this.dialog.open(EditChildSubprocessesComponent, {
            data: {
                subprocess: fullSubprocess,
                children: children,
                allSubprocesses: this.displayedSubprocesses
            }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');

            if (isNullOrUndefined(result)) return;

            var added: number[] = [];
            var removed: number[] = [];

            result.forEach(x => {
                if (x.isChecked) {
                    if (children.indexOf(x.childID) < 0) added.push(x.childID);
                } else {
                    if (children.indexOf(x.childID) >= 0) removed.push(x.childID);
                }
            });

            const calls = [];
            added.forEach(x => {
                calls.push(this._parentChildRelationService.postParentChildRelation({
                    parentChildRelationID: 0,
                    parentID: fullSubprocess.subprocessID,
                    childID: x
                }));
            });

            removed.forEach(x => {
                calls.push(this._parentChildRelationService.deleteParentChildRelation(
                    this.parentChildRelations
                        .find(y => y.parentID === fullSubprocess.subprocessID && y.childID === x)
                        .parentChildRelationID
                ));
            });

            if (calls.length > 0) {
                forkJoin(calls).subscribe(x => {
                    this.getParentChildRelationsByProcessID(this.selectedProcess);
                });

            }
        });
    }
}
