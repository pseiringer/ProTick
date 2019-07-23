import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { ParentChildRelation } from '../../../classes/ParentChildRelation';

@Injectable()
export class ParentChildRelationService {
  constructor(private http: HttpClient) { }

  getParentChildRelations(): Observable<ParentChildRelation[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<ParentChildRelation[]>('http://localhost:8080/ProTick/ParentChildRelation', {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  getParentChildRelationByID(id: number): Observable<ParentChildRelation[]> {
    const token = localStorage.getItem('jwt');
    return this.http.get<ParentChildRelation[]>('http://localhost:8080/ProTick/ParentChildRelation/' + id, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }

  postParentChildRelation(parentChildRelation: ParentChildRelation): Observable<ParentChildRelation> {
    const token = localStorage.getItem('jwt');
    console.log(parentChildRelation);
    return this.http.post<ParentChildRelation>('http://localhost:8080/ProTick/ParentChildRelation', parentChildRelation, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      })
    });
  }

  putParentChildRelation(id: number, parentChildRelation: ParentChildRelation): Observable<ParentChildRelation> {
    const token = localStorage.getItem('jwt');
    console.log(parentChildRelation);
    return this.http.put<ParentChildRelation>('http://localhost:8080/ProTick/ParentChildRelation/' + id, parentChildRelation, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token,
        'Content-Type': 'application/json'
      })
    });
  }

  deleteParentChildRelation(id: number): Observable<ParentChildRelation> {
    const token = localStorage.getItem('jwt');
    return this.http.delete<ParentChildRelation>('http://localhost:8080/ProTick/ParentChildRelation/' + id, {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + token
      })
    });
  }
}
