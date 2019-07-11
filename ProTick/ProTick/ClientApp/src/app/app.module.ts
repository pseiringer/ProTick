import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { MatTableModule, MatIconModule, MatButtonModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { CreateTicketComponent } from './create-ticket/create-ticket.component';
import { CoreModule } from './core/core.module';
import { TicketsComponent } from './tickets/tickets.component';
import { ProcessesComponent } from './processes/processes.component';
import { TeamsComponent } from './teams/teams.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    CreateTicketComponent,
    TicketsComponent,
    ProcessesComponent,
    TeamsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    CoreModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'processes', component: ProcessesComponent },
      { path: 'tickets', component: TicketsComponent },
      { path: 'create-ticket', component: CreateTicketComponent },
      { path: 'teams', component: TeamsComponent },
    ]),
    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
