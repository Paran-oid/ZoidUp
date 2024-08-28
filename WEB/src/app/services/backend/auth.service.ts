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
import { User } from '../../models/main/user.model';
import { RegisterEntry } from '../../models/auth/register-entry.model';
import { Router } from '@angular/router';
import { SignalrService } from './signalr.service';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  url: string = 'api/auth';

  private user = new BehaviorSubject<User | null>(null);
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
  }

  public GetUser(token: string) {
    return this.http.get<User>(this.url).pipe(
      map((user) => {
        this.user.next(user);
        //apparently we have to parseint here
        const userId: number = parseInt(user.id.toString());
        if (
          this.signalrService.hubConnection.state ==
          HubConnectionState.Connected
        ) {
          this.signalrService.Reauthenticate(userId);
        } else {
          this.signalrService.signalrSession$.subscribe((object: any) => {
            if (object.type === HubConnectionState.Connected) {
              this.signalrService.Reauthenticate(userId);
            }
          });
        }
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
