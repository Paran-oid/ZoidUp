import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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
  public SendRequest(senderID: number, receiverID: number) {
    return this.http.get(
      this.url + `/SendRequest?receiverID=${receiverID}&senderID=${senderID}`,
      { responseType: 'text' }
    );
  }
  public GetAllReceivedRequests(receiverID: number) {
    return this.http.get<User[]>(
      this.url + '/GetAllReceivedRequests/' + receiverID
    );
  }
  public GetAllSentRequests(senderID: number) {
    return this.http.get<User[]>(this.url + '/GetAllSentRequests/' + senderID);
  }
}
