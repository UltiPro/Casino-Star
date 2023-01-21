import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { getBaseUrlAdmin } from 'src/main';
import { Observable } from 'rxjs';
import { PostAnswerArrayOfUsers } from '../models/answer.module';
import { PostUserFull } from '../models/user/userFull.module';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  listOfUsers: Array<PostUserFull>;

  constructor(private http: HttpClient, private router: Router, private userService: UserService) {
    this.RefreshGetAllUsers();
  }

  GetAllUsers(id: number, token: string): Observable<PostAnswerArrayOfUsers> {
    return this.http.post<PostAnswerArrayOfUsers>(getBaseUrlAdmin() + "/getallusers", { id: id, token: token });
  }

  RefreshGetAllUsers() {
    this.GetAllUsers(this.userService.id as number, this.userService.token as string).subscribe(response => {
      if (response.statusCode == true) {
        this.listOfUsers = response.message;
      }
      else {
        console.log(response.message);
        this.router.navigate(['']);
      }
    }, error => {
      console.log(error);
    });
  }

  BanUser(idAdmin: number, token: string, idTarget: number): Observable<boolean> {
    return this.http.post<boolean>(getBaseUrlAdmin() + "/banuser", { idAdmin: idAdmin, token: token, idTarget: idTarget });
  }

  ActiveUser(idAdmin: number, token: string, idTarget: number): Observable<boolean> {
    return this.http.post<boolean>(getBaseUrlAdmin() + "/activeuser", { idAdmin: idAdmin, token: token, idTarget: idTarget });
  }

  AdminUser(idAdmin: number, token: string, idTarget: number): Observable<boolean> {
    return this.http.post<boolean>(getBaseUrlAdmin() + "/adminuser", { idAdmin: idAdmin, token: token, idTarget: idTarget });
  }

  UpdateMoney(idAdmin: number, token: string, idTarget: number, money: number) {
    return this.http.post<boolean>(getBaseUrlAdmin() + "/updatemoney", { idAdmin: idAdmin, token: token, idTarget: idTarget, money: money });
  }
}