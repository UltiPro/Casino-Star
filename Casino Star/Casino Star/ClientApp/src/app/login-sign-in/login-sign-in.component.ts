import { Component } from '@angular/core';

@Component({
  selector: 'app-login-sign-in',
  templateUrl: './login-sign-in.component.html',
  styleUrls: ['./login-sign-in.component.css']
})
export class LoginSignInComponent {

  public statusCode: boolean | null;
  public message: string;
  public isLogin = true;

  constructor() {
    this.statusCode = null;
    this.message = "";
  }

  ChangeView() { this.isLogin = !this.isLogin; }

  reciveStatusCode($event: boolean | null): void {
    this.statusCode = $event;
  }

  reciveMessage($event: string): void {
    this.message = $event;
  }
}