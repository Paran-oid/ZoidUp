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

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MessageDirective,
    ContextMenuComponent,
  ],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit {
  currentUser: User | null = null;
  friend: User | null = null;
  messages: Message[] = [];

  form: FormGroup = new FormGroup({});

  constructor(
    private fb: FormBuilder,
    private passUserService: PassUserService,
    private authService: AuthService,
    private messageService: MessageService
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
  ToggleAbout() {
    this.passUserService.ToggleAbout();
  }

  OnSubmit() {
    if (this.send?.value) {
      const model: CreateMessageDto = {
        senderId: this.currentUser?.id!,
        receiverId: this.friend?.id!,
        body: this.send.value,
      };
      this.messageService.SendMessage(model).subscribe((message) => {
        this.messages.push(message);
        this.form.reset();
      });
    }
  }
}
