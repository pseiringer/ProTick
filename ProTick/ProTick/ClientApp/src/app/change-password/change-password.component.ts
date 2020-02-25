import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, Validators } from '@angular/forms';

export interface ChangePasswordDialogData {
    currentPassword: string;
    newPassword: string;
    newPasswordConformation: string;
}

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {

    // TODO: neu und alt passwort darf nicht gleich sein
    constructor(public dialogRef: MatDialogRef<ChangePasswordComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ChangePasswordDialogData,
        private fb: FormBuilder) { }

    passwordForm = this.fb.group({
        currentPassword: ['', Validators.required],
        newPassword: ['', Validators.required],
        newPasswordConformation: ['', Validators.required],
    })

    onNoClick(): void {
        this.dialogRef.close();
    }

    onYesClick(): void {
        if (this.passwordForm.valid) this.dialogRef.close(this.data);
    }
}
