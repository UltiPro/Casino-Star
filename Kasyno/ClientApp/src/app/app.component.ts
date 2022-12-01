import { Component } from '@angular/core';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title : string = 'Casino Star';
  isLogged: boolean;

  constructor(private userService: UserService){
    this.isLogged = userService.getLoggedIn();
  }
}
