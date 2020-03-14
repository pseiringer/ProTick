import { Component, OnInit, ViewChild } from '@angular/core';

import { MatDialog, MatTable } from '@angular/material';
import { isNullOrUndefined } from 'util';
import { forkJoin } from 'rxjs';
import { AuthGuard } from '../../classes/Authentication/AuthGuard';

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
import { EditChildSubprocessesComponent } from './edit-child-subprocesses/edit-child-subprocesses.component';
import { EditProcessComponent } from './edit-process/edit-process.component';
import { YesNoComponent } from '../yes-no/yes-no.component';

@Component({
    selector: 'app-processes',
    templateUrl: './processes.component.html',
    styleUrls: ['./processes.component.css'],
    providers: [
        ProcessService,
        ParentChildRelationService,
        TeamService,
    ],
})

export class ProcessesComponent implements OnInit {
    @ViewChild('table', { static: true, read: MatTable }) table: MatTable<any>;
    @ViewChild('tableFirst', { static: true, read: MatTable }) tableFirst: MatTable<any>;

    private processes: Process[] = [];
    private subprocesses: Subprocess[] = [];
    private displayedSubprocesses: FullSubprocess[] = [];
    private firstSubprocess: FullSubprocess[] = [];
    private parentChildRelations: ParentChildRelation[] = [];
    private teams: Team[] = [];

    private selectedProcessID: number;

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

    displayedColumns: string[] = ['subprocessID', 'description', 'teamName', 'childProcesses', 'optionButtons', 'childOptions', 'deleteButton'];

    firstDisplayedColumns: string[] = ['firstSubprocessID', 'firstDescription', 'firstTeamName', 'childProcesses', 'childOptions'];

    serviceWorking: boolean = false;

    constructor(
        private authGuard: AuthGuard,
        private teamService: TeamService,
        private processService: ProcessService,
        private parentChildRelationService: ParentChildRelationService,
        public dialog: MatDialog,
    ) { }

    ngOnInit() {
        this.teamService.getTeams()
            .subscribe(teams => {
                this.teams = teams;
                this.getProcesses();
            });
    }

    getProcesses(): void {
        this.processService.getProcesses()
            .subscribe(processes => {
                this.processes = processes;
                if (this.selectedProcessID === undefined) this.selectedProcessID = this.processes[0].processID;
                this.getSubprocessesByProcessID(this.selectedProcessID);
            });
    }

    getSubprocessesByProcessID(selectedProcessID: number): void {
        this.processService.getSubprocessesByProcessID(selectedProcessID)
            .subscribe(subprocesses => {
                this.subprocesses = subprocesses;
                this.getParentChildRelationsByProcessID(selectedProcessID);
            });
    }

    getParentChildRelationsByProcessID(selectedProcessID: number): void {
        this.parentChildRelations = [];
        this.parentChildRelationService.getParentChildRelationByProcessID(selectedProcessID)
            .subscribe(parentChildRelations => {
                this.parentChildRelations = parentChildRelations;
                this.reloadFullSubprocesses();
                this.serviceWorking = false;
            });
    }

    reloadFullSubprocesses() {
        this.displayedSubprocesses = [];
        this.firstSubprocess = [];
        this.subprocesses
            .forEach(subprocess => {
                let teamName = '';
                let processName = '';

                this.teamService.getTeam(subprocess.teamID)
                    .subscribe(team => {
                        teamName = team.abbreviation;

                        this.processService.getProcess(subprocess.processID)
                            .subscribe(process => {
                                processName = process.description;

                                const fullSubprocess: FullSubprocess = {
                                    subprocessID: undefined,
                                    description: undefined,
                                    teamID: undefined,
                                    teamName: undefined,
                                    processID: undefined,
                                    processName: undefined
                                };

                                fullSubprocess.subprocessID = subprocess.subprocessID;
                                fullSubprocess.description = subprocess.description;
                                fullSubprocess.teamID = subprocess.teamID;
                                fullSubprocess.teamName = teamName;
                                fullSubprocess.processID = subprocess.processID;
                                fullSubprocess.processName = processName;

                                if (this.parentChildRelations.findIndex(a => a.parentID === -1 && a.childID === subprocess.subprocessID) >= 0) {
                                    this.firstSubprocess = [fullSubprocess];
                                    this.tableFirst.renderRows();
                                } else {
                                    this.displayedSubprocesses.push(fullSubprocess);
                                    this.table.renderRows();
                                }
                            });
                    });
            });
    }

