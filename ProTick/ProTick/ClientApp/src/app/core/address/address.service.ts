import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Address } from '../../../classes/Address';


@Injectable({
  providedIn: 'root'
})
export class AddressService {

  constructor(private http: HttpClient) { }

  getAddresses(): Observable<Address[]> {
    return this.http.get<Address[]>('http://localhost:8080/ProTick/Address');
  }

  getAddress(id: number): Observable<Address> {
    return this.http.get<Address>('http://localhost:8080/ProTick/Address/' + id);
  }

  postAddress(address: Address): Observable<Address> {
    console.log(address);

    return this.http.post<Address>('http://localhost:8080/ProTick/Address', address, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  putAddress(id: number, add: Address): Observable<Address> {
    console.log(add);
    return this.http.put<Address>('http://localhost:8080/ProTick/Address/' + id, add, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    });
  }

  deleteAddress(id: number): Observable<Address> {
    //console.log(id);

    return this.http.delete<Address>('http://localhost:8080/ProTick/Address/' + id);
  }
}
