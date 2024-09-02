import { CommonModule } from '@angular/common';
import { Component, HostListener, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { PassUserService } from '../../../services/frontend/pass-user.service';
import { User } from '../../../models/main/user.model';
import { AuthService } from '../../../services/backend/auth.service';
import { CreateMessageDto, Message } from '../../../models/main/message.model';
import { MessageService } from '../../../services/backend/message.service';
import { MessageDirective } from '../../../shared/directives/message.directive';
import { ContextMenuComponent } from '../../../shared/components/context-menu/context-menu.component';
import { MessageInputDirective } from '../../../shared/directives/message-input.directive';
import { NotificationService } from '../../../services/frontend/notification.service';
import { SignalrService } from '../../../services/backend/signalr.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MessageDirective,
    ContextMenuComponent,
    MessageInputDirective,
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit {
  currentUser: User | null = null;
  friend: User | null = null;
  messages: Message[] = [];
  isSending: boolean = false;

  form: FormGroup = new FormGroup({});

  constructor(
    private fb: FormBuilder,
    private passUserService: PassUserService,
    private authService: AuthService,
    private messageService: MessageService,
    private notificationService: NotificationService,
    private signalrService: SignalrService
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      send: [''],
    });
    this.passUserService.passedUser$.subscribe((friend) => {
      this.friend = friend;
    });
    this.authService.user$.subscribe((user) => {
      this.currentUser = user;
    });
    this.messageService.messages$.subscribe((messages) => {
      this.messages = messages!;
    });

    //message service messages subscriptions
    this.messageService.removedMessages$.subscribe((messageId) => {
      if (this.messages) {
        const index = this.messages.findIndex((m) => m.id == messageId);
        this.messages.splice(index, 1);
      }
    });

    this.messageService.editMessages$.subscribe((messageId) => {
      if (this.messages) {
        const item = this.messages.find((m) => m.id == messageId);
        if (item) {
          this.EditMessage(item);
        } else {
          this.notificationService.Warning("couldn't find the message");
        }
      }
    });
  }

  get send() {
    return this.form.get('send');
  }

  @HostListener('keydown', ['$event']) ListenForSubmit(event: KeyboardEvent) {
    if (event.key === 'enter' && this.form) {
      this.OnSubmit();
      return;
    }
    return;
  }
  EditMessage(message: Message) {
    //     make an is editing boolean
    // follow this structure
    //     <div *ngFor="let message of messages; let i = index">
    //   <div *ngIf="!isEditing[i]; else editTemplate">
    //     <div id="message-body">{{ message.body }}</div>
    //     <button (click)="editMessage(i)">Edit</button>
    //   </div>
    //   <ng-template #editTemplate>
    //     <input [(ngModel)]="message.body" />
    //     <button (click)="saveMessage(i)">Save</button>
    //   </ng-template>
    // </div>
    // we will add input element
    // if user clicks outside either save or discard changes
  }
  ToggleAbout() {
    this.passUserService.ToggleAbout();
  }
  OnSubmit(event?: KeyboardEvent) {
    if ((!event || event.key == 'Enter') && this.isSending == false) {
      if (this.send?.value) {
        this.isSending = true;
        let body: string = (this.send.value as string).trim();
        const model: CreateMessageDto = {
          body: body,
          senderId: parseInt(this.currentUser?.id!.toString()!),
          receiverId: this.friend?.id!,
        };
        this.messageService.SendMessage(model).subscribe((message) => {
          this.signalrService.SendMessage(message);
          this.messages.push(message);
          this.form.reset();
          this.isSending = false;
        });
      }
    }
  }
}
