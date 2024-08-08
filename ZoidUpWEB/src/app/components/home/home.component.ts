import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { User } from '../../models/user/user.model';
import { PanelComponent } from './panel/panel.component';
import { ChatComponent } from './chat/chat.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule, PanelComponent, ChatComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  friends: User[] = [];
  currentUser: User | null = null;

  constructor(public authService: AuthService) {}
  ngOnInit() {
    this.SetCurrentUser();
  }

  SetCurrentUser() {
    this.authService.user$.subscribe((user) => {
      this.currentUser = user;
      this.authService.users$.subscribe((users) => {
        this.friends = users;
        console.log(users);
      });
    });
  }

  Logout() {
    this.authService.Logout();
  }
}
// update commit
