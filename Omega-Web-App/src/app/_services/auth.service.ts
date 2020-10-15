import { Router } from '@angular/router';
import { AlertifyService } from './alertify.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from "rxjs/operators";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
baseUrl = environment.apiUrl +"Authenticate/";
jwtHelper = new JwtHelperService();
decodedToken : any;
constructor(private http: HttpClient,private alertify: AlertifyService,private router: Router) {}

login(model: any){
  return this.http.post(this.baseUrl+'login', model)
  .pipe(
    map((response: any)=>{
      const user = response;
      if(user){
        localStorage.setItem('token',user.token);
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
      }
    })
  );
}
loggedIn() {
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token);
}
isAdmin() {
  return this.decodedToken.role == "Admin";
}
canActivateAsAdmin(){
  if (this.isAdmin()) {
    return true;
  }
  this.alertify.error('شما دسترسی لازم برای ورود به این بخش را ندارید');
  this.router.navigate(['dashboard']);
  return false;
}
}
