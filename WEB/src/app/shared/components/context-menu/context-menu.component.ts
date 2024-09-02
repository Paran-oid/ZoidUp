import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ContextMenuService } from '../../../services/frontend/context-menu.service';
import { Message } from '../../../models/main/message.model';
import { MessageService } from '../../../services/backend/message.service';
import { NotificationService } from '../../../services/frontend/notification.service';

@Component({
  selector: 'app-context-menu',
  standalone: true,
  imports: [CommonModule],
  host: {
    '(document:click)': 'ClickedOutside()',
  },
  templateUrl: './context-menu.component.html',
  styleUrl: './context-menu.component.scss',
})
export class ContextMenuComponent implements OnInit {
  clientWidth!: number;
  clientHeight!: number;
  maxWidth!: number;
  isOwnMessage: boolean = true;
  isShown: boolean = false;
  messageId!: number;

  @ViewChild('menu') ContextMenu!: ElementRef;
  private mouseLocation: { left?: number; top?: number } = { left: 0, top: 0 };

  constructor(
    public contextMenuService: ContextMenuService,
    private messageService: MessageService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.contextMenuService.show.subscribe((e) => {
      this.ShowMenu(e.event, e.messageId, e.isOwnMessage);
    });
  }

  get LocationCSS() {
    return {
      position: 'fixed',
      left: this.mouseLocation.left + 'px',
      top: this.mouseLocation.top + 'px',
      display: this.isShown ? 'block' : 'none',
    };
  }

  ClickedOutside() {
    this.isShown = false;
  }

  ShowMenu(event: MouseEvent, messageId: number, ownMessage: boolean) {
    this.isShown = true;
    this.clientWidth = window.innerWidth;
    this.clientHeight = window.innerHeight;
    this.maxWidth = this.clientWidth - 150;
    if (this.ContextMenu) {
      let contextMenu = this.ContextMenu.nativeElement as HTMLDivElement;
      this.mouseLocation = {
        left: event.clientX > this.maxWidth ? this.maxWidth : event.clientX,
        top:
          event.clientY >= innerHeight - contextMenu.offsetHeight
            ? innerHeight - contextMenu.offsetHeight
            : event.clientY,
      };
      console.log(this.mouseLocation);
      this.isOwnMessage = ownMessage;
      this.messageId = messageId;
    }
  }

  ReplyMessage() {}
  EditMessage(messageId: number) {
    this.messageService.editMessages.next(messageId);
  }
  DeleteMessage(messageId: number) {
    this.messageService.Delete(messageId).subscribe({
      next: (response) => {
        this.messageService.removedMessages.next(messageId);
        this.notificationService.Info(response);
      },
    });
  }
  RemoveMessage(messageId: number) {
    this.messageService.removedMessages.next(messageId);
    this.notificationService.Info('message was removed only for this session');
  }
  ForwardMessage() {}
}
