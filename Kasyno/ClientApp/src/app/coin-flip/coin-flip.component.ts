import { Component, ElementRef, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GamesService } from '../services/games.service';

@Component({
  selector: 'app-coin-flip',
  templateUrl: './coin-flip.component.html',
  styleUrls: ['./coin-flip.component.css'],
  animations: [
    trigger('coin', [
      state('1', style({
        transform: "rotateY(0)"
      })),
      state('2', style({
        transform: "rotateY(1800deg)"
      })),
      state('3', style({
        transform: "rotateY(1980deg)"
      })),
      transition('1 => 2', animate(3000)),
      transition('1 => 3', animate(3000))
    ])
  ]
})
export class CoinFlipComponent {
  public state = '1';

  public isLoged: boolean;
  public infoBox = false;
  public blockOfGame = false;

  public decisionCoin: number = 1;
  public moneyBetted: number | null = null;

  public statusCode: boolean | null;
  public message: string;
  public messageTitle: string;

  @ViewChild('golden_coin') golden_coin: ElementRef<HTMLDivElement>;
  @ViewChild('silver_coin') silver_coin: ElementRef<HTMLDivElement>;

  AmmountForm: FormGroup = new FormGroup({
    InputMoney: new FormControl(null, [Validators.min(1), Validators.max(this.userService.user?.GetMoney() as number), Validators.required])
  });

  constructor(private userService: UserService, private gameService: GamesService) {
    this.isLoged = userService.getLoggedIn();
  }

  LounchGame(counter: number) {
    this.blockOfGame = true;
  }

  ChangeDecision(value: boolean) {
    if (value) {
      this.silver_coin.nativeElement.classList.remove("coin-select");
      this.golden_coin.nativeElement.classList.add("coin-select");
      this.decisionCoin = 1;
    }
    else {
      this.golden_coin.nativeElement.classList.remove("coin-select");
      this.silver_coin.nativeElement.classList.add("coin-select");
      this.decisionCoin = 2;
    }
  }

  reciveStatusCode($event: any): void {
    this.statusCode = $event as boolean | null;
  }
}