import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegexLogin, RegexEmail, RegexPassword, CheckPasswords } from '../../validation';
import { UserRegistration } from '../../models/user/userRegistration.module';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {

  checkbox: boolean | null;

  @Output()
  statusCode = new EventEmitter<boolean>();

  @Output()
  message = new EventEmitter<string>();

  signInForm: FormGroup = new FormGroup({
    InputLogin: new FormControl(null, [Validators.pattern(RegexLogin()), Validators.required]),
    InputEmail: new FormControl(null, [Validators.pattern(RegexEmail()), Validators.required]),
    InputPassword: new FormControl(null, [Validators.pattern(RegexPassword()), Validators.required]),
    InputPassword2: new FormControl(null, [Validators.pattern(RegexPassword()), Validators.required])
  }, {
    validators: CheckPasswords("InputPassword", "InputPassword2")
  });

  user: UserRegistration;

  constructor(private userService: UserService, private router: Router) {
    this.user = new UserRegistration("", "", "", "");
  }

  onSubmit(): void {
    this.userService.CreateUser(this.user).subscribe(status => {
      this.statusCode.emit(status.statusCode);
      this.message.emit(status.message);
    }, error => {
      this.statusCode.emit(error.error.statusCode);
      this.message.emit(error.error.message);
    });
  }
}