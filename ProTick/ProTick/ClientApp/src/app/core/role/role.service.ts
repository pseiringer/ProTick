import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Role } from '../../../classes/Role';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }

  getRoles(): Observable<Role[]> {
      return this.http.get<Role[]>('http://localhost:8080/ProTick/Role',
          { headers: this.jwtHeader.getJwtHeader() });
  }
}
