import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { Message } from '../../models/main/message.model';

@Injectable({
  providedIn: 'root',
})
export class ContextMenuService {
  public show: Subject<{ event: MouseEvent; obj: Message }> = new Subject<{
    event: MouseEvent;
    obj: Message;
  }>();
  constructor() {}
}
