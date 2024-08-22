import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../../models/user/user.model';

@Injectable({
  providedIn: 'root',
})
export class FriendshipService {
  private readonly url = 'api/friends';
  constructor(private http: HttpClient) {}

  public GetFriends(userId: number) {
    return this.http.get<User[]>(this.url + '/' + userId);
  }
  public RemoveFriend(userId: number, friendId: number) {
    return this.http.delete(this.url + `/${userId}/${friendId}`, {
      responseType: 'text',
    });
  }
}
