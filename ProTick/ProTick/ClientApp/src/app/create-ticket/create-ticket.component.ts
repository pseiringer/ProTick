import { Component, OnInit } from '@angular/core';
import { Process } from '../../classes/Process';
import { ProcessDataService } from '../core/process/process-data.service';

@Component({
  selector: 'app-create-ticket',
  templateUrl: './create-ticket.component.html',
  styleUrls: ['./create-ticket.component.css'],
  providers: [ProcessDataService]
})

export class CreateTicketComponent implements OnInit {
  allProcesses: any = [];

  process: Process = {
    processID: null,
    description: null
  };

  constructor(private _processDataService: ProcessDataService) { }

  ngOnInit() { }

  getProcesses() {
    this._processDataService.getProcesses()
      .subscribe(data => this.allProcesses = data);

    console.log(this.allProcesses);
  }

  postProcess() {
    this.process.description = "Beschreibung_1";

    this._processDataService.postProcess(this.process);
  }
}
