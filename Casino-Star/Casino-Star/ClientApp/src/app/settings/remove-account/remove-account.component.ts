import { Component, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegexPassword } from '../../validation';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-remove-account',
  templateUrl: './remove-account.component.html',
  styleUrls: ['./remove-account.component.css']
})
export class RemoveAccountComponent {
  @Output()
  statusCode = new EventEmitter<boolean>();

  @Output()
  message = new EventEmitter<string>();

  DeleteAccountForm: FormGroup = new FormGroup({
    InputPassword: new FormControl(null, [Validators.pattern(RegexPassword()), Validators.required])
  });

  constructor(private userService: UserService) { }

  password: string = "";

  onSubmit(): void {
    this.userService.RemoveAccount(this.password).subscribe(status => {
      if (status.statusCode == false) {
        this.statusCode.emit(status.statusCode);
        this.message.emit(status.message);
      }
      else {
        this.userService.Logout();
      }
    }, error => {
      this.statusCode.emit(error.statusCode);
      this.message.emit(error.message);
    });
  }
}