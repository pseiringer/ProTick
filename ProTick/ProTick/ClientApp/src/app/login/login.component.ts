import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['login.component.css'],
})
export class LoginComponent {
    invalidLogin: boolean;
    invalidFormInput: boolean;

    constructor(private router: Router, private http: HttpClient) { }

    login(form: NgForm) {
        this.invalidFormInput = form.invalid;
        if (this.invalidFormInput) return;

        let credentials = JSON.stringify(form.value);
        this.http.post("http://localhost:8080/ProTick/Authentication/Login", credentials, {
            headers: new HttpHeaders({
                "Content-Type": "application/json"
            })
        }).subscribe(response => {
            let token = (<any>response).token;
            //console.log('token recieved');
            sessionStorage.setItem("jwt", token);
            this.invalidLogin = false;
            this.router.navigate(["/tickets"]);
        }, err => {
            this.invalidLogin = true;
        });
    }

}
