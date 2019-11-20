import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { ParentChildRelation } from '../../../classes/ParentChildRelation';
import { Subprocess } from '../../../classes/Subprocess';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';

@Injectable()
export class ParentChildRelationService {

  url: string = 'http://localhost:8080/ProTick';

  constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }

  getParentChildRelations(): Observable<ParentChildRelation[]> {
      return this.http.get<ParentChildRelation[]>(this.url + '/ParentChildRelation',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getParentChildRelationByID(id: number): Observable<ParentChildRelation[]> {
      return this.http.get<ParentChildRelation[]>(this.url + '/ParentChildRelation/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getParentChildRelationByProcessID(processID: number): Observable<ParentChildRelation[]> {
      return this.http.get<ParentChildRelation[]>(this.url + `/Process/${processID}/ParentChildRelations`,
          { headers: this.jwtHeader.getJwtHeader() });
  }

  getChildrenBySubprocessID(subprocessID: number): Observable<Subprocess[]> {
      return this.http.get<Subprocess[]>('http://localhost:8080/ProTick/Subprocess/' + subprocessID + '/Children',
          { headers: this.jwtHeader.getJwtHeader() });
  }

  postParentChildRelation(parentChildRelation: ParentChildRelation): Observable<ParentChildRelation> {

    console.log(parentChildRelation);
      return this.http.post<ParentChildRelation>(this.url + '/ParentChildRelation',
          parentChildRelation,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  putParentChildRelation(id: number, parentChildRelation: ParentChildRelation): Observable<ParentChildRelation> {
    console.log(parentChildRelation);
      return this.http.put<ParentChildRelation>(this.url + '/ParentChildRelation/' + id,
          parentChildRelation,
          { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
  }

  deleteParentChildRelation(id: number): Observable<ParentChildRelation> {
      return this.http.delete<ParentChildRelation>(this.url + '/ParentChildRelation/' + id,
          { headers: this.jwtHeader.getJwtHeader() });
  }
}
