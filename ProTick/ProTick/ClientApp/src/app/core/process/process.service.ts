import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Process } from '../../../classes/Process';

@Injectable()
export class ProcessService {

  constructor(private http: HttpClient) { }

  getProcesses() : Observable<Process[]> {
    return this.http.get<Process[]>('http://localhost:8080/ProTick/Process');
  }

  postProcess(process: Process): Observable<Process> {
    console.log(process);

    return this.http.post<Process>('http://localhost:8080/ProTick/Process', process, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }
}
