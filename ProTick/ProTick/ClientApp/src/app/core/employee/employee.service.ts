import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Employee } from '../../../classes/Employee';
import { EmployeeTeam } from '../../../classes/EmployeeTeam';
import { Team } from '../../../classes/Team';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';




@Injectable()
export class EmployeeService {

  constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }

  getEmployees(): Observable<Employee[]> {
      return this.http.get<Employee[]>('http://localhost:8080/ProTick/Employee',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getEmployee(id: number): Observable<Employee> {
      return this.http.get<Employee>('http://localhost:8080/ProTick/Employee/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getEmployeeTeamssByEmployeeID(id: number): Observable<EmployeeTeam[]> {
      return this.http.get<EmployeeTeam[]>('http://localhost:8080/ProTick/Employee/' + id + '/EmployeeTeams',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getTeamsByEmployeeID(id: number): Observable<Team[]> {
      return this.http.get<Team[]>('http://localhost:8080/ProTick/Employee/' + id + '/Teams',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  postEmployee(employee: Employee): Observable<Employee> {
    console.log(employee);

      return this.http.post<Employee>('http://localhost:8080/ProTick/Employee',
          employee,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  putEmployee(id: number, emp: Employee): Observable<Employee> {
    console.log(emp);
      return this.http.put<Employee>('http://localhost:8080/ProTick/Employee/' + id,
          emp,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  deleteEmployee(id: number): Observable<Employee> {
    //console.log(id);

      return this.http.delete<Employee>('http://localhost:8080/ProTick/Employee/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }
}
