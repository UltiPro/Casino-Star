import { Component } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent {
  
  panelOpenState = false;

  public isLoged: boolean;

  constructor(private userService: UserService) { 
    this.isLoged = userService.getLoggedIn();
  }

}
