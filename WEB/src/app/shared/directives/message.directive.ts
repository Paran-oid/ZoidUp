import {
  Directive,
  ElementRef,
  HostListener,
  Input,
  Renderer2,
} from '@angular/core';
import { Message } from '../../models/main/message.model';
import { ContextMenuService } from '../../services/frontend/context-menu.service';

@Directive({
  selector: '[appMessage]',
  standalone: true,
})
export class MessageDirective {
  @Input('message') message!: Message;
  constructor(private contextMenuService: ContextMenuService) {}
  @HostListener('contextmenu', ['$event']) DisplayContextMenu(
    event: MouseEvent
  ) {
    this.contextMenuService.show.next({ event: event, obj: this.message });
    event.preventDefault();
  }
}
