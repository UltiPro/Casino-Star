import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegexLogin, RegexPassword } from '../../validation';
import { UserLogin } from '../../models/user/userLogin.module';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  @Output()
  statusCode = new EventEmitter<boolean>();

  @Output()
  message = new EventEmitter<string>();

  loginForm: FormGroup = new FormGroup({
    InputLogin: new FormControl(null, [Validators.pattern(RegexLogin()), Validators.required]),
    InputPassword: new FormControl(null, [Validators.pattern(RegexPassword()), Validators.required]),
  });

  user: UserLogin;

  constructor(private userService: UserService) {
    this.user = new UserLogin("", "");
  }

  onSubmit(): void {
    /*this.userService.LoginUser(this.user).subscribe(status => {
      if(status)
      this.statusCode.emit(status.statusCode);
      this.message.emit(status.message);
    });*/
  }
}