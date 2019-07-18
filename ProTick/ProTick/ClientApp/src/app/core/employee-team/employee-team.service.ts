import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { EmployeeTeam } from '../../../classes/EmployeeTeam';

@Injectable({
  providedIn: 'root'
})
export class EmployeeTeamService {

  constructor(private http: HttpClient) { }

  getEmployeeTeams(): Observable<EmployeeTeam[]> {
    return this.http.get<EmployeeTeam[]>('http://localhost:8080/ProTick/EmployeeTeam');
  }
  
  postEmployeeTeam(employeeTeam: EmployeeTeam): Observable<EmployeeTeam> {
    console.log(employeeTeam);
    return this.http.post<EmployeeTeam>('http://localhost:8080/ProTick/EmployeeTeam', employeeTeam, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  deleteEmployeeTeam(id: number): Observable<EmployeeTeam> {
    //console.log(id);

    return this.http.delete<EmployeeTeam>('http://localhost:8080/ProTick/EmployeeTeam/' + id);
  }



}



