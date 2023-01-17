import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user/user.module';
import { AdminService } from 'src/app/services/admin.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  #listOfUsers: Array<User>;

  constructor(private userService: UserService, private adminService: AdminService, private router: Router) { 
    adminService.GetAllUsers(this.userService.user?.GetId() as number,userService.token as string).subscribe(response => {
      console.log(response.message); // tutaj kończymy
    });
  }

  ngOnInit(): void {
    if(this.userService.user?.GetAdmin() == false || undefined) this.router.navigate(['']);
  }

}
