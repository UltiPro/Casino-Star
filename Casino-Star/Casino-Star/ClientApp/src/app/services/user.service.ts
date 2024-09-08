import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
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
  token: string | null = null;
  id: number | null = null;
  loggedIn: boolean = false;

  constructor(private http: HttpClient, private router: Router) {
    this.RefreshUser();
  }

  CreateUser(userRegistration: UserRegistration): Observable<PostUserRegistration> {
    return this.http.post<PostUserRegistration>(getBaseUrlUser() + "/register", userRegistration);
  }

  LoginUser(userLogin: UserLogin): Observable<PostUserLogin | PostUserLoginSuccess> {
    return this.http.post<PostUserLogin | PostUserLoginSuccess>(getBaseUrlUser() + "/login", userLogin);
  }

  LoginUserComplete(token: string, user: User) {
    this.user = user;
    this.token = token;
    this.loggedIn = true;
    window.localStorage.setItem("token", JSON.stringify(this.token));
    window.localStorage.setItem("id", JSON.stringify(this.user.GetId()));
  }

  Logout() {
    window.localStorage.clear();
    this.user = null;
    this.token = null;
    this.loggedIn = false;
    this.router.navigate(['/']);
  }

  GetUserData(id: number, token: string): Observable<PostUser> {
    return this.http.post<PostUser>(getBaseUrlUser() + "/getuser", { id: id, token: token });
  }

  RefreshUser() {
    this.token = window.localStorage.getItem("token");
    const tempId = window.localStorage.getItem("id");
    if (tempId != null) this.id = parseInt(tempId);
    if (this.id != null && this.token != null && this.token?.length > 200) {
      this.GetUserData(this.id, this.token).subscribe(answer => {
        this.user = new User(answer.id, answer.login, answer.email, answer.money, answer.admin);
      }, error => {
        console.log(error);
      });
      this.loggedIn = true;
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