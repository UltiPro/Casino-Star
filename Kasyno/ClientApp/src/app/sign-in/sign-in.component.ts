import { Component, OnInit } from '@angular/core';
import { CheckEmail, CheckLogin, CheckPassword, CheckPasswords } from '../validation';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {

  isLoginValid: boolean;
  isPasswordValid: boolean;
  isEmailValid: boolean;
  isSecondPasswordValid: boolean;
  password: string;

  constructor() {
    this.isLoginValid = false;
    this.isPasswordValid = false;
    this.isEmailValid = false;
    this.isSecondPasswordValid = false;
    this.password = "";
  }

  ngOnInit(): void {
  }

  onChangeLogin(event: any): void {
    if (CheckLogin(event.target.value)) this.isLoginValid = true;
    else this.isLoginValid = false;
  }

  onChangePassword(event: any): void {
    if (CheckPassword(event.target.value)) {
      this.isPasswordValid = true;
      this.password = event.target.value;
    }
    else {
      this.isPasswordValid = false;
      this.password = "";
    }
  }

  onChangeEmail(event: any): void {
    if (CheckEmail(event.target.value)) this.isEmailValid = true;
    else this.isEmailValid = false;
  }

  onChangeSecondPassword(event: any): void {
    if (CheckPasswords(event.target.value,this.password)) this.isSecondPasswordValid = true;
    else this.isSecondPasswordValid = false;
  }
}
