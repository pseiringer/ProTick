import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Employee } from '../../../classes/Employee';
import { EmployeeTeam } from '../../../classes/EmployeeTeam';
import { Team } from '../../../classes/Team';




@Injectable()
export class EmployeeService {

  constructor(private http: HttpClient) { }

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>('http://localhost:8080/ProTick/Employee');
  }

  getEmployee(id: number): Observable<Employee> {
    return this.http.get<Employee>('http://localhost:8080/ProTick/Employee/' + id);
  }

  getEmployeeTeamssByEmployeeID(id: number): Observable<EmployeeTeam[]> {
    return this.http.get<EmployeeTeam[]>('http://localhost:8080/ProTick/Employee/' + id + '/EmployeeTeams');
  }

  getTeamsByEmployeeID(id: number): Observable<Team[]> {
    return this.http.get<Team[]>('http://localhost:8080/ProTick/Employee/' + id + '/Teams');
  }

  postEmployee(employee: Employee): Observable<Employee> {
    console.log(employee);

    return this.http.post<Employee>('http://localhost:8080/ProTick/Employee', employee, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  putEmployee(id: number, emp: Employee): Observable<Employee> {
    console.log(emp);
    return this.http.put<Employee>('http://localhost:8080/ProTick/Employee/' + id, emp, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  deleteEmployee(id: number): Observable<Employee> {
    //console.log(id);

    return this.http.delete<Employee>('http://localhost:8080/ProTick/Employee/' + id);
  }
}
