import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface CreateProcessDialogData {
  description: string;
}

@Component({
  selector: 'app-create-process',
  templateUrl: './create-process.component.html',
  styleUrls: ['./create-process.component.css']
})

export class CreateProcessComponent {

  constructor(public dialogRef: MatDialogRef<CreateProcessComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateProcessDialogData) { }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
