import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SpinnerService {
  requestCount: number = 0;

  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  isLoading = this.isLoadingSubject.asObservable();

  constructor() {}

  Loading() {
    this.requestCount++;
    this.isLoadingSubject.next(true);
  }

  Inactive() {
    this.requestCount--;
    if (this.requestCount <= 0) {
      this.requestCount = 0;
      this.isLoadingSubject.next(false);
    }
  }
}
