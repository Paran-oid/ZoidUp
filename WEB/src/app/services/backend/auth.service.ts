import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginEntry } from '../../models/auth/login-entry.model';
import { AccessTokenResponse } from '../../models/auth/access-token-response.model';
import {
  catchError,
  map,
  BehaviorSubject,
  Observable,
  ReplaySubject,
  throwError,
} from 'rxjs';
import { User } from '../../models/user/user.model';
import { RegisterEntry } from '../../models/auth/register-entry.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  url: string = 'api/auth';

  //subject gets data
  private user = new BehaviorSubject<User | null>(null);

  //observer reads data
  public user$ = this.user.asObservable();

  constructor(private http: HttpClient) {
    const token: string | null = localStorage.getItem('token');
    if (token) {
      this.GetUser(token).subscribe();
    }
  }

  public GetUser(token: string) {
    return this.http.get<User>(this.url).pipe(
      map((user) => {
        this.user.next(user);
      }),
      catchError((error) => {
        localStorage.removeItem('token');
        this.user.next(null);
        return error;
      })
    );
  }

  public Login(model: LoginEntry) {
    let headers = new HttpHeaders();
    headers = headers.append('Content-Type', 'application/json');

    const body = {
      username: model.username,
      password: model.password,
    };

    return this.http.post<AccessTokenResponse>(this.url + '/login', body, {
      headers: headers,
      responseType: 'json',
    });
  }

  public Register(model: RegisterEntry) {
    return this.http.post<AccessTokenResponse>(this.url + '/register', model);
  }

  public Logout() {
    return this.http.get(this.url + '/logout').pipe(
      map((response) => {
        localStorage.removeItem('token');
        this.user.next(null);
      })
    );
  }
}
