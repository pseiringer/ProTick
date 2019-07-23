import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { MatTableModule, MatIconModule, MatButtonModule, MatTabsModule, MatNativeDateModule, MatTooltipModule, MatSelectModule, MatListModule, MatStepperModule } from '@angular/material';
import { MatDatepickerModule, MatDatepickerIntl } from '@angular/material/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DragDropModule } from '@angular/cdk/drag-drop';

import { DatePipe } from '@angular/common'


import { MatDialogModule, MatFormFieldModule, MatInputModule } from '@angular/material';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { CoreModule } from './core/core.module';

import { ProcessesComponent } from './processes/processes.component';
import { CreateProcessComponent } from './create-process/create-process.component';
import { CreateSubprocessComponent } from './create-subprocess/create-subprocess.component';

import { TicketsComponent } from './tickets/tickets.component';
import { CreateTicketComponent } from './tickets/create-ticket/create-ticket.component';

import { TeamsComponent } from './teams/teams.component';

import { LoginComponent } from './login/login.component';
import { JwtHelper } from 'angular2-jwt';
import { AuthGuard } from '../classes/Authentication/AuthGuard';

import { CreateTeamComponent } from './create-team/create-team.component';
import { CreateEmployeeComponent } from './create-employee/create-employee.component';

import { YesNoComponent } from './yes-no/yes-no.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ProcessesComponent,
    CreateProcessComponent,
    TicketsComponent,
    TeamsComponent,
    LoginComponent,
    CreateTeamComponent,
    CreateTicketComponent,
    YesNoComponent,
    CreateEmployeeComponent,
    CreateSubprocessComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    CoreModule,
    MatDialogModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatInputModule,
    MatListModule,
    MatSelectModule,
    MatStepperModule,
    MatTooltipModule,
    MatTabsModule,
    MatTableModule,
    MatNativeDateModule,
    MatIconModule,
    MatButtonModule,
    BrowserAnimationsModule,
    DragDropModule,
    RouterModule.forRoot([
      { path: '', component: TicketsComponent, canActivate: [AuthGuard], pathMatch: 'full'},
      { path: 'counter', component: CounterComponent, canActivate: [AuthGuard] },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard] },
      { path: 'processes', component: ProcessesComponent, canActivate: [AuthGuard] },
      { path: 'create-process', component: CreateProcessComponent, canActivate: [AuthGuard] },
      { path: 'create-subprocess', component: CreateSubprocessComponent, canActivate: [AuthGuard] },
      { path: 'tickets', component: TicketsComponent, canActivate: [AuthGuard] },
      { path: 'create-ticket', component: CreateTicketComponent, canActivate: [AuthGuard] },
      { path: 'teams', component: TeamsComponent, canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent },
    ])
  ],
  providers: [JwtHelper, AuthGuard, DatePipe],
  entryComponents: [
    CreateProcessComponent,
    CreateTeamComponent,
    CreateTicketComponent,
    CreateEmployeeComponent,
    YesNoComponent,
    CreateSubprocessComponent,
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
