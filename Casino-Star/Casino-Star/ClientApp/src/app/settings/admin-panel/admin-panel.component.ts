import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/services/admin.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  constructor(private userService: UserService, protected adminService: AdminService, private router: Router) {
    this.userService.RefreshUser();
  }

  ngOnInit(): void {
    if (this.userService.user?.GetAdmin() == false || undefined) this.router.navigate(['']);
  }

  BanUser(idTarget: number) {
    this.adminService.BanUser(this.userService.id as number, this.userService.token as string, idTarget).subscribe(response => {
      if (response) {
        this.adminService.RefreshGetAllUsers();
      }
    }, error => {
      console.log(error);
    });
  }

  ActiveUser(idTarget: number) {
    this.adminService.ActiveUser(this.userService.id as number, this.userService.token as string, idTarget).subscribe(response => {
      if (response) {
        this.adminService.RefreshGetAllUsers();
      }
    }, error => {
      console.log(error);
    });
  }

  AdminUser(idTarget: number) {
    this.adminService.AdminUser(this.userService.id as number, this.userService.token as string, idTarget).subscribe(response => {
      if (response) {
        this.adminService.RefreshGetAllUsers();
      }
    }, error => {
      console.log(error);
    });
  }

  UpdateMoney(idTarget: number, input: any) {
    this.adminService.UpdateMoney(this.userService.id as number, this.userService.token as string, idTarget, input.target.value).subscribe(response => {
      if (response) {
        this.adminService.RefreshGetAllUsers();
      }
    }, error => {
      console.log(error);
    });
  }
}