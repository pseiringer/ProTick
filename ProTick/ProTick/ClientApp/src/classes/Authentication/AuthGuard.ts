import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { JwtHelper } from 'angular2-jwt';
import { isNullOrUndefined } from 'util';
import { StaticDatabaseObjectsService } from '../../app/core/static-database-objects/static-database-objects.service';
import { isNull, isNullOrUndefined } from 'util';

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
        return sessionStorage.getItem('jwt');
    }

    private getDecodedToken(): any {
        var token = this.getToken();
        if (isNullOrUndefined(token)) return null;
        return this.jwtHelper.decodeToken(token);
    }

    getUsername(): string {
        var token = this.getDecodedToken();
        if (isNullOrUndefined(token)) return null;
        return token.nameid;
    }

    getRole(): string {
        var token = this.getDecodedToken();
        if (isNullOrUndefined(token)) return null;
        return token.role;
    }

    isAdmin(): boolean {
        return this.getRole() === this.staticDbObj.getRoles().Admin;
    }

    isEmployee(): boolean {
        return this.getRole() === this.staticDbObj.getRoles().Employee;
    }

}
