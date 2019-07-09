import { Component, OnInit } from '@angular/core';
import { ProcessDataService } from '../core/process/process-data.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [ProcessDataService]
})
export class TicketsComponent implements OnInit {

  allProcesses = [];

  constructor(private _processDataService: ProcessDataService) { }

  ngOnInit() {
    this._processDataService.getProcesses()
      .subscribe(data => this.allProcesses = data);
  }

}
