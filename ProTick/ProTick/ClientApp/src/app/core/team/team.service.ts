import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Team } from '../../../classes/Team';
import { Employee } from '../../../classes/Employee';
import { EmployeeTeam } from '../../../classes/EmployeeTeam';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';

@Injectable()
export class TeamService {
  constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }

  getTeams(): Observable<Team[]> {
      return this.http.get<Team[]>('http://localhost:8080/ProTick/Team',
          { headers: this.jwtHeader.getJwtHeader() });
  }
  
  getTeam(id: number): Observable<Team> {
      return this.http.get<Team>('http://localhost:8080/ProTick/Team/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getTeamsByUsername(user: string): Observable<Team[]> {
    return this.http.get<Team[]>('http://localhost:8080/ProTick/Team/Username/' + user,
            { headers: this.jwtHeader.getJwtHeader() });
  }

  postTeam(team: Team): Observable<Team> {
    console.log(team);

      return this.http.post<Team>('http://localhost:8080/ProTick/Team',
          team,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }


  getEmployeesByTeamID(id: number): Observable<Employee[]> {
      return this.http.get<Employee[]>('http://localhost:8080/ProTick/Team/' + id + '/Employees',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getEmployeeTeamssByTeamID(id: number): Observable<EmployeeTeam[]> {
      return this.http.get<EmployeeTeam[]>('http://localhost:8080/ProTick/Team/' + id + '/EmployeeTeams',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  putTeam(id: number, team: Team): Observable<Team> {
    console.log(team);
      return this.http.put<Team>('http://localhost:8080/ProTick/Team/' + id,
          team,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  deleteTeam(id: number): Observable<Team> {
      return this.http.delete<Team>('http://localhost:8080/ProTick/Team/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }

}
