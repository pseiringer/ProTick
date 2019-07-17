import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Team } from '../../../classes/Team';
import { Employee } from '../../../classes/Employee';

@Injectable()
export class TeamService {
  constructor(private http: HttpClient) { }

  getTeams(): Observable<Team[]> {
    return this.http.get<Team[]>('http://localhost:8080/ProTick/Team');
  }

  getTeamById(id: number): Observable<Team> {
    return this.http.get<Team>('http://localhost:8080/ProTick/Team/' + id);
  }

  postTeam(team: Team): Observable<Team> {
    console.log(team);

    return this.http.post<Team>('http://localhost:8080/ProTick/Team', team, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  getEmployeesByTeamID(id: number): Observable<Employee[]> {
    return this.http.get<Employee[]>('http://localhost:8080/ProTick/Team/' + id + '/Employees');
  }

  deleteTeam(id: number): Observable<Team> {
    //console.log(id);

    return this.http.delete<Team>('http://localhost:8080/ProTick/Team/' + id);
  }

}
