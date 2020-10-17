import { User } from './../_models/User';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  baseUrl = environment.apiUrl;
  token = localStorage.getItem('token');
  headers = new HttpHeaders().set('Authorization', 'Bearer ' + this.token);
  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<User[]> {
    var users: User[] = new Array<User>();
    return this.http
      .get<User[]>(this.baseUrl + 'users', {
        observe: 'response',
        headers: this.headers,
      })
      .pipe(
        map((response) => {
          users = response.body;
          return users;
        })
      );
  }
  addUser(user: User) {
    return this.http.post(this.baseUrl + 'users/', user, {
      headers: this.headers,
    });
  }
  updateUser(id: string, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user, {
      headers: this.headers,
    });
  }
  deleteUser(id: string) {
    return this.http.delete(this.baseUrl + 'users/' + id, {
      headers: this.headers,
    });
  }
}
