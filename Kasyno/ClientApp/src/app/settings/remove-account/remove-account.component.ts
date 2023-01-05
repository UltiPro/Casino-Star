import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegexPassword } from '../../validation';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-remove-account',
  templateUrl: './remove-account.component.html',
  styleUrls: ['./remove-account.component.css']
})
export class RemoveAccountComponent {

  DeleteAccountForm: FormGroup = new FormGroup({
    InputPassword: new FormControl(null, [Validators.pattern(RegexPassword()), Validators.required])
  });

  password: string | null = null;

  onSubmit(): void {
    
  }

}
