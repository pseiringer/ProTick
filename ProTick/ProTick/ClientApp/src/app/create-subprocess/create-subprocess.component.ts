import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TeamService } from '../core/team/team.service';
import { ProcessService } from '../core/process/process.service';
import { Team } from '../../classes/Team';
import { Process } from '../../classes/Process';
import { FormBuilder, Validators } from '@angular/forms';

export interface CreateSubprocessDialogData {
    description: string,
    teamID: number,
    processID: number
}

@Component({
    selector: 'app-create-subprocess',
    templateUrl: './create-subprocess.component.html',
    styleUrls: ['./create-subprocess.component.css'],
    providers: [TeamService, ProcessService]
})
export class CreateSubprocessComponent implements OnInit {

    teams: Team[];
    processes: Process[];

    constructor(public dialogRef: MatDialogRef<CreateSubprocessComponent>,
        @Inject(MAT_DIALOG_DATA) public data: CreateSubprocessDialogData,
        private _teamService: TeamService,
        private _processService: ProcessService,
        private fb: FormBuilder) { }

    subprocessForm = this.fb.group({
        description: ['', Validators.required],
        teamID: ['', Validators.compose([Validators.required, Validators.min(0)])],
        processID: ['', Validators.compose([Validators.required, Validators.min(0)])]
    })

    ngOnInit() {
        this.getTeams();
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    onYesClick(): void {
        if (this.subprocessForm.valid) this.dialogRef.close(this.data);
    }

    getTeams() {
        this._teamService.getTeams()
            .subscribe(data => { this.teams = data; this.getProcesses(); });
    }

    getProcesses() {
        this._processService.getProcesses()
            .subscribe(data => { this.processes = data; });
    }
}
