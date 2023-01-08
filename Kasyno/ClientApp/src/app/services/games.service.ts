import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PostAnswer, PostAnswerWithAngle } from '../models/answer.module';
import { getBaseUrlGames } from 'src/main';

@Injectable({
  providedIn: 'root'
})
export class GamesService {

  constructor(private http: HttpClient) { }

  CoinFlip(id: number, token: string, decision: boolean, betMoney: number, gameCounter: number): Observable<PostAnswer> {
    return this.http.post<PostAnswer>(getBaseUrlGames() + "/coinflip", { id: id, token: token, decision: decision, betMoney: betMoney, gameCounter: gameCounter });
  }

  RouletteColor(id: number, token: string, decision: string, betMoney: number): Observable<PostAnswerWithAngle> {
    return this.http.post<PostAnswerWithAngle>(getBaseUrlGames() + "/roulettecolor", { id: id, token: token, decision: decision, betMoney: betMoney });
  }

  RouletteNumber(id: number, token: string, decision: number, betMoney: number): Observable<PostAnswerWithAngle> {
    return this.http.post<PostAnswerWithAngle>(getBaseUrlGames() + "/roulettenumber", { id: id, token: token, decision: decision, betMoney: betMoney });
  }
}