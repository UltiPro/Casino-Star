import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegexLogin, RegexPassword } from '../../validation';
import { UserLogin, PostUserLogin, PostUserLoginSuccess } from '../../models/user/userLogin.module';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user/user.module';

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

  constructor(private userService: UserService, private router: Router) {
    this.user = new UserLogin("", "");
  }

  onSubmit(): void {
    this.userService.LoginUser(this.user).subscribe(status => {
      if (status.statusCode == false) {
        status = status as PostUserLogin;
        this.statusCode.emit(status.statusCode);
        this.message.emit(status.message);
      }
      else {
        status = status as PostUserLoginSuccess;
        this.userService.LoginUserComplete(status.message.token, new User(
          status.message.user.id,
          status.message.user.login,
          status.message.user.email,
          status.message.user.money,
          status.message.user.admin));
        this.router.navigateByUrl('');
      }
    }, error => {
      this.statusCode.emit(error.error.statusCode);
      this.message.emit(error.error.message);
    });
  }
}