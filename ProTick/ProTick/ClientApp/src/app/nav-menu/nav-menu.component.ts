import { Component } from '@angular/core';
import { AuthGuard } from '../../classes/Authentication/AuthGuard';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  constructor(private authGuard: AuthGuard) { }

  isExpanded = false;

  logout() {
    localStorage.removeItem("jwt");
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
