import { Component, OnInit } from '@angular/core';
import { ProcessDataService } from '../core/process/process-data.service';
import { TicketService } from '../core/ticket/ticket.service';
import { AuthGuard } from '../../classes/Authentication/AuthGuard';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [TicketService]
})
export class TicketsComponent implements OnInit {

  allTickets = [];
  firstTicket = {};
  secondTicket = {};

  constructor(private ticketService: TicketService, private auth: AuthGuard) { }

  ngOnInit() {
    this.ticketService.getTicket()
      .subscribe(data => this.allTickets = data);
    this.ticketService.getTicketByID(1)
      .subscribe(data => this.firstTicket = data);
    this.secondTicket = null;
  }

  findTicket() {
    this.ticketService.getTicketByID(1)
      .subscribe(data => this.secondTicket = data);
  }
  
}
