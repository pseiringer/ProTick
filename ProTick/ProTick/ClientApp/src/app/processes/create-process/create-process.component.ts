import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, Validators } from '@angular/forms';

export interface CreateProcessDialogData {
    description: string
}

@Component({
    selector: 'app-create-process',
    templateUrl: './create-process.component.html',
    styleUrls: ['./create-process.component.css']
})

export class CreateProcessComponent {

    constructor(public dialogRef: MatDialogRef<CreateProcessComponent>,
        @Inject(MAT_DIALOG_DATA) public data: CreateProcessDialogData,
        private fb: FormBuilder) { }

    processForm = this.fb.group({
        description: ['', Validators.required]
    })

    onNoClick(): void {
        this.dialogRef.close();
    }

    onYesClick(): void {
        if (this.processForm.valid) this.dialogRef.close(this.data);
    }
}