    openCreateProcessDialog(): void {
        const dialogRef = this.dialog.open(CreateProcessComponent, {
            data: { description: this.process.description }
        });

        dialogRef.afterClosed()
            .subscribe(result => {
                if (isNullOrUndefined(result)) return;

                this.process.description = result.description;
                this.processService.postProcess(this.process)
                    .subscribe(process => {
                        this.selectedProcessID = process.processID;
                        this.getProcesses();
                    });
                this.process.description = undefined;
            });
    }

    openEditProcessDialog(): void {
        this.processService.getProcess(this.selectedProcessID)
            .subscribe(process => {
                const dialogRef = this.dialog.open(EditProcessComponent, {
                    data: { description: process.description }
                });

                dialogRef.afterClosed()
                    .subscribe(result => {
                        if (isNullOrUndefined(result)) return;

                        process.description = result.description;
                        this.processService.putProcess(process, this.selectedProcessID)
                            .subscribe(process => {
                                this.getProcesses();
                                this.selectedProcessID = process.processID;
                            });
                    });
            });
    }

    openDeleteProcessDialog(): void {
        this.processService.getProcess(this.selectedProcessID)
            .subscribe(process => {
                if (this.authGuard.canActivate()) {
                    const dialogRef = this.dialog.open(YesNoComponent, {
                        data: {
                            title: "Prozess löschen",
                            text: "Möchten Sie wirklich den Prozess \"" + process.description + "\" und alle dazugehörigen Kindprozesse sowie Beziehungen und Tickets löschen?",
                            no: "Nein",
                            yes: "Ja"
                        }
                    });

                    dialogRef.afterClosed()
                        .subscribe(result => {
                            if (result === true) {
                                this.processService.deleteProcess(process.processID)
                                    .subscribe(process => {
                                        this.selectedProcessID = undefined;
                                        this.getProcesses()
                                    });
                            }
                        });
                }
            });
    }

    // TODO: ersten Subprozess immer als Startprozess festlegen
    openCreateSubprocessDialog(): void {
        const dialogRef = this.dialog.open(CreateSubprocessComponent, {
            data: {
                description: this.subprocess.description,
                teamID: this.subprocess.teamID,
                processID: this.selectedProcessID,
            }
        });

        dialogRef.afterClosed()
            .subscribe(result => {
                if (isNullOrUndefined(result)) return;

                this.subprocess.description = result.description;
                this.subprocess.teamID = result.teamID;
                this.subprocess.processID = result.processID;

                this.processService.postSubprocess(this.subprocess)
                    .subscribe(subprocess => {
                        this.selectedProcessID = subprocess.processID;

                        this.parentChildRelationService.postParentChildRelation({
                            parentChildRelationID: 0,
                            parentID: subprocess.subprocessID,
                            childID: -1,
                        })
                            .subscribe(x => {
                                this.getSubprocessesByProcessID(this.selectedProcessID);
                            });
                    });

                this.subprocess.description = undefined;
                this.subprocess.teamID = undefined;
                this.subprocess.processID = undefined;
            });
    }

    openDeleteSubprocessDialog(subprocessID: number): void {
        this.processService.getSubprocessById(subprocessID)
            .subscribe(subprocess => {
                if (this.authGuard.canActivate()) {
                    const dialogRef = this.dialog.open(YesNoComponent, {
                        data: {
                            title: "Subprozess löschen",
                            text: "Möchten Sie wirklich den Subprozess " + subprocess.subprocessID + " und alle dazugehörigen Beziehungen und Tickets löschen?",
                            no: "Nein",
                            yes: "Ja"
                        }
                    });

                    dialogRef.afterClosed()
                        .subscribe(result => {
                            if (result === true) {
                                this.processService.deleteSubprocess(subprocess.subprocessID)
                                    .subscribe(subprocess => this.getProcesses());
                            }
                        });
                }
            });
    }

