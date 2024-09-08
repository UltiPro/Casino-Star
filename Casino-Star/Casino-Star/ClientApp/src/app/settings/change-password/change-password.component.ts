import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CheckPasswords, RegexPassword } from '../../validation';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
  @Output()
  statusCode = new EventEmitter<boolean>();

  @Output()
  message = new EventEmitter<string>();

  ChangePasswordForm: FormGroup = new FormGroup({
    InputPassword: new FormControl(null, [Validators.pattern(RegexPassword()), Validators.required]),
    InputNewPassword: new FormControl(null, [Validators.pattern(RegexPassword()), Validators.required]),
    InputNewPasswordRepeat: new FormControl(null, [Validators.pattern(RegexPassword()), Validators.required])
  }, {
    validators: CheckPasswords("InputNewPassword", "InputNewPasswordRepeat")
  });

  constructor(private userService: UserService) { }

  password: string = "";
  newPassword: string = "";
  newPasswordRepeat: string = "";

  onSubmit(): void {
    this.userService.UpdatePassword(this.password, this.newPassword, this.newPasswordRepeat).subscribe(status => {
      this.statusCode.emit(status.statusCode);
      this.message.emit(status.message);
    }, error => {
      this.statusCode.emit(error.error.statusCode);
      this.message.emit(error.error.message);
    });
  }
}