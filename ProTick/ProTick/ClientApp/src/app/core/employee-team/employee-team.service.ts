import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { EmployeeTeam } from '../../../classes/EmployeeTeam';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';

@Injectable({
  providedIn: 'root'
})
export class EmployeeTeamService {

  constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }

  getEmployeeTeams(): Observable<EmployeeTeam[]> {
      return this.http.get<EmployeeTeam[]>('http://localhost:8080/ProTick/EmployeeTeam',
          { headers: this.jwtHeader.getJwtHeader() });
  }
  
  postEmployeeTeam(employeeTeam: EmployeeTeam): Observable<EmployeeTeam> {
    console.log(employeeTeam);
      return this.http.post<EmployeeTeam>('http://localhost:8080/ProTick/EmployeeTeam',
          employeeTeam,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  deleteEmployeeTeam(id: number): Observable<EmployeeTeam> {
    //console.log(id);

      return this.http.delete<EmployeeTeam>('http://localhost:8080/ProTick/EmployeeTeam/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  deleteEmployeeTeamByTeamAndEmployeeID(tId: number, eId: number): Observable<EmployeeTeam> {
    //console.log(id);

      return this.http.delete<EmployeeTeam>('http://localhost:8080/ProTick/EmployeeTeam/' + tId + '/' + eId,
          { headers: this.jwtHeader.getJwtHeader() });
  }


}



