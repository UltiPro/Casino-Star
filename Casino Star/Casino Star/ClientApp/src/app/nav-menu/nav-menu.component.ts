import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from "../services/user.service"

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  public appName: string = "Casino Star";

  constructor(public router: Router, protected userService: UserService) {
    userService.RefreshUser();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}