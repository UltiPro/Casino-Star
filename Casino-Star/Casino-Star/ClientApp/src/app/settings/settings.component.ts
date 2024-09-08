import { Component } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent {
  panelOpenState = false;

  public statusCode: boolean | null;
  public message: string;

  constructor(protected userService: UserService, public router: Router) {
    userService.RefreshUser();
    this.statusCode = null;
    this.message = "";
  }

  reciveStatusCode($event: any): void {
    this.statusCode = $event as boolean | null;
  }

  reciveMessage($event: any): void {
    this.message = $event as string;
  }
}