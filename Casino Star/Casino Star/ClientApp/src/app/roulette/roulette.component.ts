import { Component, ElementRef, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GamesService } from '../services/games.service';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { PostAnswerWithAngle } from '../models/answer.module';
import { RouletteHistoryItem } from '../models/games/rouletteHistory.module';

@Component({
  selector: 'app-roulette',
  templateUrl: './roulette.component.html',
  styleUrls: ['./roulette.component.css'],
  animations: [
    trigger('ball', [
      state('1', style({
        transform: "translate(-50%,-50%) rotate(90deg)"
      })),
      state('2', style({
        transform: 'translate(-50%,-50%) rotate({{angle}}deg)'
      }), { params: { angle: 180 } }),
      transition('1 => 2', animate('7s ease-out'))
    ])
  ]
})
export class RouletteComponent {
  public state: string = '1';

  public gameOrHistory: boolean = true;
  public gameHistory: Array<RouletteHistoryItem>;
  private copy_gameHistory: Array<RouletteHistoryItem>;

  public blockOfGame = false;

  public statusCode: boolean | null;
  public message: string;
  public messageTitle: string;

  public choosenColor: string = 'red';
  public numberBetted: number;

  public moneyBettedFirst: number | null = null;
  public moneyBettedSecond: number | null = null;

  public angle: number = 0;

  private audioWin = new Audio();
  private audioLose = new Audio();

  private sortByDate: boolean = false;
  private sortByPrize: number = 0;

  private numbersAngles = [0, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26];

  @ViewChild('buttonFirst') buttonFirst: ElementRef<HTMLButtonElement>;

  FirstForm: FormGroup = new FormGroup({
    InputMoney: new FormControl(null, [Validators.min(1), Validators.max(1000000), Validators.required])
  });

  SecondForm: FormGroup = new FormGroup({
    InputMoney: new FormControl(null, [Validators.min(1), Validators.max(1000000), Validators.required]),
    InputNumber: new FormControl(null, [Validators.min(0), Validators.max(36), Validators.required])
  });

  constructor(protected userService: UserService, private gameService: GamesService) {
    userService.RefreshUser();
    this.RefreshGameHistory();
    this.audioWin.src = "../../assets/win.mp3";
    this.audioLose.src = "../../assets/lose.mp3";
    this.audioWin.load();
    this.audioLose.load();
  }

  reciveStatusCode($event: any): void {
    this.statusCode = $event as boolean | null;
  }

  onSubmitFirst() {
    if (this.blockOfGame || !this.userService.loggedIn) return;
    else {
      this.state = '1';
      this.blockOfGame = true;
      this.gameService.RouletteColor(this.userService.user?.GetId() as number, this.userService.token as string, this.choosenColor, this.moneyBettedFirst as number).subscribe(status => {
        this.ContinueGame(status);
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
    this.FirstForm.reset();
  }

  onSubmitSecond() {
    if (this.blockOfGame || !this.userService.loggedIn) return;
    else {
      this.state = '1';
      this.blockOfGame = true;
      this.gameService.RouletteNumber(this.userService.user?.GetId() as number, this.userService.token as string, this.numberBetted, this.moneyBettedSecond as number).subscribe(status => {
        this.ContinueGame(status);
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

  ContinueGame(status: PostAnswerWithAngle) {
    this.angle = 90 + (this.numbersAngles.findIndex(e => e == status.number)) * 10 + (360 * Math.floor(Math.random() * 10 + 1));
    if (status.statusCode == true) {
      this.messageTitle = "Congratulations";
    }
    else {
      this.messageTitle = "Better luck next time";
    }
    this.state = '2';
    setTimeout(() => {
      this.playAudio(status.statusCode);
      this.statusCode = status.statusCode;
      this.message = status.message;
      this.userService.RefreshUser();
      this.RefreshGameHistory();
    }, 7500)
    setTimeout(() => {
      this.blockOfGame = false;
    }, 10500);
  }

  ChangeColor(color: string) {
    this.choosenColor = color;
    if (color == 'red') {
      this.buttonFirst.nativeElement.classList.remove("btn-dark");
      this.buttonFirst.nativeElement.classList.remove("btn-success");
      this.buttonFirst.nativeElement.classList.add("btn-danger");
    }
    else if (color == 'green') {
      this.buttonFirst.nativeElement.classList.remove("btn-dark");
      this.buttonFirst.nativeElement.classList.remove("btn-danger");
      this.buttonFirst.nativeElement.classList.add("btn-success");
    }
    else if (color == 'black') {
      this.buttonFirst.nativeElement.classList.remove("btn-success");
      this.buttonFirst.nativeElement.classList.remove("btn-danger");
      this.buttonFirst.nativeElement.classList.add("btn-dark");
    }
  }

  playAudio(win: boolean) {
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

  RefreshGameHistory() {
    if (!this.userService.loggedIn) return;
    this.gameService.GetRouletteHistory(this.userService.id as number, 100).subscribe(status => {
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

  Reset(element: any) {
    element.target.parentNode.childNodes[1].classList.remove("bg-primary");
    element.target.parentNode.childNodes[2].classList.remove("bg-success");
    element.target.parentNode.childNodes[2].classList.remove("bg-danger");
    element.target.parentNode.childNodes[3].classList.remove("bg-warning");
    element.target.parentNode.childNodes[3].classList.remove("bg-secondary");
    element.target.parentNode.childNodes[4].classList.remove("bg-secondary");
  }
}