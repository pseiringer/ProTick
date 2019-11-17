import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { State } from '../../../classes/State';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';

@Injectable()
export class StateService {
  constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }

  getStates(): Observable<State[]> {
      return this.http.get<State[]>('http://localhost:8080/ProTick/State',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getState(id: number): Observable<State> {
      return this.http.get<State>('http://localhost:8080/ProTick/State/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  postState(state: State): Observable<State> {
      return this.http.post<State>('http://localhost:8080/ProTick/State',
          state,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  putStates(id: number, state: State): Observable<State> {
    console.log(state);
      return this.http.put<State>('http://localhost:8080/ProTick/State/' + id,
          state,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  deleteStates(id: number): void {
      this.http.delete('http://localhost:8080/ProTick/State/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }
}
