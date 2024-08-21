import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from '../../models/user/user.model';

@Injectable({
  providedIn: 'root',
})
export class PassUserService {
  passedUser = new BehaviorSubject<User | null>(null);
  passedUser$ = this.passedUser.asObservable();

  hiddenAboutValue = true;
  hiddenAbout = new BehaviorSubject<boolean>(true);
  hiddenAbout$ = this.hiddenAbout.asObservable();
  constructor() {}

  ToggleAbout() {
    console.log(`before : ${this.hiddenAboutValue}`);
    this.hiddenAboutValue = !this.hiddenAboutValue;
    console.log(`after : ${this.hiddenAboutValue}`);
    this.hiddenAbout.next(this.hiddenAboutValue);
  }
}
