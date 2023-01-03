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
  #user: any = null;
  #loggedIn: boolean = false;

  constructor(private http: HttpClient) { }

  getLoggedIn = () => this.#loggedIn;

  LoginUser(userLogin: UserLogin): Observable<PostUserLogin> {
    return this.http.put<PostUserLogin>(getBaseUrlUser() + "/login", userLogin);
  }

  CreateUser(userRegistration: UserRegistration): Observable<PostUserRegistration> {
    return this.http.post<PostUserRegistration>(getBaseUrlUser() + "/register", userRegistration);
  }
}