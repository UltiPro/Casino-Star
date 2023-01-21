import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegexEmail } from '../../validation';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-change-email',
  templateUrl: './change-email.component.html',
  styleUrls: ['./change-email.component.css']
})
export class ChangeEmailComponent {
  @Output()
  statusCode = new EventEmitter<boolean>();

  @Output()
  message = new EventEmitter<string>();

  ChangeEmailForm: FormGroup = new FormGroup({
    InputEmail: new FormControl(null, [Validators.pattern(RegexEmail()), Validators.required]),
    InputNewEmail: new FormControl(null, [Validators.pattern(RegexEmail()), Validators.required])
  });

  constructor(private userService: UserService) { }

  email: string = "";
  newEmail: string = "";

  onSubmit(): void {
    this.userService.UpdateEmail(this.email, this.newEmail).subscribe(status => {
      this.statusCode.emit(status.statusCode);
      this.message.emit(status.message);
    }, error => {
      this.statusCode.emit(error.error.statusCode);
      this.message.emit(error.error.message);
    });
  }
}