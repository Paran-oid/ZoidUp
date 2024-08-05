import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { SpinnerComponent } from '../../shared/spinner/spinner.component';
import { SignalrService } from '../../services/signalr.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { User } from '../../models/user/user.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SpinnerComponent, FormsModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit, OnDestroy {
  user: User | null = null;
  users!: User[];
  text: string = '';
  selectedUser: string | null = null;

  messages: string[] = [];
  conversations: { user: string; message: string }[] = [];
  privateConversations: { user: string; message: string }[] = [];

  constructor(
    private authService: AuthService,
    private router: Router,
    private signalrService: SignalrService
  ) {}
  ngOnInit() {
    if (!localStorage.getItem('token')) {
      this.router.navigate(['/']);
      return;
    }
    this.authService.user.subscribe((user: User | null) => {
      this.user = user;
    });

    this.signalrService.MessageReceived.subscribe(
      (conversation: { user: string; message: string }) => {
        this.conversations.push(conversation);
      }
    );

    this.signalrService.HasJoined.subscribe((message) => {
      this.messages.push(message);
      this.RemoveMessageAfter(message);
    });

    this.signalrService.ReceivePrivateMessage.subscribe(
      (conversation: { user: string; message: string }) => {
        console.log(conversation);
        this.privateConversations.push(conversation);
      }
    );

    this.authService.users.subscribe((users: User[]) => {
      this.users = users;
    });

    this.signalrService.StartConnection();
  }

  get otherUsers() {
    return this.users.filter((user) => user.username !== this.user!.username);
  }

  RemoveMessageAfter(message: string) {
    setTimeout(() => {
      this.messages.splice(this.messages.indexOf(message), 1);
    }, 10000);
  }

  SignOut() {
    this.authService.Logout();
    window.location.reload();
  }

  SendMessage() {
    console.log(this.selectedUser);
    if (this.selectedUser == null) {
      this.signalrService.SendMessageToAll(this.user!.username, this.text);
    } else {
      //make that work
      this.signalrService.SendMessageToUser(this.selectedUser, this.text);
    }
  }

  ngOnDestroy() {
    this.signalrService.hubConnection.stop();
  }
}
