import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Team } from '../../../classes/Team';
import { Employee } from '../../../classes/Employee';
import { EmployeeTeam } from '../../../classes/EmployeeTeam';

@Injectable()
export class TeamService {
  constructor(private http: HttpClient) { }

  getTeams(): Observable<Team[]> {
    return this.http.get<Team[]>('http://localhost:8080/ProTick/Team');
  }
  
  getTeam(id: number): Observable<Team> {
    return this.http.get<Team>('http://localhost:8080/ProTick/Team/' + id);
  }

  getTeamsByUsername(user: string): Observable<Team[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<Team[]>('http://localhost:8080/ProTick/Team/Username/' + user, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
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

  getEmployeeTeamssByTeamID(id: number): Observable<EmployeeTeam[]> {
    return this.http.get<EmployeeTeam[]>('http://localhost:8080/ProTick/Team/' + id + '/EmployeeTeams');
  }

  putTeam(id: number, team: Team): Observable<Team> {
    console.log(team);
    return this.http.put<Team>('http://localhost:8080/ProTick/Team/' + id, team, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  deleteTeam(id: number): Observable<Team> {
    //console.log(id);

    return this.http.delete<Team>('http://localhost:8080/ProTick/Team/' + id);
  }

}
