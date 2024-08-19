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
    title: 'default',
    type: 'Info',
    description: 'hello there',
  });
  model$ = this.model.asObservable();

  constructor() {}

  Display() {
    this.isDisplayed.next(true);
    setTimeout(() => {
      this.isDisplayed.next(false);
    }, 5000);
  }

  Info(description: string) {
    this.Display();
    const model: NotificationModel = {
      title: 'Info',
      description: 'template',
      type: 'Info',
    };
    this.model.next(model);
  }

  Warning(description: string) {
    this.Display();
    const model: NotificationModel = {
      title: 'Warning',
      description: 'template',
      type: 'Warning',
    };
    this.model.next(model);
  }
}
