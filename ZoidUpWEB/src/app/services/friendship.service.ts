import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user/user.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FriendshipService {
  url: string = environment.url + '/Request';

  constructor(private http: HttpClient) {}

  public GetAllRecommendedFriends(userID: number) {
    return this.http.get<User[]>(
      this.url + '/GetAllRecommendedFriends/' + userID
    );
  }
  public GetFriends(userID: number) {
    return this.http.get<User[]>(this.url + '/GetAllFriends/' + userID);
  }
}
