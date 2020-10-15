import { AlertifyService } from './../_services/alertify.service';
import { Injectable } from '@angular/core';
import { 
  Router,
  CanActivate,
  ActivatedRouteSnapshot
} from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class RoleGuard implements CanActivate {
  jwtHelper = new JwtHelperService();
  constructor(public authService: AuthService, public router: Router,private alertify: AlertifyService) {}
  
  canActivate(route: ActivatedRouteSnapshot): boolean {
    // this will be passed from the route config
    // on the data property
    const expectedRole = route.data.expectedRole;
    // decode the token to get its payload
    const tokenPayload = this.authService.decodedToken;
    if (
      !this.authService.loggedIn() || 
      tokenPayload.role !== expectedRole
    ) {
      this.alertify.error('شما دسترسی لازم برای ورود به این بخش را ندارید');
      this.router.navigate(['dashboard']);
      return false;
    }
    return true;
  }
}