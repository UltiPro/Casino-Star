import { Component, ElementRef, ViewChild } from '@angular/core';
import { UserService } from '../services/user.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GamesService } from '../services/games.service';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { PostAnswerWithAngle } from '../models/answer.module';

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

  public isLoged: boolean;
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

  private numbersAngles = [0, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26];

  @ViewChild('buttonFirst') buttonFirst: ElementRef<HTMLButtonElement>;

  FirstForm: FormGroup = new FormGroup({
    InputMoney: new FormControl(null, [Validators.min(1), Validators.max(this.userService.user?.GetMoney() as number), Validators.required])
  });

  SecondForm: FormGroup = new FormGroup({
    InputMoney: new FormControl(null, [Validators.min(1), Validators.max(this.userService.user?.GetMoney() as number), Validators.required]),
    InputNumber: new FormControl(null, [Validators.min(0), Validators.max(36), Validators.required])
  });

  constructor(private userService: UserService, private gameService: GamesService) {
    this.isLoged = userService.getLoggedIn();
    this.audioWin.src = "../../assets/win.mp3";
    this.audioLose.src = "../../assets/lose.mp3";
    this.audioWin.load();
    this.audioLose.load();
  }

  reciveStatusCode($event: any): void {
    this.statusCode = $event as boolean | null;
  }

  onSubmitFirst() {
    if (this.blockOfGame) return;
    else {
      this.state = '1';
      this.blockOfGame = true;
      this.gameService.RouletteColor(this.userService.user?.GetId() as number, this.userService.token as string, this.choosenColor, this.moneyBettedFirst as number).subscribe(status => {
        this.ContinueGame(status);
      });
    }
  }

  onSubmitSecond() {
    if (this.blockOfGame) return;
    else {
      this.state = '1';
      this.blockOfGame = true;
      this.gameService.RouletteNumber(this.userService.user?.GetId() as number, this.userService.token as string, this.numberBetted, this.moneyBettedSecond as number).subscribe(status => {
        this.ContinueGame(status);
      });
    }
  }

  ContinueGame(status : PostAnswerWithAngle){
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
}