import { ChangeDetectorRef, Component, ViewChild, OnInit } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { isNullOrUndefined } from 'util';

import { MatDialog, MatSidenav } from '@angular/material';

import { AuthGuard } from '../classes/Authentication/AuthGuard';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AuthenticationService } from './core/authentication/authentication.service';

import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    ngOnInit(): void {
        
    }

    mobileQuery: MediaQueryList;

    private _mobileQueryListener: () => void;

    constructor(changeDetectorRef: ChangeDetectorRef,
        media: MediaMatcher, private auth: AuthGuard,
        public dialog: MatDialog,
        private authService: AuthenticationService,
        private snackBar: MatSnackBar) {
        this.mobileQuery = media.matchMedia('(max-width: 600px)');
        this._mobileQueryListener = () => changeDetectorRef.detectChanges();
        this.mobileQuery.addListener(this._mobileQueryListener);
    }

    logout() {
        sessionStorage.removeItem("jwt");
    }

    @ViewChild(MatSidenav, {static: false}) snav: any;
    toggleSidenav() {
        this.snav.toggle();
    }

    private currentPassword: string;
    private newPassword: string;

    openChangePasswordDialog(): void {
        const dialogRef = this.dialog.open(ChangePasswordComponent, {
            data: {
                currentPassword: this.currentPassword,
                newPassword: this.newPassword,
                newPasswordConformation: '',
            }
        });

        dialogRef.afterClosed()
            .subscribe(result => {
                if (isNullOrUndefined(result)) return;

                this.currentPassword = result.currentPassword;
                this.newPassword = result.newPassword;

                this.authService.changePassword(this.auth.getUsername(), this.newPassword, this.currentPassword)
                    .subscribe(result => {
                        this.snackBar.open('Passwort wurde erfolgreich geändert', undefined, {
                            duration: 20000
                        });

                        this.currentPassword = undefined;
                        this.newPassword = undefined;
                    },
                        errorResult => {
                            this.snackBar.open('Passwort konnte nicht geändert werden', undefined, {
                                duration: 3000
                            });

                            this.currentPassword = undefined;
                            this.newPassword = undefined;
                        });
            });
    }  
}
