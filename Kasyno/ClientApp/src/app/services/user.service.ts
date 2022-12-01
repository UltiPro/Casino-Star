import { HttpClient } from '@angular/common/http';
import { Injectable, Output, EventEmitter } from '@angular/core';
import { User } from '../models/user/user.module';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  #user: any = null;
  #loggedIn : boolean = false;

  constructor(private http: HttpClient) { }

  loginIn(user: User){
    //this.#user = http.get();
    this.#loggedIn = true;
  }

  logOut(){
    this.#user = null;
    this.#loggedIn = false;
  }

  getLoggedIn = () => this.#loggedIn;
}
