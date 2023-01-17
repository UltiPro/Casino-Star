import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { getBaseUrlAdmin } from 'src/main';
import { Observable } from 'rxjs';
import { PostAnswerArrayOfUsers } from '../models/answer.module';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient, private router: Router) { 

  }

  GetAllUsers(id: number, token: string): Observable<PostAnswerArrayOfUsers> {
    return this.http.post<PostAnswerArrayOfUsers>(getBaseUrlAdmin() + "/getallusers", { id: id, token: token });
  }
}
