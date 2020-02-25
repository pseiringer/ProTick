import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthenticationService } from '../core/authentication/authentication.service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['login.component.css'],
})
export class LoginComponent {
    invalidLogin: boolean;
    invalidFormInput: boolean;

    constructor(private router: Router, private authService: AuthenticationService) { }

    login(form: NgForm) {
        this.invalidFormInput = form.invalid;
        if (this.invalidFormInput) return;
        let credentials = JSON.stringify(form.value);
        this.authService.login(credentials).subscribe(response => {
            let token = (<any>response).token;
            sessionStorage.setItem("jwt", token);
            this.invalidLogin = false;
            this.router.navigate(["/tickets"]);
        }, err => {
            this.invalidLogin = true;
        });
    }
}
