import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Process } from '../../../classes/Process';
import { Subprocess } from '../../../classes/Subprocess';
import { ParentChildRelation } from '../../../classes/ParentChildRelation';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';

@Injectable()
export class ProcessService {

    url: string = 'http://localhost:8080/ProTick';


  constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }

  getProcess(processID: number) {
      return this.http.get<Process>(this.url + `/Process/${processID}`,
          { headers: this.jwtHeader.getJwtHeader() });
  }

    getProcesses(): Observable<Process[]> {
        return this.http.get<Process[]>(this.url + '/Process',
          { headers: this.jwtHeader.getJwtHeader() });
    }
  
  getProcessesWithSubprocess(hasSubprocess: boolean): Observable<Process[]> {
      return this.http.get<Process[]>(this.url + '/Process/hasSubprocess=' + hasSubprocess,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getSubprocesses(): Observable<Subprocess[]> {
      return this.http.get<Subprocess[]>(this.url + '/Subprocess',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getSubprocessById(subprocessID: number): Observable<Subprocess> {
      return this.http.get<Subprocess>(this.url + `/Subprocess/${subprocessID}`,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getSubprocessesByProcessID(processID: number): Observable<Subprocess[]> {
      return this.http.get<Subprocess[]>(this.url + `/Process/${processID}/Subprocesses`,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  deleteSubprocess(subprocessID: number): Observable<Subprocess> {
      return this.http.delete<Subprocess>(this.url + `/Subprocess/${subprocessID}`,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  postProcess(process: Process): Observable<Process> {
      console.log(process);
      return this.http.post<Process>(this.url + '/Process',
          process,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  postSubprocess(subprocess: Subprocess): Observable<Subprocess> {
      console.log(subprocess);
      return this.http.post<Subprocess>(this.url + '/Subprocess',
          subprocess,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  putSubprocess(subprocess: Subprocess, id: number): Observable<Subprocess> {
        console.log(subprocess + ' ' + id);
        return this.http.put<Subprocess>(this.url + '/Subprocess/' + id,
          subprocess,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
    }
}
