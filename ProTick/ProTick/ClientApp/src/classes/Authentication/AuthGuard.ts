import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { JwtHelper } from 'angular2-jwt';
import { StaticDatabaseObjectsService } from '../../app/core/static-database-objects/static-database-objects.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private jwtHelper: JwtHelper, private router: Router, private staticDbObj: StaticDatabaseObjectsService) { }

  canActivate(): boolean {
    var token = this.getToken();

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }

    if (this.router.url != '/login')
      this.router.navigate(['login']);

    return false;
  }

  private getToken(): string {
    return localStorage.getItem('jwt');
  }

  private getDecodedToken(): any {
    return this.jwtHelper.decodeToken(this.getToken());
  }

  getUsername(): string {
    return this.getDecodedToken().nameid;
  }

  getRole(): string {
    return this.getDecodedToken().role;
  }

  isAdmin(): boolean {
    return this.getRole() === this.staticDbObj.getRoles().Admin;
  }

  isEmployee(): boolean {
    return this.getRole() === this.staticDbObj.getRoles().Employee;
  }

}
