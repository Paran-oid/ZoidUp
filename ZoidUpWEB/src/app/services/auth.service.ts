import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginEntry } from '../models/other/login-entry.model';
import { AccessTokenResponse } from '../models/other/access-token-response.model';
import { catchError, map, BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { User } from '../models/user/user.model';
import { RegisterEntry } from '../models/other/register-entry.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  url: string = environment.url;

  private userSubject!: BehaviorSubject<User | null>;
  private usersSubject!: BehaviorSubject<User[]>;
  public user!: Observable<User | null>;
  public users!: Observable<User[]>;
  constructor(private http: HttpClient) {
    const token: string | null = localStorage.getItem('token');

    this.userSubject = new BehaviorSubject<User | null>(null);
    this.usersSubject = new BehaviorSubject<User[]>([]);

    this.user = this.userSubject.asObservable();
    this.users = this.usersSubject.asObservable();
    if (token) {
      this.GetUser(token).subscribe({
        next: (response) => {
          this.userSubject.next(response);
          this.GetAllUsers().subscribe({
            next: (response) => {
              this.usersSubject.next(response);
            },
          });
        },
        error: (error) => {
          console.error(error);
          this.userSubject.next(null);
          alert("We couldn't log you in. going back to home page");
        },
      });
    }
  }

  //turn this to get all friends
  public GetAllUsers() {
    return this.http.get<User[]>(this.url + '/User/GetAllUsers');
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
    this.userSubject.next(null);
  }
}
