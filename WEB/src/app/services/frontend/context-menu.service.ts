import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { Message } from '../../models/main/message.model';

@Injectable({
  providedIn: 'root',
})
export class ContextMenuService {
  public show = new Subject<{
    event: MouseEvent;
    messageId: number;
    isOwnMessage: boolean;
  }>();
  constructor() {}
}
