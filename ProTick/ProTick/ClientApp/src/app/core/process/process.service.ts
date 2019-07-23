import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Process } from '../../../classes/Process';
import { Subprocess } from '../../../classes/Subprocess';
import { ParentChildRelation } from '../../../classes/ParentChildRelation';

@Injectable()
export class ProcessService {

  url: string = 'http://localhost:8080/ProTick';

  constructor(private http: HttpClient) { }

  getProcesses(): Observable<Process[]> {
    return this.http.get<Process[]>(this.url + '/Process');
  }

  getProcessesWithSubprocess(hasSubprocess: boolean): Observable<Process[]> {
    return this.http.get<Process[]>(this.url + '/Process/hasSubprocess=' + hasSubprocess);
  }

  getSubprocesses(): Observable<Subprocess[]> {
    return this.http.get<Subprocess[]>(this.url + '/Subprocess');
  }

  getSubprocessById(subprocessID: number): Observable<Subprocess> {
    return this.http.get<Subprocess>(this.url + `/Subprocess/${subprocessID}`);
  }

  getSubprocessesByProcessID(processID: number): Observable<Subprocess[]> {
    return this.http.get<Subprocess[]>(this.url + `/Process/${processID}/Subprocesses`);
  }

  deleteSubprocess(subprocessID: number): Observable<Subprocess> {
    return this.http.delete<Subprocess>(this.url + `/Subprocess/${subprocessID}`);
  }

  postProcess(process: Process): Observable<Process> {
    console.log(process);

    return this.http.post<Process>(this.url + '/Process', process, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  postSubprocess(subprocess: Subprocess): Observable<Subprocess> {
    console.log(subprocess);

    return this.http.post<Subprocess>(this.url + '/Subprocess', subprocess, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }
}
