import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getBaseUrlUser } from 'src/main';
import { User, PostUser } from '../models/user/user.module';
import { UserRegistration, PostUserRegistration } from '../models/user/userRegistration.module';
import { UserLogin, PostUserLogin, PostUserLoginSuccess } from '../models/user/userLogin.module';
import { Router } from '@angular/router';
import { PostAnswer } from 'src/app/models/answer.module';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user: User | null = null;
  #loggedIn: boolean = false;
  token: string | null = null;

  constructor(private http: HttpClient, private router: Router) {
    this.RefreshUser();
  }

  getLoggedIn = () => this.#loggedIn;

  CreateUser(userRegistration: UserRegistration): Observable<PostUserRegistration> {
    return this.http.post<PostUserRegistration>(getBaseUrlUser() + "/register", userRegistration);
  }

  LoginUser(userLogin: UserLogin): Observable<PostUserLogin | PostUserLoginSuccess> {
    return this.http.post<PostUserLogin | PostUserLoginSuccess>(getBaseUrlUser() + "/login", userLogin);
  }

  LoginUserComplete(token: string, user: User) {
    this.user = user;
    this.#loggedIn = true;
    this.token = token;
    window.localStorage.setItem("token", JSON.stringify(this.token));
    window.localStorage.setItem("id", JSON.stringify(this.user.GetId()));
  }

  Logout() {
    window.localStorage.clear();
    this.#loggedIn = false;
    this.router.navigate(['/']);
  }

  GetUserData(id: number, token: string): Observable<PostUser> {
    return this.http.post<PostUser>(getBaseUrlUser() + "/getuser", { id: id, token: token });
  }

  RefreshUser() {
    this.token = window.localStorage.getItem("token");
    const id = window.localStorage.getItem("id");
    if (id != null && this.token != null && this.token?.length > 350) {
      this.GetUserData(parseInt(id), this.token).subscribe(answer => {
        this.user = new User(answer.id, answer.login, answer.email, answer.money, answer.admin);
      });
      this.#loggedIn = true;
    }
  }

  RemoveAccount(password: string): Observable<PostAnswer> {
    return this.http.post<PostAnswer>(getBaseUrlUser() + "/deleteuser", { id: this.user?.GetId(), token: this.token, password: password });
  }

  RechargeAccount(value: number): Observable<PostAnswer> {
    return this.http.post<PostAnswer>(getBaseUrlUser() + "/rechargeaccount", { id: this.user?.GetId(), token: this.token, value: value });
  }

  UpdatePassword(oldPassword: string, newPassword: string, newPasswordRepeat: string): Observable<PostAnswer> {
    return this.http.post<PostAnswer>(getBaseUrlUser() + "/changepassword", { id: this.user?.GetId(), token: this.token, oldPassword: oldPassword, newPassword: newPassword, newPasswordRepeat: newPasswordRepeat });
  }

  UpdateEmail(oldEmail: string, newEmail: string): Observable<PostAnswer> {
    return this.http.post<PostAnswer>(getBaseUrlUser() + "/changeemail", { id: this.user?.GetId(), token: this.token, oldEmail: oldEmail, newEmail: newEmail });
  }
}