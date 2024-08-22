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
import { environment } from '../../../environments/environment.development';
import { User } from '../../models/user/user.model';
import { RegisterEntry } from '../../models/auth/register-entry.model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  url: string = environment.url + '/auth';

  //subject gets data
  private user = new BehaviorSubject<User | null>(null);

  //observer reads data
  public user$ = this.user.asObservable();

  constructor(private http: HttpClient, private router: Router) {
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
        console.error(error);
        alert("We couldn't log you in. Going back to the home page");

        localStorage.removeItem('token');
        this.user.next(null);
        this.router.navigate(['/']);
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
    //make an actual endpoint here
    localStorage.removeItem('token');
    this.user.next(null);
    this.router.navigate(['/']);
  }
}
