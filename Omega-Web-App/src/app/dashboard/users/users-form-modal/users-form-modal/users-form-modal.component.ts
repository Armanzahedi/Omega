import { AlertifyService } from './../../../../_services/alertify.service';
import { User } from 'src/app/_models/User';
import { UsersService } from './../../../../_services/users.service';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-users-form-modal',
  templateUrl: './users-form-modal.component.html',
  styleUrls: ['./users-form-modal.component.css'],
})
export class UsersFormModalComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: User,
    private usersService: UsersService,
    private dialogRef: MatDialogRef<UsersFormModalComponent>,
    private alertify: AlertifyService
  ) {}
  user: User;
  passwordField: boolean = true;
  ngOnInit() {
    this.user = this.data;
    if (this.user.id != null) {
      this.passwordField = false;
    }
  }
  addUser() {
    if (this.user.id != null) {
      this.usersService.updateUser(this.user.id, this.user).subscribe(
        () => {
          this.alertify.success('کاربر با موفقیت بروز رسانی شد');
        },
        () => {},
        () => {
          this.dialogRef.close();
        }
      );
    } else {
      this.usersService.addUser(this.user).subscribe(
        () => {
          this.alertify.success('کاربر با موفقیت ثبت شد');
        },
        () => {},
        () => {
          this.dialogRef.close();
        }
      );
    }
  }
}
