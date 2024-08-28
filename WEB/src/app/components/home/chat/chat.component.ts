import { CommonModule } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { PassUserService } from '../../../services/frontend/pass-user.service';
import { User } from '../../../models/main/user.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit {
  currentUser: any = { id: 1, username: 'Aziz' };
  friend: User | null = null;

  sentMessageForm: FormGroup = new FormGroup({});

  messages: any[] = [
    { id: 1, userName: 'Aziz', body: 'hello how are you doing?', userId: 1 },
    {
      id: 2,
      userName: 'Yasmine',
      body: 'I am doing good what about you?',
      userId: 2,
    },
    {
      id: 3,
      userName: 'Yasmine',
      body: 'Loving the weather today!',
      userId: 2,
    },
    {
      id: 4,
      userName: 'Aziz',
      body: 'We should watch a movie together at 9pm!',
      userId: 1,
    },
    {
      id: 5,
      userName: 'Yasmine',
      body: 'Sure thing, see you then!',
      userId: 2,
    },
  ];

  constructor(
    private fb: FormBuilder,
    private passUserService: PassUserService
  ) {}

  ngOnInit() {
    this.sentMessageForm = this.fb.group({
      send: [''],
    });
    this.passUserService.passedUser$.subscribe((friend) => {
      this.friend = friend;
    });
  }

  get send() {
    return this.sentMessageForm.get('send')?.value;
  }

  @HostListener('keydown', ['$event']) ListenForSubmit(event: KeyboardEvent) {
    if (event.key === 'enter' && this.sentMessageForm) {
      this.OnSubmit();
      return;
    }
    return;
  }
  ToggleAbout() {
    this.passUserService.ToggleAbout();
  }

  OnSubmit() {}
}
