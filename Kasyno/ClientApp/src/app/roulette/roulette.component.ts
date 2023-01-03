import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-roulette',
  templateUrl: './roulette.component.html',
  styleUrls: ['./roulette.component.css']
})
export class RouletteComponent implements OnInit {

  public isLoged: boolean;

  constructor(private userService: UserService) { 
    this.isLoged = userService.getLoggedIn();
  }

  ngOnInit(): void {
  }

}
