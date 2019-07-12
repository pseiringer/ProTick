import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

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
import { LoginComponent } from './login/login.component';
import { JwtHelper } from 'angular2-jwt';
import { AuthGuard } from '../classes/Authentication/AuthGuard';

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
    LoginComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    CoreModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full'},
      { path: 'counter', component: CounterComponent, canActivate: [AuthGuard] },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard] },
      { path: 'processes', component: ProcessesComponent, canActivate: [AuthGuard] },
      { path: 'tickets', component: TicketsComponent, canActivate: [AuthGuard] },
      { path: 'create-ticket', component: CreateTicketComponent, canActivate: [AuthGuard] },
      { path: 'teams', component: TeamsComponent, canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent },
    ])
  ],
  providers: [JwtHelper, AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
