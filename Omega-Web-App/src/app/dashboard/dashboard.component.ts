import { AuthService } from '../_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  constructor(private authService : AuthService) { }
  ngOnInit(): void {
  }
  loggedIn(){
    return this.authService.loggedIn();
  }
}
