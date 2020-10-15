import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router,
    private alertify: AlertifyService) {}
  canActivate(): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }
    this.router.navigate(['/login']);
    return false;
  }
  // canActivateChild(): boolean {
  //   if (this.authService.isAdmin()) {
  //     return true;
  //   }
  //   this.alertify.error('شما دسترسی لازم برای ورود به این بخش را ندارید');
  //   this.router.navigate(['dashboard'])
  //   return false;
  // }
  
}
