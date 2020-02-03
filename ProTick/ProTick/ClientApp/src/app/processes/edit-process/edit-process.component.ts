import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, Validators } from '@angular/forms';

export interface EditProcessDialogData {
    description: string
}

@Component({
  selector: 'app-edit-process',
  templateUrl: './edit-process.component.html',
  styleUrls: ['./edit-process.component.css']
})
export class EditProcessComponent {

    constructor(public dialogRef: MatDialogRef<EditProcessComponent>,
        @Inject(MAT_DIALOG_DATA) public data: EditProcessDialogData,
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
