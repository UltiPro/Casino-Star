import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getBaseUrlUser } from 'src/main';
import { User } from '../models/user/user.module';
import { UserRegistration, PostUserRegistration } from '../models/user/userRegistration.module';
import { UserLogin, PostUserLogin } from '../models/user/userLogin.module';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  //tutaj
  
  #user: any = null;
  #loggedIn: boolean = false;

  url = "http://localhost:5000/api/User";

  constructor(private http: HttpClient) { }

  loginIn(user: User) {
    //this.#user = http.get();
    this.#loggedIn = true;
  }

  logOut() {
    this.#user = null;
    this.#loggedIn = false;
  }

  getLoggedIn = () => this.#loggedIn;

  //tutaj

  LoginUser(userLogin: UserLogin): PostUserLogin | null { // nie sprawne
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    /*const answer = this.http.put<PostUserLogin>(getBaseUrlUser(), JSON.stringify(userLogin), httpOptions);*/
    return null;
  }

  CreateUser(userRegistration: UserRegistration): Observable<PostUserRegistration> {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post<PostUserRegistration>(getBaseUrlUser(), JSON.stringify(userRegistration), httpOptions);
  }
}