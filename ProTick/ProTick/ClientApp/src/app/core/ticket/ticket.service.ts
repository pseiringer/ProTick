import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Ticket } from '../../../classes/Ticket';

@Injectable()
export class TicketService {
  constructor(private http: HttpClient) { }

  getTicket(): Observable<Ticket[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<Ticket[]>('http://localhost:8080/ProTick/Ticket', {
        headers: new HttpHeaders({
          'Authorization': 'Bearer ' + token
        })
      });
  }

  getTicketByID(id: number): Observable<Ticket[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<Ticket[]>('http://localhost:8080/ProTick/Ticket/' + id, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  getTicketsByUsername(user: string): Observable<Ticket[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<Ticket[]>('http://localhost:8080/ProTick/Ticket/Username/' + user, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  getTicketsByStateID(id: number): Observable<Ticket[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<Ticket[]>('http://localhost:8080/ProTick/State/' + id + '/Tickets', {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  getTicketsByTeamID(id: number): Observable<Ticket[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<Ticket[]>('http://localhost:8080/ProTick/Team/' + id + '/Tickets', {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  postTicket(ticket: Ticket): Observable<Ticket> {
    const token = localStorage.getItem('jwt');
    console.log(ticket);
    return this.http.post<Ticket>('http://localhost:8080/ProTick/Ticket', ticket, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      })
    });
  }

  putTicket(id: number, ticket: Ticket): Observable<Ticket> {
    const token = localStorage.getItem('jwt');
    console.log(ticket);
    return this.http.put<Ticket>('http://localhost:8080/ProTick/Ticket/' + id, ticket, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      })
    });
  }

  deleteTicket(id: number): Observable<Ticket> {
    const token = localStorage.getItem('jwt');
    return this.http.delete<Ticket>('http://localhost:8080/ProTick/Ticket/' + id, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }
}
