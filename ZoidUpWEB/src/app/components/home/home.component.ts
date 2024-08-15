import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { User } from '../../models/user/user.model';
import { PanelComponent } from './panel/panel.component';
import { ChatComponent } from './chat/chat.component';
import { AboutComponent } from './about/about.component';
import { FriendshipService } from '../../services/friendship.service';
import { PassUserService } from '../../services/frontend/pass-user.service';
import { RequestsPopupComponent } from './requests-popup/requests-popup.component';
import { SendRequestsService } from '../../services/frontend/send-requests.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    PanelComponent,
    ChatComponent,
    AboutComponent,
    RequestsPopupComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  friends: User[] = [];
  currentUser: User | null = null;
  selectedUser: User | null = null;

  isInSendRequest: boolean = false;

  constructor(
    private authService: AuthService,
    private friendshipService: FriendshipService,
    private passUserService: PassUserService,
    private sendRequestsService: SendRequestsService
  ) {}
  ngOnInit() {
    this.SetCurrentUser();
    this.passUserService.passedUser$.subscribe((selectedUser) => {
      this.selectedUser = selectedUser;
    });
    this.sendRequestsService.sendRequests$.subscribe((response) => {
      this.isInSendRequest = response;
    });
  }

  SetCurrentUser() {
    this.authService.user$.subscribe((user) => {
      this.currentUser = user;
      if (user) {
        this.friendshipService.GetFriends(user?.id!).subscribe((friends) => {
          this.friends = friends;
        });
      }
    });
  }

  Logout() {
    this.authService.Logout();
  }
}
// update commit
