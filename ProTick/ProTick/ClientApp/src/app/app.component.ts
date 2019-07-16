import { Component } from '@angular/core';
import { AuthGuard } from '../classes/Authentication/AuthGuard';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(private auth: AuthGuard) { }

  title = 'app';
}
