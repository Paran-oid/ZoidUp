import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { RequestUserDTO } from '../../models/user/user.model';
import { FriendshipService } from '../friendship.service';

@Injectable({
  providedIn: 'root',
})
export class SendRequestsService {
  sendRequests = new BehaviorSubject<boolean>(false);
  sendRequests$ = this.sendRequests.asObservable();

  request = new BehaviorSubject<RequestUserDTO | null>(null);
  request$ = this.request.asObservable();

  constructor(private friendshipService: FriendshipService) {}

  SeeSentRequests(userID: number) {
    this.friendshipService.GetAllSentRequests(userID).subscribe();
    this.sendRequests.next(true);
  }
  LeaveSendRequests() {
    this.sendRequests.next(false);
  }
}
