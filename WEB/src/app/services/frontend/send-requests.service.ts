import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { RequestUserDTO } from '../../models/main/user.model';
import { RequestService } from '../backend/request.service';

@Injectable({
  providedIn: 'root',
})
export class SendRequestsService {
  sendRequests = new BehaviorSubject<boolean>(false);
  sendRequests$ = this.sendRequests.asObservable();

  request = new BehaviorSubject<RequestUserDTO | null>(null);
  request$ = this.request.asObservable();

  constructor(private RequestService: RequestService) {}

  SeeSentRequests(userId: number) {
    this.RequestService.GetAllSentRequests(userId).subscribe();
    this.sendRequests.next(true);
  }
  LeaveSendRequests() {
    this.sendRequests.next(false);
  }
}
