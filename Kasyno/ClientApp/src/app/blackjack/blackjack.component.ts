import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-blackjack',
  templateUrl: './blackjack.component.html',
  styleUrls: ['./blackjack.component.css']
})
export class BlackjackComponent implements OnInit {

  public isLoged: boolean;

  constructor(private userService: UserService) { 
    this.isLoged = userService.getLoggedIn();
  }

  ngOnInit(): void {
  }

}
