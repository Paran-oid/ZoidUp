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
import { SignalrService } from './signalr.service';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  url: string = 'api/auth';

  //subject gets data
  private user = new BehaviorSubject<User | null>(null);

  //observer reads data
  public user$ = this.user.asObservable();

  constructor(
    private http: HttpClient,
    private signalrService: SignalrService,
    private router: Router
  ) {
    let token =
      localStorage.getItem('token') || sessionStorage.getItem('token');
    if (token) {
      this.GetUser(token).subscribe();
    }

    //listeners
    this.signalrService.AuthenticateListener();
  }

  public GetUser(token: string) {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<User>(this.url, { headers: headers }).pipe(
      map((user) => {
        this.user.next(user);
      }),
      catchError((error) => {
        localStorage.removeItem('token');
        sessionStorage.removeItem('token');
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
    localStorage.removeItem('token');
    sessionStorage.removeItem('token');
    this.user.next(null);
    window.location.reload();
  }
}
