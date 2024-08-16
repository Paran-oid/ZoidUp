import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RequestUserDTO, User } from '../models/user/user.model';
import { BehaviorSubject, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FriendshipService {
  url: string = environment.url + '/Request';

  sentRequests: BehaviorSubject<RequestUserDTO[] | null> = new BehaviorSubject<
    RequestUserDTO[] | null
  >(null);
  sentRequests$ = this.sentRequests.asObservable();

  constructor(private http: HttpClient) {}

  public HasRequests(userID: number) {
    return this.http.get<boolean>(this.url + '/HasRequests/' + userID, {});
  }
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
      this.url + `/SendRequest?senderID=${senderID}&receiverID=${receiverID}`,
      { responseType: 'text' }
    );
  }
  public UnsendRequest(senderID: number, receiverID: number) {
    return this.http.delete(
      this.url + `/UnsendRequest?senderID=${senderID}&receiverID=${receiverID}`,
      { responseType: 'text' }
    );
  }
  public GetAllReceivedRequests(receiverID: number) {
    return this.http.get<User[]>(
      this.url + '/GetAllReceivedRequests/' + receiverID
    );
  }
  public GetAllSentRequests(senderID: number) {
    return this.http
      .get<RequestUserDTO[]>(this.url + '/GetAllSentRequests/' + senderID)
      .pipe(
        map((requests) => {
          console.log(requests);
          this.sentRequests.next(requests);
        })
      );
  }
}
