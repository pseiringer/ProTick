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

  postTicket(ticket: Ticket): Observable<Ticket> {
    console.log(ticket);

    const token = localStorage.getItem('jwt');

    return this.http.post<Ticket>('http://localhost:8080/ProTick/Ticket', ticket, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer '+ token,
        'Content-Type': 'application/json'
      })
    });
  }
}
