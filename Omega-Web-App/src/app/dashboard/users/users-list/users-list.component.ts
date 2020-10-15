import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  constructor(private authService : AuthService) { }
  ngOnInit(): void {
  }
  loggedIn(){
    return this.authService.loggedIn();
  }

}
