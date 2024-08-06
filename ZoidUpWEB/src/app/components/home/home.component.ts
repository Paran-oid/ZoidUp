import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { SignalrService } from '../../services/signalr.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { User } from '../../models/user/user.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  users: User[] = [];
  CurrentUser: User | null = null;
  constructor(public authService: AuthService, private router: Router) {}
  ngOnInit() {
    if (!localStorage.getItem('token')) {
      this.router.navigate(['/']);
      return;
    }
    this.authService.user$.subscribe((user) => {
      this.CurrentUser = user;
    });

    this.authService.users$.subscribe((users) => {
      this.users = users;
      console.log(users);
    });
  }
}
