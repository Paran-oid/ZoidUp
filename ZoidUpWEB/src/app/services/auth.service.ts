import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginEntry } from '../models/other/login-entry.model';
import { AccessTokenResponse } from '../models/other/access-token-response.model';
import { catchError, map } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { User } from '../models/user/user.model';
import { RegisterEntry } from '../models/other/register-entry.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  url: string = environment.url;
  public user: User | null = null;
  constructor(private http: HttpClient) {
    const token: string | null = localStorage.getItem('token');
    if (token) {
      this.GetUser(token).subscribe({
        next: (response) => {
          this.user = response;
        },
      });
    }
  }

  public GetUser(token: string) {
    let params = new HttpParams();
    params = params.append('token', token);
    return this.http.get<User>(this.url + '/User/GetUser', { params: params });
  }

  public Login(model: LoginEntry) {
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');

    let params = new HttpParams();
    params = params.append('username', model.username);
    params = params.append('password', model.password);

    return this.http
      .get<AccessTokenResponse>(this.url + '/User/Login', {
        headers: headers,
        params: params,
        responseType: 'json',
      })
      .pipe(
        map((response) => {
          localStorage.setItem('token', response.token);
          window.location.reload();
        })
      );
  }

  public Register(model: RegisterEntry) {
    return this.http.post<AccessTokenResponse>(
      this.url + '/User/Register',
      model
    );
  }

  public Logout() {
    localStorage.removeItem('token');
    this.user = null;
  }
}
