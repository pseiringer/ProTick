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
import { CreateSubprocessComponent } from '../create-subprocess/create-subprocess.component';

import { MatDialog, MatTable } from '@angular/material';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'app-processes',
    templateUrl: './processes.component.html',
    styleUrls: ['./processes.component.css'],
    providers: [ProcessService, ParentChildRelationService, TeamService],
})

export class ProcessesComponent implements OnInit {

    @ViewChild(MatTable, { static: true, read: MatTable }) table: MatTable<any>;

    private processes: Process[] = [];
    private subprocesses: Subprocess[] = [];
    private parentChildRelations: ParentChildRelation[] = [];
    private displayedSubprocesses: FullSubprocess[] = [];
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

    displayedColumns: string[] = ['subprocessID', 'description', 'teamName'];

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
                this.subprocesses = data;
                this.reloadFullSubprocesses();
            });
    }

    reloadFullSubprocesses() {
        console.log('reloadFullSubprocesses');
        this.displayedSubprocesses = [];

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

                    this.displayedSubprocesses.push(fullSubprocess);
                    this.table.renderRows();
                });
            });
        });
    }

    getParentChildRelationsByProcessID(_processID): void {
        this._parentChildRelationService.getParentChildRelationByProcessID(_processID)
            .subscribe(data => this.parentChildRelations = data);
    }

    openCreateProcessDialog(): void {
        const dialogRef = this.dialog.open(CreateProcessComponent, {
            data: { description: this.process.description }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');

            if (isNullOrUndefined(result)) return;

            this.process.description = result;

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
                .subscribe(x => { this.getSubprocessesByProcessID(this._processID); });

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
}
