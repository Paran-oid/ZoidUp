import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RequestUserDTO, User } from '../../models/user/user.model';
import { BehaviorSubject, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RequestService {
  url: string = environment.url + '/requests';

  sentRequests: BehaviorSubject<RequestUserDTO[] | null> = new BehaviorSubject<
    RequestUserDTO[] | null
  >(null);
  sentRequests$ = this.sentRequests.asObservable();

  constructor(private http: HttpClient) {}

  public HasRequests(userId: number) {
    return this.http.get<boolean>(this.url + '/has/' + userId, {});
  }
  public GetAllRecommendedFriends(userId: number) {
    return this.http.get<User[]>(this.url + '/recommendations/' + userId);
  }

  public SendRequest(senderId: number, receiverId: number) {
    const body = {
      senderId: senderId,
      receiverId: receiverId,
    };
    return this.http.post(this.url, body, {
      responseType: 'text',
    });
  }
  public UnsendRequest(senderId: number, receiverId: number) {
    const body = {
      senderId: senderId,
      receiverId: receiverId,
    };
    return this.http.delete(this.url, { responseType: 'text' });
  }
  public GetAllReceivedRequests(receiverId: number) {
    return this.http.get<User[]>(this.url + '/' + receiverId);
  }
  public GetAllSentRequests(senderId: number) {
    return this.http.get<RequestUserDTO[]>(this.url + '/sent/' + senderId).pipe(
      map((requests) => {
        this.sentRequests.next(requests);
      })
    );
  }
}
