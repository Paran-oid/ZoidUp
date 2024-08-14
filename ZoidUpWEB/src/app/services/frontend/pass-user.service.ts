import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from '../../models/user/user.model';

@Injectable({
  providedIn: 'root',
})
export class PassUserService {
  passedUser = new BehaviorSubject<User | null>(null);
  passedUser$ = this.passedUser.asObservable();
  constructor() {}
}
