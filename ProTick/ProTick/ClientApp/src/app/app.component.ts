import { ChangeDetectorRef, Component } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { isNullOrUndefined } from 'util';

import { MatDialog } from '@angular/material';

import { AuthGuard } from '../classes/Authentication/AuthGuard';
import { ChangePasswordComponent } from './change-password/change-password.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
    mobileQuery: MediaQueryList;

    private _mobileQueryListener: () => void;

    constructor(changeDetectorRef: ChangeDetectorRef, media: MediaMatcher, private auth: AuthGuard, public dialog: MatDialog, ) {
        this.mobileQuery = media.matchMedia('(max-width: 600px)');
        this._mobileQueryListener = () => changeDetectorRef.detectChanges();
        this.mobileQuery.addListener(this._mobileQueryListener);
    }

    logout() {
        sessionStorage.removeItem("jwt");
    }

    private currentPassword: string;
    private newPassword: string;
    private newPasswordConformation: string;

    openChangePasswordDialog(): void {
        const dialogRef = this.dialog.open(ChangePasswordComponent, {
            data: {
                currentPassword: this.currentPassword,
                newPassword: this.newPassword,
                newPasswordConformation: this.newPasswordConformation,
            }
        });

        dialogRef.afterClosed()
            .subscribe(result => {
                if (isNullOrUndefined(result)) return;

                this.currentPassword = result.currentPassword;
                this.newPassword = result.newPassword;
                this.newPasswordConformation = result.newPasswordConformation;

                console.log(this.auth.getUsername());
            });
    }
}
