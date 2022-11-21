import { Component, OnInit } from '@angular/core';
import { LoginComponent } from '../login/login.component';
import { SignInComponent } from '../sign-in/sign-in.component';

@Component({
  selector: 'app-login-sign-in',
  templateUrl: './login-sign-in.component.html',
  styleUrls: ['./login-sign-in.component.css']
})
export class LoginSignInComponent implements OnInit {

  public isLogin = false;

  constructor() { }

  ngOnInit(): void {
  }

  ChangeView() { this.isLogin = !this.isLogin; }
}
