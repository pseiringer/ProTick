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
import { FinishTicketComponent } from './finish-ticket/finish-ticket.component';
import { YesNoComponent } from '../yes-no/yes-no.component';


@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css'],
  providers: [TicketService, StateService, ProcessService, TeamService]
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

  displayedColumns: string[] = ['ticketID', 'description', 'stateDescription', 'subprocessDescription', 'teamDescription', 'options'];


  constructor(private ticketService: TicketService,
    private processService: ProcessService,
    private teamService: TeamService,
    private stateService: StateService,
    public dialog: MatDialog) { }


  ngOnInit() {
    this.reloadTickets();
    this.reloadTeams();
    this.reloadStates();

    //this.renderTable();
  }
  

  onFinished(ticket: Ticket) {
    console.log(ticket);
    const dialogRef = this.dialog.open(FinishTicketComponent, {
      data: {
        ticketID: ticket.ticketID,
        description: ticket.description,
        note: ticket.note,
        stateID: ticket.stateID,
        subprocessID: ticket.subprocessID
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(JSON.stringify(result));
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
        console.log(JSON.stringify(result));

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
        console.log(JSON.stringify(result));
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


  reloadStates() {
    this.allStates = [];
    this.stateService.getStates()
      .subscribe(data => {
        //TODO Error handling
        this.allStates = data;
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
          fullTicket.subprocessID = d.ticketID;
          this.processService.getSubprocessById(d.subprocessID)
            .subscribe(subprocess => {
              fullTicket.subprocessDescription = subprocess.description;
              fullTicket.teamID = subprocess.teamID;
              this.teamService.getTeam(subprocess.teamID)
                .subscribe(team => {
                  fullTicket.teamDescription = team.description;
                  console.log(fullTicket);
                  this.allTickets.push(fullTicket);
                  this.renderTable();
                });
            });
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

}
