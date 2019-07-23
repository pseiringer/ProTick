import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { ParentChildRelation } from '../../../classes/ParentChildRelation';

@Injectable()
export class ParentChildRelationService {

  url: string = 'http://localhost:8080/ProTick';

  constructor(private http: HttpClient) { }

  getParentChildRelations(): Observable<ParentChildRelation[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<ParentChildRelation[]>(this.url + '/ParentChildRelation', {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  getParentChildRelationByID(id: number): Observable<ParentChildRelation[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<ParentChildRelation[]>(this.url + '/ParentChildRelation/' + id, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  getParentChildRelationByProcessID(processID: number): Observable<ParentChildRelation[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<ParentChildRelation[]>(this.url + `/Process/${processID}/ParentChildRelations`, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  postParentChildRelation(parentChildRelation: ParentChildRelation): Observable<ParentChildRelation> {
    const token = localStorage.getItem('jwt');
    console.log(parentChildRelation);
    return this.http.post<ParentChildRelation>(this.url + '/ParentChildRelation', parentChildRelation, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      })
    });
  }

  putParentChildRelation(id: number, parentChildRelation: ParentChildRelation): Observable<ParentChildRelation> {
    const token = localStorage.getItem('jwt');
    console.log(parentChildRelation);
    return this.http.put<ParentChildRelation>(this.url + '/ParentChildRelation/' + id, parentChildRelation, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      })
    });
  }

  deleteParentChildRelation(id: number): Observable<ParentChildRelation> {
    const token = localStorage.getItem('jwt');
    return this.http.delete<ParentChildRelation>(this.url + '/ParentChildRelation/' + id, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }
}
