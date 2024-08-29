import { Component, OnInit } from '@angular/core';
import { NavigationStart, Router, RouterOutlet } from '@angular/router';
import { AuthService } from './services/backend/auth.service';
import { SpinnerService } from './services/frontend/spinner.service';
import { CommonModule } from '@angular/common';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';
import { PassUserService } from './services/frontend/pass-user.service';
import { CookieService } from 'ngx-cookie-service';
import { NotificationService } from './services/frontend/notification.service';
import { NotificationComponent } from './shared/components/notification/notification.component';
import { FriendshipService } from './services/backend/friendship.service';
import { SignalrService } from './services/backend/signalr.service';
import { MessageService } from './services/backend/message.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    SpinnerComponent,
    NotificationComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers: [
    SignalrService,
    AuthService,
    PassUserService,
    CookieService,
    NotificationService,
    FriendshipService,
    MessageService,
  ],
})
export class AppComponent implements OnInit {
  constructor(
    public spinnerService: SpinnerService,
    public notificationService: NotificationService,
    private router: Router,
    private signalrService: SignalrService
  ) {}

  ngOnInit(): void {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        this.notificationService.isDisplayed.next(false);
      }
    });
    this.signalrService.StartConnection();
  }
}
