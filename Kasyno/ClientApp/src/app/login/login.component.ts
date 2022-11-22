import { Component, OnInit} from '@angular/core';
import { CheckLogin, CheckPassword } from '../validation';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  isLoginValid: boolean;
  isPasswordValid: boolean;

  constructor() {
    this.isLoginValid = false;
    this.isPasswordValid = false;
  }

  ngOnInit(): void {
  }

  onChangeLogin(event: any): void {
    if (CheckLogin(event.target.value)) this.isLoginValid = true;
    else this.isLoginValid = false;
  }

  onChangePassword(event: any): void {
    if (CheckPassword(event.target.value)) this.isPasswordValid = true;
    else this.isPasswordValid = false;
  }
}