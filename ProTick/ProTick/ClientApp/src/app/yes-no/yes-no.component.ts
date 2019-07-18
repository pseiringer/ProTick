import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-yes-no',
  templateUrl: './yes-no.component.html',
  styleUrls: ['./yes-no.component.css']
})
export class YesNoComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: YesNoDialogOptions) { }

  title: string = undefined;
  subheading: string = undefined;
  text: string = undefined;
  no: string = undefined;
  yes: string = undefined;

  ngOnInit() {
    if (this.data != null) {
      if (this.data.title !== undefined)
        this.title = this.data.title;
      if (this.data.subheading !== undefined)
        this.subheading = this.data.subheading;
      if (this.data.text !== undefined)
        this.text = this.data.text;
      if (this.data.no !== undefined)
        this.no = this.data.no;
      if (this.data.yes !== undefined)
        this.yes = this.data.yes;
    }
  }

  titleExists(): boolean {
    return this.title !== undefined && this.title !== '';
  }
  subheadingExists(): boolean {
    return this.subheading !== undefined && this.subheading !== '';
  }
  textExists(): boolean {
    return this.text !== undefined && this.yes !== '';
  }
  noExists(): boolean {
    return this.no !== undefined && this.no !== '';
  }
  yesExists(): boolean {
    return this.yes !== undefined && this.yes !== '';
  }
}


export class YesNoDialogOptions {
  title: string;
  subheading: string;
  text: string;
  no: string;
  yes: string;
}
