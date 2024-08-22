import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/backend/auth.service';
import { NavigationStart, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { User } from '../../models/user/user.model';
import { PanelComponent } from './panel/panel.component';
import { ChatComponent } from './chat/chat.component';
import { AboutComponent } from './about/about.component';
import { RequestService } from '../../services/backend/request.service';
import { PassUserService } from '../../services/frontend/pass-user.service';
import { RequestsPopupComponent } from './requests-popup/requests-popup.component';
import { SendRequestsService } from '../../services/frontend/send-requests.service';
import { SpinnerComponent } from '../../shared/components/spinner/spinner.component';
import { CookieService } from 'ngx-cookie-service';
import { NotificationComponent } from '../../shared/components/notification/notification.component';
import { SpinnerService } from '../../services/frontend/spinner.service';
import { FriendshipService } from '../../services/backend/friendship.service';

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
    SpinnerComponent,
    NotificationComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  friends: User[] = [];
  currentUser: User | null = null;
  selectedUser: User | null = null;

  isInSendRequest: boolean = false;
  isHiddenAbout: boolean = true;
  hasCookie: boolean = false;

  constructor(
    private authService: AuthService,
    private RequestService: RequestService,
    private passUserService: PassUserService,
    private sendRequestsService: SendRequestsService,
    public cookieService: CookieService,
    private router: Router,
    public spinnerService: SpinnerService,
    private friendshipService: FriendshipService
  ) {}
  ngOnInit() {
    this.SetCurrentUser();
    this.passUserService.passedUser$.subscribe((selectedUser) => {
      this.selectedUser = selectedUser;
    });
    this.sendRequestsService.sendRequests$.subscribe((response) => {
      this.isInSendRequest = response;
    });
    this.passUserService.hiddenAbout$.subscribe((response) => {
      this.isHiddenAbout = response;
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
