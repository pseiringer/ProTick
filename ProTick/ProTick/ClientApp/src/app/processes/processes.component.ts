import { Component, OnInit } from '@angular/core';
import { ProcessDataService } from '../core/process/process-data.service';

@Component({
  selector: 'app-processes',
  templateUrl: './processes.component.html',
  styleUrls: ['./processes.component.css'],
  providers: [ProcessDataService]
})
export class ProcessesComponent implements OnInit {

  allProcesses: any = [];

  constructor(private _processDataService: ProcessDataService) { }

  ngOnInit() {
    this._processDataService.getProcesses()
      .subscribe(data => this.allProcesses = data);
  }
}
