import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PostAnswer } from '../models/answer.module';
import { getBaseUrlGames } from 'src/main';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class GamesService {

  constructor(private http: HttpClient, private userService: UserService) { }

  //CoinFlip(): Observable<PostAnswer> {
    //return this.http.post<PostAnswer>(getBaseUrlGames() + "/coinflip", { id: this.userService.user?.GetId(), token: this.userService.token });
  //}
}
