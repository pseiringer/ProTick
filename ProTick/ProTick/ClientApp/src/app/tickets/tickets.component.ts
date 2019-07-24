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
import { ForwardTicketComponent, Functionality } from './forward-ticket/forward-ticket.component';
import { StaticDatabaseObjectsService } from '../core/static-database-objects/static-database-objects.service';
import { AuthGuard } from '../../classes/Authentication/AuthGuard';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [TicketService, StateService, ProcessService, TeamService, StaticDatabaseObjectsService],
  animations: [
    trigger('detailExpand', [
      state('collapsed, void', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
      transition('expanded <=> void', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class TicketsComponent implements OnInit {

  @ViewChild(MatTable, { static: true, read:MatTable }) table: MatTable<any>;
  //@ViewChild(MatSort, { static: false }) sort: MatSort;

  allTeams: Team[] = [];
  selectedTeam: number = 0;
  allStates: State[] = [];
  selectedState: number = 0;
  allTickets: FullTicket[] = [];
  displayedTickets: FullTicket[] = [];

  //dataSource = new MatTableDataSource(this.displayedTickets);

  newTicket: Ticket;

  openDescription: string;
  inProgressDescription: string;
  finishedDescription: string;

  displayedColumns: string[] = ['ticketID', 'description', 'stateDescription', 'subprocessDescription', 'teamDescription', 'options'];

  expandedElement: FullTicket | null;

  ticketDoneString: string = "";

  constructor(private ticketService: TicketService,
    private processService: ProcessService,
    private teamService: TeamService,
    private stateService: StateService,
    public dialog: MatDialog,
    private staticDatabaseObjectService: StaticDatabaseObjectsService,
    private authGuard: AuthGuard) { }


  ngOnInit() {
    this.reloadTickets();
    this.reloadTeams();
    this.reloadStates();

    //this.renderTable();
  }
    

  onBegin(ev: Event, ticket: Ticket) {
    ev.stopPropagation();

    const id = ticket.ticketID;
    console.log(ticket);
    const dialogRef = this.dialog.open(ForwardTicketComponent, {
      data: {
        ticket: {
          ticketID: id,
          description: ticket.description,
          note: ticket.note,
          stateID: ticket.stateID,
          subprocessID: ticket.subprocessID
        },
        functionality: Functionality.Begin
      }
      });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined)
        this.ticketService.putTicket(id, result)
          .subscribe(_ => this.reloadTickets())
    });
  }

  onFinished(ticket: Ticket) {
    const id = ticket.ticketID;
    console.log(ticket);
    const dialogRef = this.dialog.open(ForwardTicketComponent, {
      data: {
        ticket: {
          ticketID: id,
          description: ticket.description,
          note: ticket.note,
          stateID: ticket.stateID,
          subprocessID: ticket.subprocessID
        },
        functionality: Functionality.Finish
      }
      });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined)
        this.ticketService.putTicket(id, result)
          .subscribe(_ => this.reloadTickets())
    });
  }

  onAdd(): void {
    
    const dialogRef = this.dialog.open(CreateTicketComponent, {
      data: {
        ticket: {
          ticketID: undefined,
          description: undefined,
          note: undefined,
          stateID: undefined,
          subprocessID: undefined
        },
        isEdit: false
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
        ticket: {
          ticketID: id,
          description: ticket.description,
          note: ticket.note,
          stateID: ticket.stateID,
          subprocessID: ticket.subprocessID
        },
        isEdit: true
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
    if (this.authGuard.canActivate()) {
      const dialogRef = this.dialog.open(YesNoComponent, {
        data: {
          title: "Delete",
          text: "Do you really want to delete Ticket "+id,
          no: "No",
          yes: "Yes"
        }
      });
      dialogRef.afterClosed().subscribe(result => {
        if (result === true) {
          this.ticketService.deleteTicket(id)
            .subscribe(data => {
              //TODO error handling
              this.reloadTickets();
            });
        }
      });
    }
  }


  reloadStates() {
    this.allStates = [];
    this.stateService.getStates()
      .subscribe(data => {
        //TODO Error handling
        this.allStates = data;

        this.openDescription = this.allStates
          .find(x => x.stateID === this.staticDatabaseObjectService.getStates().Open).description;
        this.inProgressDescription = this.allStates
          .find(x => x.stateID === this.staticDatabaseObjectService.getStates().InProgress).description;
        this.finishedDescription = this.allStates
          .find(x => x.stateID === this.staticDatabaseObjectService.getStates().Finished).description;
      });
  }

  reloadTeams() {
    this.allTeams = [];
    this.teamService.getTeams()
      .subscribe(data => {
        //TODO Error handling
        this.allTeams = data;
      });
  }

  reloadTickets() {
    this.allTickets = [];
    this.ticketService.getTicket()
      .subscribe(data => {
        //TODO error handling
        this.fillTickets(data); 
      });
  }
  

  renderTable() {
    this.displayedTickets = this.allTickets
      .filter(x => {
        if (this.selectedState !== 0 && this.selectedState !== x.stateID) return false;
        if (this.selectedTeam !== 0 && this.selectedTeam !== x.teamID) return false;
        return true;
      })
      .sort((x, y) => x.ticketID - y.ticketID);

    //this.dataSource = new MatTableDataSource(this.displayedTickets);
    //this.dataSource.sort = this.sort;

    this.table.renderRows();
  }
  
  fillTickets(data: Ticket[]) {
    data.forEach(d => {
      const fullTicket: FullTicket = {
        ticketID: undefined,
        description: undefined,
        note: undefined,
        processDescription: undefined,
        stateID: undefined,
        stateDescription: undefined,
        subprocessID: undefined,
        subprocessDescription: undefined,
        teamID: undefined,
        teamDescription: undefined
      };

      fullTicket.ticketID = d.ticketID
      fullTicket.description = d.description;
      fullTicket.note = d.note;
      fullTicket.stateID = d.stateID;
      this.stateService.getState(d.stateID)
        .subscribe(state => {
          fullTicket.stateDescription = state.description;
          fullTicket.subprocessID = d.subprocessID;
          if (fullTicket.subprocessID == -1) {
            fullTicket.subprocessDescription = this.ticketDoneString;
            fullTicket.teamID = -1;
            fullTicket.teamDescription = this.ticketDoneString;
            this.allTickets.push(fullTicket);
            this.renderTable();
          }
          else {
            this.processService.getSubprocessById(d.subprocessID)
              .subscribe(subprocess => {
                fullTicket.subprocessDescription = subprocess.description;
                fullTicket.teamID = subprocess.teamID;
                this.teamService.getTeam(subprocess.teamID)
                  .subscribe(team => {
                    fullTicket.teamDescription = team.description;
                    this.processService.getProcess(subprocess.processID)
                      .subscribe(process => {
                        fullTicket.processDescription = process.description;
                        this.allTickets.push(fullTicket);
                        console.log(fullTicket);
                        this.renderTable();
                      });                   
                  });
              });
          }
        });
    })       
  }


  getTicketsByStateID(stateID) {
    this.selectedState = stateID;
    this.renderTable();
  }

  getEmployeesByTeamID(teamID) {
    this.selectedTeam = teamID;
    this.renderTable();
  }


  ticketFinished(ticket): boolean {
    return ticket.subprocessID === -1;
  }
}
