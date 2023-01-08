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

  public decisionCoin: boolean = true;
  public moneyBetted: number | null = null;

  public statusCode: boolean | null;
  public message: string;
  public messageTitle: string;

  private audioWin = new Audio();
  private audioLose = new Audio();

  @ViewChild('golden_coin') golden_coin: ElementRef<HTMLDivElement>;
  @ViewChild('silver_coin') silver_coin: ElementRef<HTMLDivElement>;

  AmmountForm: FormGroup = new FormGroup({
    InputMoney: new FormControl(null, [Validators.min(1), Validators.max(this.userService.user?.GetMoney() as number), Validators.required])
  });

  constructor(private userService: UserService, private gameService: GamesService) {
    this.isLoged = userService.getLoggedIn();
    this.audioWin.src = "../../assets/win.mp3";
    this.audioLose.src = "../../assets/lose.mp3";
    this.audioWin.load();
    this.audioLose.load();
  }

  LounchGame(counter: number) {
    this.state = '1';
    if (this.blockOfGame) return;
    else {
      this.blockOfGame = true;
      this.gameService.CoinFlip(this.userService.user?.GetId() as number, this.userService.token as string, this.decisionCoin, this.moneyBetted as number, counter).subscribe(status => {
        if (status.statusCode == true) {
          if (this.decisionCoin) this.state = '2';
          else this.state = '3';
          this.messageTitle = "Congratulations";
        }
        else {
          if (this.decisionCoin) this.state = '3';
          else this.state = '2';
          this.messageTitle = "Better luck next time";
        }
        setTimeout(() => {
          this.playAudio(status.statusCode);
          this.statusCode = status.statusCode;
          this.message = status.message;
          this.userService.RefreshUser();
        }, 3500)
        setTimeout(() => {
          this.blockOfGame = false;
        }, 7000);
      });
    }
  }

  ChangeDecision(value: boolean) {
    if (this.blockOfGame) return;
    if (value) {
      this.silver_coin.nativeElement.classList.remove("coin-select");
      this.golden_coin.nativeElement.classList.add("coin-select");
      this.decisionCoin = true;
    }
    else {
      this.golden_coin.nativeElement.classList.remove("coin-select");
      this.silver_coin.nativeElement.classList.add("coin-select");
      this.decisionCoin = false;
    }
  }

  reciveStatusCode($event: any): void {
    this.statusCode = $event as boolean | null;
  }

  playAudio(win: boolean) {
    if (win) this.audioWin.play();
    else this.audioLose.play()
  }
}