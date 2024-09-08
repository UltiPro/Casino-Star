import { Component, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-recharge-account',
  templateUrl: './recharge-account.component.html',
  styleUrls: ['./recharge-account.component.css']
})
export class RechargeAccountComponent {
  @Output()
  statusCode = new EventEmitter<boolean>();

  @Output()
  message = new EventEmitter<string>();

  RechargeAccountForm: FormGroup = new FormGroup({
    InputMoney: new FormControl(null, [Validators.max(1000000), Validators.min(1), Validators.required])
  });

  constructor(private userService: UserService) { }

  value: number = 0;

  onSubmit(): void {
    this.userService.RechargeAccount(this.value).subscribe(status => {
      if (status.statusCode == true) this.userService.RefreshUser();
      this.statusCode.emit(status.statusCode);
      this.message.emit(status.message);
    }, error => {
      this.statusCode.emit(error.error.statusCode);
      this.message.emit(error.error.message);
    });
  }
}