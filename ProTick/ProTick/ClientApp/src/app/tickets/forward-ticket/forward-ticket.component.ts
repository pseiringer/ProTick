import { Component, OnInit, Inject } from '@angular/core';
import { Ticket } from '../../../classes/Ticket';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Subprocess } from '../../../classes/Subprocess';
import { Validators, FormBuilder } from '@angular/forms';
import { ProcessService } from '../../core/process/process.service';
import { ParentChildRelationService } from '../../core/parent-child-relation/parent-child-relation.service';
import { StaticDatabaseObjectsService } from '../../core/static-database-objects/static-database-objects.service';

@Component({
    selector: 'app-forward-ticket',
    templateUrl: './forward-ticket.component.html',
    styleUrls: ['./forward-ticket.component.css'],
    providers: [ProcessService, ParentChildRelationService, StaticDatabaseObjectsService]
})

export class ForwardTicketComponent implements OnInit {

    constructor(public dialogRef: MatDialogRef<Ticket>,
        @Inject(MAT_DIALOG_DATA) public options: ForwardTicketDialogOptions,
        private fb: FormBuilder,
        private processService: ProcessService,
        private parentChildService: ParentChildRelationService,
        private staticDatabaseObjectService: StaticDatabaseObjectsService) {
    }

    functionality: Functionality = undefined;
    isDone: boolean = false;

    ticketID: number = undefined;
    description: string = undefined;

    allSubprocesses: Subprocess[] = [];
    selectedSubprocess: number = undefined;


    forwardTicketForm = this.fb.group({
        note: [''],
        subprocessID: ['', Validators.compose([Validators.required, Validators.min(0)])]
    })

    error: string = 'Ungültige Eingabe!';

    ngOnInit() {
        let ticket = this.options.ticket;

        console.log(ticket);

        if (ticket !== undefined) {
            if (ticket.ticketID !== undefined)
                this.ticketID = ticket.ticketID;

            if (ticket.ticketID !== undefined)
                this.description = ticket.description;

            this.forwardTicketForm.patchValue({
                'note': ticket.note
            });

            if (this.options.functionality !== undefined) {
                this.functionality = this.options.functionality;

                if (this.functionality === Functionality.Begin) {
                    //this.forwardTicketForm.controls['note'].disable();
                    this.forwardTicketForm.controls['subprocessID'].disable();

                    this.processService
                        .getSubprocessById(ticket.subprocessID) //TODO get next subprocess
                        .subscribe(data => {
                            console.log(data);
                            this.allSubprocesses = [data];
                            if (data !== undefined && data.subprocessID !== undefined)
                                this.selectedSubprocess = data.subprocessID;
                            else
                                this.selectedSubprocess = -1;
                        });
                }
                else if (this.functionality === Functionality.Finish) {

                    this.parentChildService
                        .getChildrenBySubprocessID(ticket.subprocessID) //TODO get next subprocess
                        .subscribe(data => {
                            console.log(data);
                            this.allSubprocesses = [];
                            if (data !== undefined && data.length > 0)
                                if (data[0] !== null) {
                                    this.allSubprocesses = data;
                                    this.selectedSubprocess = data[0].subprocessID;
                                }
                                else {
                                    this.isDone = true;
                                    this.selectedSubprocess = 0;
                                }
                            else
                                this.selectedSubprocess = -1;
                        });
                }
            }
            else {
                this.forwardTicketForm.disable();
            }
        }
        else {
            this.forwardTicketForm.disable();
        }
    }

    onCancelClicked(): void {
        this.dialogRef.close();
    }

    onSaveClicked(): void {
        if (this.forwardTicketForm.valid) {
            let result = this.forwardTicketForm.value;

            result.ticketID = this.ticketID;
            result.description = this.description;

            const isBegin = this.functionality === Functionality.Begin;
            const isFinish = this.functionality === Functionality.Finish;

            if (isBegin) {
                result.stateID = this.staticDatabaseObjectService.getStates().InProgress;
                //result.subprocessID = this.forwardTicketForm.controls['subprocessID'].value;
            }
            else if (isFinish) {
                result.stateID = this.staticDatabaseObjectService.getStates().Open;
            }

            if (this.isDone) {
                result.subprocessID = -1;
                result.stateID = this.staticDatabaseObjectService.getStates().Finished;
            }

            this.dialogRef.close(result);
        }
    }
}

export interface ForwardTicketDialogOptions {
    functionality: Functionality,
    ticket: Ticket
}

export enum Functionality {
    Begin = "beginnen",
    Finish = "abschließen",
}
