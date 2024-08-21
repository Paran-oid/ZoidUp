import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoginEntry } from '../../models/other/login-entry.model';
import {
  BehaviorSubject,
  catchError,
  finalize,
  map,
  mergeMap,
  Observable,
  of,
  throwError,
} from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { SpinnerComponent } from '../../shared/components/spinner/spinner.component';
import { CookieService } from 'ngx-cookie-service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink, SpinnerComponent],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.scss',
})
export class WelcomeComponent implements OnInit {
  isSubmitted: boolean = false;
  message = '';
  test: string = '';
  form: FormGroup = new FormGroup({});
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private cookieService: CookieService,
    private notificationService: NotificationService
  ) {}

  get username() {
    return this.form.get('username');
  }

  get password() {
    return this.form.get('password');
  }

  ngOnInit(): void {
    if (localStorage.getItem('token')) {
      this.router.navigate(['/home']);
    }

    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(6)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  Login() {
    this.isSubmitted = true;
    if (this.form.invalid) {
      this.message = 'Please enter the following fields';
      return;
    } else {
      const model: LoginEntry = {
        username: this.form.get('username')?.value,
        password: this.form.get('password')?.value,
      };
      this.authService.Login(model).subscribe({
        next: (response) => {
          this.notificationService.Success(
            `Welcome back ${this.username?.value}`
          );
          setTimeout(() => {
            localStorage.setItem('token', response.token);
            this.cookieService.set('first_time', 'true');
            window.location.reload();
          }, 2000);
        },
        error: (err: HttpErrorResponse) => {
          this.message = err.error;
          this.form.reset();
        },
      });
    }
  }
}
