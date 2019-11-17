import { Injectable } from "@angular/core";
import { HttpHeaders } from '@angular/common/http';

@Injectable()
export class JwtHeader {
  constructor() { }

    getJwtHeader(): HttpHeaders {
        const token = localStorage.getItem('jwt');
        return new HttpHeaders({
            'Authorization': 'Bearer ' + token
        });
    }

    getJwtHeaderWithContent(contentType: string): HttpHeaders {
        return this.getJwtHeader().append('Content-Type', contentType);
    }

}
