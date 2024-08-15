import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { RequestUserDTO } from '../../models/user/user.model';

@Injectable({
  providedIn: 'root',
})
export class SendRequestsService {
  sendRequests = new BehaviorSubject<boolean>(false);
  sendRequests$ = this.sendRequests.asObservable();

  request = new BehaviorSubject<RequestUserDTO | null>(null);
  request$ = this.request.asObservable();

  constructor() {}

  SeeSendRequests() {
    this.sendRequests.next(true);
  }
  LeaveSendRequests() {
    this.sendRequests.next(false);
  }
  FetchData()
  {
    use it here and find a solution for the user problem
  }
}
