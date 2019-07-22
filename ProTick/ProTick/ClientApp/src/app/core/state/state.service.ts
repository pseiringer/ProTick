import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { State } from '../../../classes/State';

@Injectable()
export class StateService {
  constructor(private http: HttpClient) { }

  getStates(): Observable<State[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<State[]>('http://localhost:8080/ProTick/State', {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  getState(id: number): Observable<State> {
    const token = localStorage.getItem('jwt');
    return this.http.get<State>('http://localhost:8080/ProTick/State/' + id, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  postState(state: State): Observable<State> {
    const token = localStorage.getItem('jwt');
    return this.http.post<State>('http://localhost:8080/ProTick/State', state, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      })
    });
  }

  putStates(id: number, state: State): Observable<State> {
    const token = localStorage.getItem('jwt');
    console.log(state);
    return this.http.put<State>('http://localhost:8080/ProTick/State/' + id, state, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      })
    });
  }

  deleteStates(id: number): void {
    const token = localStorage.getItem('jwt');
    this.http.delete('http://localhost:8080/ProTick/State/' + id, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }
}