    attributeChanged(subprocessID: number, attribute: string, value: string): void {
        let changeSubprocess = this.subprocesses.find(x => x.subprocessID === subprocessID);

        if (isNullOrUndefined(changeSubprocess)) return;

        changeSubprocess[attribute] = value;

        this.processService.putSubprocess(changeSubprocess, subprocessID)
            .subscribe(x => this.getSubprocessesByProcessID(this.selectedProcessID));
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

            const calls = [];

            oldParents.forEach(x => {
                calls.push(this.parentChildRelationService.deleteParentChildRelation(x.parentChildRelationID));
            });

            if (calls.length > 0) {
                forkJoin(calls)
                    .subscribe(result => {
                        this.parentChildRelationService.postParentChildRelation(newFirst)
                            .subscribe(parentChildRelation => {
                                if (!isNullOrUndefined(newChild)) {
                                    this.parentChildRelationService.postParentChildRelation(newChild)
                                        .subscribe(parentChildRelation => this.getParentChildRelationsByProcessID(this.selectedProcessID));
                                } else {
                                    this.getParentChildRelationsByProcessID(this.selectedProcessID);
                                }
                            });
                    });
            } else {
                this.parentChildRelationService.postParentChildRelation(newFirst)
                    .subscribe(parentChildRelation => {
                        if (!isNullOrUndefined(newChild)) {
                            this.parentChildRelationService.postParentChildRelation(newChild)
                                .subscribe(parentChildRelation => this.getParentChildRelationsByProcessID(this.selectedProcessID));
                        } else {
                            this.getParentChildRelationsByProcessID(this.selectedProcessID);
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

            const calls = [];
            oldParents.forEach(x => {
                calls.push(this.parentChildRelationService.deleteParentChildRelation(x.parentChildRelationID));
            });

            if (calls.length > 0) {
                forkJoin(calls)
                    .subscribe(result => {
                        this.parentChildRelationService.putParentChildRelation(first.parentChildRelationID, first)
                            .subscribe(parentChildRelation => {
                                this.parentChildRelationService.postParentChildRelation(newChild)
                                    .subscribe(parentChildRelation => this.getParentChildRelationsByProcessID(this.selectedProcessID));
                            });
                    });
            } else {
                this.parentChildRelationService.putParentChildRelation(first.parentChildRelationID, first)
                    .subscribe(parentChildRelation => {
                        this.parentChildRelationService.postParentChildRelation(newChild)
                            .subscribe(parentChildRelation => this.getParentChildRelationsByProcessID(this.selectedProcessID));
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
        var children = this.getChildrenOfSubprocess(fullSubprocess.subprocessID).map(x => x.childID);

        const dialogRef = this.dialog.open(EditChildSubprocessesComponent, {
            height: '600px',
            width: '400px',
            data: {
                subprocess: fullSubprocess,
                children: children,
                allSubprocesses: this.displayedSubprocesses,
            }
        });

        dialogRef.afterClosed()
            .subscribe(result => {
                if (isNullOrUndefined(result)) return;

                var added: number[] = [];
                var removed: number[] = [];

                result.forEach(x => {
                    if (x.isChecked) {
                        if (children.indexOf(x.childID) < 0) added.push(x.childID);
                    }
                    else {
                        if (children.indexOf(x.childID) >= 0) removed.push(x.childID);
                    }
                });

                const calls = [];
                added.forEach(x => {
                    calls.push(this.parentChildRelationService.postParentChildRelation({
                        parentChildRelationID: 0,
                        parentID: fullSubprocess.subprocessID,
                        childID: x,
                    }));
                });

                removed.forEach(x => {
                    calls.push(this.parentChildRelationService.deleteParentChildRelation(
                        this.parentChildRelations
                            .find(y => y.parentID === fullSubprocess.subprocessID && y.childID === x)
                            .parentChildRelationID
                    ));
                });

                if (calls.length > 0) {
                    forkJoin(calls).subscribe(x => {
                        this.getParentChildRelationsByProcessID(this.selectedProcessID);
                    });
                }
            });
    }
}
