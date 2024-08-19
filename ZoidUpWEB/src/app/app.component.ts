import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { SpinnerService } from './services/spinner.service';
import { CommonModule } from '@angular/common';
import { SpinnerComponent } from './shared/components/spinner/spinner.component';
import { PassUserService } from './services/frontend/pass-user.service';
import { CookieService } from 'ngx-cookie-service';
import { NotificationService } from './services/notification.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, SpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers: [AuthService, PassUserService, CookieService, NotificationService],
})
export class AppComponent implements OnInit {
  constructor(public spinnerService: SpinnerService) {}

  ngOnInit(): void {}
}
