import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NotificationModel } from '../models/other/notification.model';
import { Title } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  isDisplayed = new BehaviorSubject<boolean>(false);
  isDisplayed$ = this.isDisplayed.asObservable();

  model = new BehaviorSubject<NotificationModel>({
    title: 'CYKA BLYAT',
    type: 'Info',
  });
  model$ = this.model.asObservable();

  constructor() {}

  Display() {
    this.isDisplayed.next(true);
    setTimeout(() => {
      this.isDisplayed.next(false);
    }, 5000);
  }

  Info(title: string) {
    this.Display();
    const model: NotificationModel = {
      title: title,
      type: 'Info',
    };
    this.model.next(model);
  }

  Warning(title: string) {
    this.Display();
    const model: NotificationModel = {
      title: title,
      type: 'Warning',
    };
    this.model.next(model);
  }

  Success(title: string) {
    this.Display();
    const model: NotificationModel = {
      title: title,
      type: 'Success',
    };
    this.model.next(model);
  }
}
