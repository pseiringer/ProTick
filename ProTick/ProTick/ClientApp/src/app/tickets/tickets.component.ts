import { Component, OnInit, ViewChild } from '@angular/core';
import { ProcessService } from '../core/process/process.service';
import { TicketService } from '../core/ticket/ticket.service';
import { StateService } from '../core/state/state.service';
import { TeamService } from '../core/team/team.service';
import { MatDialog, MatTable } from '@angular/material';
import { Ticket } from '../../classes/Ticket';
import { FullTicket } from '../../classes/FullTicket';
import { Team } from '../../classes/Team';
import { State } from '../../classes/State';
import { CreateTicketComponent } from './create-ticket/create-ticket.component';
import { YesNoComponent } from '../yes-no/yes-no.component';


@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [TicketService, StateService, ProcessService, TeamService]
})
export class TicketsComponent implements OnInit {

  @ViewChild(MatTable, {static: false}) table: MatTable<any>;

  allTeams = [];
  currentTeam: Team = {
    teamID: undefined,
    description: undefined,
    abbreviation: undefined
  }

  allStates = [];
  currentState: State = {
    ticketID: undefined,
    description: undefined,
    stateID: undefined,
    subprocessID: undefined
  }

  allTickets: FullTicket[] = [];
  newTicket: Ticket;

  //expandedElement: newTicket | null;

  displayedColumns: string[] = ['ticketID', 'description', 'stateDescription', 'subprocessDescription', 'teamDescription', 'options'];


  constructor(private ticketService: TicketService,
    private processService: ProcessService,
    private teamService: TeamService,
    private stateService: StateService,
    public dialog: MatDialog) { }


  ngOnInit() {
    this.reloadTickets();
  }

  onFinished() {
    //TODO FinishTicket-Dialog
  }

  onAdd(): void {
    
    const dialogRef = this.dialog.open(CreateTicketComponent, {
      data: {
        ticketID: undefined,
        description: undefined,
        stateID: undefined,
        subprocessID: undefined
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {                
        this.ticketService.postTicket(result)
          .subscribe(data => {
            //TODO error handling
            this.reloadTickets();
          });          
      }
    });
  }

  onEdit(ticket: Ticket) {
    const id = ticket.ticketID;
    const dialogRef = this.dialog.open(CreateTicketComponent, {
      data: {
        ticketID: id,
        description: ticket.description,
        stateID: ticket.stateID,
        subprocessID: ticket.subprocessID
      }
    });

    dialogRef.afterClosed().subscribe(result => {

      if (result !== undefined) {
        this.ticketService.putTicket(id, result)
          .subscribe(data => {
            //TODO error handling
            this.reloadTickets();
          });
      }
    });
  }

  onDelete(id: number) {
    const dialogRef = this.dialog.open(YesNoComponent, {
      data: {
        title: "Delete",
        text: "Do you really want to delete Ticket "+id,
        no: "No",
        yes: "Yes"
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.ticketService.deleteTicket(id)
          .subscribe(data => {
            //TODO error handling
            this.reloadTickets();
            console.log('Team deleted.');
          });
      }
    });
  }

  reloadTickets() {
    this.allTickets = [];
    this.ticketService.getTicket()
      .subscribe(data => {
        //TODO error handling
        data.forEach(d => {
          const fullTicket: FullTicket = {
            ticketID: undefined,
            description: undefined,
            stateID: undefined,
            stateDescription: undefined,
            subprocessID: undefined,
            subprocessDescription: undefined,
            teamID: undefined,
            teamDescription: undefined
          };

          fullTicket.ticketID = d.ticketID
          fullTicket.description = d.description;

          fullTicket.stateID = d.stateID;
          this.stateService.getStateByID(d.stateID)
            .subscribe(state => {
              fullTicket.stateDescription = state.description;
              fullTicket.subprocessID = d.ticketID;
              this.processService.getSubprocessById(d.subprocessID)
                .subscribe(subprocess => {
                  fullTicket.subprocessDescription = subprocess.description;
                  fullTicket.teamID = subprocess.teamID;
                  this.teamService.getTeamById(subprocess.teamID)
                    .subscribe(team => {
                      fullTicket.teamDescription = team.description;
                      console.log(fullTicket);
                      this.allTickets.push(fullTicket);
                      this.table.renderRows();
                    });
                });
            });
        })        
      });
  }
}
