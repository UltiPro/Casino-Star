import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-coin-flip',
  templateUrl: './coin-flip.component.html',
  styleUrls: ['./coin-flip.component.css']
})
export class CoinFlipComponent implements OnInit {

  public isLoged: boolean;

  constructor(private userService: UserService) { 
    this.isLoged = userService.getLoggedIn();
  }

  ngOnInit(): void {
  }
}
