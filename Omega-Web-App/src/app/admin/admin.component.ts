import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  constructor(private authService : AuthService) { }
  ngOnInit(): void {
  }
  loggedIn(){
    return this.authService.loggedIn();
  }
}