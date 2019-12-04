import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Subprocess } from '../../../classes/Subprocess';

export interface EditChildSubprocessesData {
    subprocess: Subprocess,
    children: number[],
    allSubprocesses: Subprocess[]
}

interface DisplayChildSubprocess {
    childID: number,
    description: string,
    isChecked: boolean
}

@Component({
    selector: 'app-edit-child-subprocesses',
    templateUrl: './edit-child-subprocesses.component.html',
    styleUrls: ['./edit-child-subprocesses.component.css']
})
export class EditChildSubprocessesComponent implements OnInit {

    dataSource: DisplayChildSubprocess[] = [];

    constructor(public dialogRef: MatDialogRef<EditChildSubprocessesComponent>,
        @Inject(MAT_DIALOG_DATA) public data: EditChildSubprocessesData) { }

    displayedColumns: string[] = ['childID', 'description', 'checkbox'];

    //TODO: isChecked is always false
    ngOnInit(): void {
        this.dataSource = this.data.allSubprocesses.map(x => {
            return {
                childID: x.subprocessID,
                description: x.description,
                isChecked: this.data.children.indexOf(x.subprocessID) > 0
            }
        });
        console.log(this.dataSource[0].isChecked);
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    //TODO: check if valid
    onYesClick(): void {
        this.dialogRef.close(this.data);
    }
}
