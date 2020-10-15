import { AlertifyService } from './../../_services/alertify.service';
import { AuthService } from './../../_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  model: any = {};
  loading = false;
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  login(){
    this.loading = true;
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('ورود با موفقیت انجام شد')
    },error => {
        console.log(error);
        this.loading = false;
      },
      () => {
        this.loading = false;
      }
    )
  }
  ngOnInit() {
  }

}
