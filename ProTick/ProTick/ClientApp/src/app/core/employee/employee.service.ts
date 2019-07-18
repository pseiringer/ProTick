import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Employee } from '../../../classes/Employee';
import { EmployeeTeam } from '../../../classes/EmployeeTeam';



@Injectable()
export class EmployeeService {

  constructor(private http: HttpClient) { }

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>('http://localhost:8080/ProTick/Employee');
  }

  getEmployeeTeamssByEmployeeID(id: number): Observable<EmployeeTeam[]> {
    return this.http.get<EmployeeTeam[]>('http://localhost:8080/ProTick/Employee/' + id + '/EmployeeTeams');
  }

  postEmployee(employee: Employee): Observable<Employee> {
    console.log(employee);

    return this.http.post<Employee>('http://localhost:8080/ProTick/Employee', employee, {
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
