import { Component, ElementRef, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GamesService } from '../services/games.service';
import { CoinflipHistoryItem } from '../models/games/coinflipHistory.module';

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

  public gameOrHistory: boolean = true;
  public gameHistory: Array<CoinflipHistoryItem>;
  private copy_gameHistory: Array<CoinflipHistoryItem>;

  public infoBox = false;
  public blockOfGame = false;

  public decisionCoin: boolean = true;
  public moneyBetted: number | null = null;

  public statusCode: boolean | null;
  public message: string;
  public messageTitle: string;

  private audioWin = new Audio();
  private audioLose = new Audio();

  private sortByDate: boolean = false;
  private sortByPrize: number = 0;
  private sortByDecision: number = 0;
  private sortByCounter: number = 0;

  @ViewChild('golden_coin') golden_coin: ElementRef<HTMLDivElement>;
  @ViewChild('silver_coin') silver_coin: ElementRef<HTMLDivElement>;

  AmmountForm: FormGroup = new FormGroup({
    InputMoney: new FormControl(null, [Validators.min(1), Validators.max(1000005), Validators.required])
  });

  constructor(protected userService: UserService, private gameService: GamesService) {
    userService.RefreshUser();
    this.RefreshGameHistory();
    this.audioWin.src = "../../assets/win.mp3";
    this.audioLose.src = "../../assets/lose.mp3";
    this.audioWin.load();
    this.audioLose.load();
  }

  LounchGame(counter: number): void {
    this.state = '1';
    if (this.blockOfGame || !this.userService.loggedIn) return;
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
          this.RefreshGameHistory();
        }, 3500)
        setTimeout(() => {
          this.blockOfGame = false;
        }, 7000);
      }, error => {
        this.messageTitle = "Something went wrong";
        this.statusCode = error.error.statusCode;
        if (error.status == 400) this.message = "Your input data was invalid";
        else this.message = error.error.message;
        setTimeout(() => {
          this.blockOfGame = false;
        }, 3000);
      });
    }
  }

  ChangeDecision(value: boolean): void {
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

  playAudio(win: boolean): void {
    if (win) this.audioWin.play();
    else this.audioLose.play()
  }

  ChangeView(set: boolean, element: any): void {
    this.gameOrHistory = set;
    if (set) {
      element.target.classList.remove("btn-secondary");
      element.target.classList.add("btn-dark");
      element.target.nextElementSibling.classList.remove("btn-dark");
      element.target.nextElementSibling.classList.add("btn-secondary");
    }
    else {
      element.target.classList.remove("btn-secondary");
      element.target.classList.add("btn-dark");
      element.target.previousElementSibling.classList.remove("btn-dark");
      element.target.previousElementSibling.classList.add("btn-secondary");
    }
  }

  RefreshGameHistory(): void {
    if (!this.userService.loggedIn) return;
    this.gameService.GetCoinFlipHistory(this.userService.id as number, 100).subscribe(status => {
      if (status != null) {
        this.gameHistory = status;
        this.copy_gameHistory = status;
      }
    }, error => {
      this.messageTitle = "Something went wrong with game history";
      this.statusCode = error.error.statusCode;
      this.message = error.error.message;
    });
  }

  SortByDate(element: any): void {
    this.Reset(element);
    this.sortByPrize = 0;
    this.sortByDecision = 0;
    this.sortByCounter = 0;
    this.gameHistory.reverse();
    this.sortByDate = !this.sortByDate;
    if (this.sortByDate) {
      element.target.classList.add("bg-primary");
    }
    else {
      element.target.classList.remove("bg-primary");
    }
  }

  ShowByPrize(element: any) {
    this.Reset(element);
    this.sortByDate = false;
    this.sortByDecision = 0;
    this.sortByCounter = 0;
    this.gameHistory = this.copy_gameHistory;
    if (this.sortByPrize == 0) {
      this.gameHistory = this.gameHistory.filter(e => e.winMoney > 0);
      element.target.classList.add("bg-success");
      this.sortByPrize++;
    }
    else if (this.sortByPrize == 1) {
      this.gameHistory = this.gameHistory.filter(e => e.winMoney < 0);
      element.target.classList.add("bg-danger");
      this.sortByPrize++;
    }
    else {
      this.sortByPrize = 0;
    }
  }

  ShowByDecision(element: any) {
    this.Reset(element);
    this.sortByPrize = 0;
    this.sortByDate = false;
    this.sortByCounter = 0;
    this.gameHistory = this.copy_gameHistory;
    if (this.sortByDecision == 0) {
      this.gameHistory = this.gameHistory.filter(e => e.decision == 'gold');
      element.target.classList.add("bg-warning");
      this.sortByDecision++;
    }
    else if (this.sortByDecision == 1) {
      this.gameHistory = this.gameHistory.filter(e => e.decision == 'silver');
      element.target.classList.add("bg-secondary");
      this.sortByDecision++;
    }
    else {
      this.sortByDecision = 0;
    }
  }

  ShowByCounter(element: any) {
    this.Reset(element);
    this.sortByPrize = 0;
    this.sortByDecision = 0;
    this.sortByDate = false;
    this.gameHistory = this.copy_gameHistory;
    if (this.sortByCounter == 0) {
      element.target.classList.add("bg-secondary");
      this.gameHistory = this.gameHistory.filter(e => e.decisionCounter == '2');
      this.sortByCounter++;
    }
    else if (this.sortByCounter == 1) {
      element.target.classList.add("bg-secondary");
      this.gameHistory = this.gameHistory.filter(e => e.decisionCounter == '4');
      this.sortByCounter++;
    }
    else if (this.sortByCounter == 2) {
      element.target.classList.add("bg-secondary");
      this.gameHistory = this.gameHistory.filter(e => e.decisionCounter == '10');
      this.sortByCounter++;
    }
    else {
      this.sortByCounter = 0;
    }
  }

  Reset(element: any) {
    element.target.parentNode.childNodes[1].classList.remove("bg-primary");
    element.target.parentNode.childNodes[2].classList.remove("bg-success");
    element.target.parentNode.childNodes[2].classList.remove("bg-danger");
    element.target.parentNode.childNodes[3].classList.remove("bg-warning");
    element.target.parentNode.childNodes[3].classList.remove("bg-secondary");
    element.target.parentNode.childNodes[4].classList.remove("bg-secondary");
  }
}