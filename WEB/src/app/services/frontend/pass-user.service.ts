import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from '../../models/main/user.model';

@Injectable({
  providedIn: 'root',
})
export class PassUserService {
  passedUser = new BehaviorSubject<User | null>(null);
  passedUser$ = this.passedUser.asObservable();

  hiddenAboutValue = true;
  hiddenAbout = new BehaviorSubject<boolean>(true);
  hiddenAbout$ = this.hiddenAbout.asObservable();

  passState = new BehaviorSubject<string>('');
  passState$ = this.passState.asObservable();

  constructor() {}

  ToggleAbout() {
    this.hiddenAboutValue = !this.hiddenAboutValue;

    this.hiddenAbout.next(this.hiddenAboutValue);
  }
}
