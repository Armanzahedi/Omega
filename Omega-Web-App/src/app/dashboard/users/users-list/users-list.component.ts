import { UsersService } from './../../../_services/users.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';
import { MatDialog } from '@angular/material/dialog';
import { UsersFormModalComponent } from '../users-form-modal/users-form-modal/users-form-modal.component';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css'],
})
export class UsersListComponent implements OnInit {
  users: User[];
  selectedUser: User;
  loading: boolean = true;
  displayedColumns: string[] = ['userName', 'email', 'role', 'actions'];
  constructor(
    private usersService: UsersService,
    public dialog: MatDialog,
    private alertify: AlertifyService
  ) {}
  ngOnInit(): void {
    this.loadUsers();
  }
  getUserRole(user: User) {
    if (user.isAdmin) {
      user.role = 'مدیر';
    } else {
      user.role = 'کاربر';
    }
  }
  loadUsers() {
    this.loading = true;
    this.usersService.getAllUsers().subscribe(
      (res: Array<User>) => {
        res.forEach((user) => {
          this.getUserRole(user);
        });
        this.users = res;
      },
      (error) => {
        console.log(error);
      },
      () => {
        this.loading = false;
      }
    );
  }
  addUser() {
    this.openUserForm(new User());
  }
  editUser(userId: string) {
    this.selectedUser = this.users.find((u) => u.id === userId);
    this.openUserForm(this.selectedUser);
  }
  deleteUser(userId: string) {
    this.usersService.deleteUser(userId).subscribe(() => {
      this.alertify.success('کاربر با موفقیت حذف شد');
      this.users = this.users.filter((u) => u.id !== userId);
    });
  }
  openUserForm(user: User) {
    this.dialog.open(UsersFormModalComponent, {
      data: user,
    });
  }
}
