import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ContextMenuService } from '../../../services/frontend/context-menu.service';
import { Message } from '../../../models/main/message.model';

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
export class ContextMenuComponent {
  isShown: boolean = false;
  items: string[] = ['edit', 'delete'];
  private mouseLocation: { left: number; top: number } = { left: 0, top: 0 };

  constructor(public contextMenuService: ContextMenuService) {
    contextMenuService.show.subscribe((e) => {
      this.ShowMenu(e.event, e.obj);
    });
  }

  get LocationCSS() {
    return {
      position: 'fixed',
      left: this.mouseLocation.left + 'px',
      top: this.mouseLocation.top + 'px',
    };
  }

  ClickedOutside() {
    this.isShown = false;
  }

  ShowMenu(event: MouseEvent, message: Message) {
    this.isShown = true;
    this.items = ['edit', 'delete'];
    this.mouseLocation = {
      left: event.clientX,
      top: event.clientY,
    };
  }
}
