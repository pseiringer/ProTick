import { Component, OnInit } from '@angular/core';
import { ProcessDataService } from '../core/process/process-data.service';
import { TicketService } from '../core/ticket/ticket.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [TicketService]
})
export class TicketsComponent implements OnInit {

  allTickets = [];
  firstTicket = {};

  constructor(private ticketService: TicketService) { }

  ngOnInit() {
    this.ticketService.getTicket()
      .subscribe(data => this.allTickets = data);
    this.ticketService.getTicketByID(1)
      .subscribe(data => this.firstTicket = data);
  }

}
