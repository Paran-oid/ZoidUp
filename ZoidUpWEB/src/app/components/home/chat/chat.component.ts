import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent {
  model: any = { username: 'Yasmin', status: 'offline since 3 days ago' };
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
      body: 'We should hangout!',
      userId: 1,
    },
  ];
  currentUser: any = { id: 1, username: 'Aziz' };
}
