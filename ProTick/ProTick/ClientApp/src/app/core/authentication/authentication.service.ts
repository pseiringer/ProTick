import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Employee } from '../../../classes/Employee';
import { JwtHeader } from '../../../classes/Authentication/JwtHeader';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {

    constructor(private http: HttpClient, private jwtHeader: JwtHeader) { }

    login(credentials: string): Observable<any> {
        return this.http.post<any>('http://localhost:8080/ProTick/Authentication/Login', credentials, {
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        })
    }

    changePassword(user: string, newPass: string, oldPass: string): Observable<Employee> {
        return this.http.put<Employee>('http://localhost:8080/ProTick/Authentication/ChangePassword',
            { username: user, password: newPass, oldPassword: oldPass },
            { headers: this.jwtHeader.getJwtHeaderWithContent('application/json') });
    }
}
