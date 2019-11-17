import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Ticket } from '../../../classes/Ticket';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';

@Injectable()
export class TicketService {
    constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }



    getTicket(): Observable<Ticket[]> {
        return this.http.get<Ticket[]>('http://localhost:8080/ProTick/Ticket',
            { headers: this.jwtHeader.getJwtHeader() });
    }

    getTicketByID(id: number): Observable<Ticket[]> {
        return this.http.get<Ticket[]>('http://localhost:8080/ProTick/Ticket/' + id,
            { headers: this.jwtHeader.getJwtHeader() });
    }

    getTicketsByUsername(user: string): Observable<Ticket[]> {
        return this.http.get<Ticket[]>('http://localhost:8080/ProTick/Ticket/Username/' + user,
            { headers: this.jwtHeader.getJwtHeader() });
    }

    getTicketsByStateID(id: number): Observable<Ticket[]> {
        return this.http.get<Ticket[]>('http://localhost:8080/ProTick/State/' + id + '/Tickets',
            { headers: this.jwtHeader.getJwtHeader() });
    }

    getTicketsByTeamID(id: number): Observable<Ticket[]> {
        return this.http.get<Ticket[]>('http://localhost:8080/ProTick/Team/' + id + '/Tickets',
            { headers: this.jwtHeader.getJwtHeader() });
    }

    postTicket(ticket: Ticket): Observable<Ticket> {
        console.log(ticket);
        return this.http.post<Ticket>('http://localhost:8080/ProTick/Ticket',
            ticket,
            { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
    }

    putTicket(id: number, ticket: Ticket): Observable<Ticket> {
        console.log(ticket);
        return this.http.put<Ticket>('http://localhost:8080/ProTick/Ticket/' + id,
            ticket,
            { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
    }

    deleteTicket(id: number): Observable<Ticket> {
        return this.http.delete<Ticket>('http://localhost:8080/ProTick/Ticket/' + id,
            { headers: this.jwtHeader.getJwtHeader() });
    }
}
