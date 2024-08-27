import { Component, OnInit, Signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/backend/auth.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoginEntry } from '../../models/auth/login-entry.model';
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
import { NotificationService } from '../../services/frontend/notification.service';
import { SpinnerService } from '../../services/frontend/spinner.service';
import { SignalrService } from '../../services/backend/signalr.service';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { User } from '../../models/user/user.model';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink, SpinnerComponent],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.scss',
})
export class WelcomeComponent implements OnInit {
  isHidden: boolean = false;
  isSubmitted: boolean = false;
  message = '';
  form: FormGroup = new FormGroup({});
  constructor(
    private spinnerService: SpinnerService,
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private cookieService: CookieService,
    private notificationService: NotificationService,
    private signalrService: SignalrService
  ) {}

  ngOnInit(): void {
    if (localStorage.getItem('token') || sessionStorage.getItem('token')) {
      this.router.navigate(['/home']);
    }

    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      remember: [false],
    });
  }

  get username() {
    return this.form.get('username');
  }

  get password() {
    return this.form.get('password');
  }

  get remember() {
    return this.form.get('remember');
  }

  Login() {
    this.isSubmitted = true;
    this.spinnerService.isLoading.next(true);
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
            if (this.remember?.value) {
              localStorage.setItem('token', response.token);
            } else {
              sessionStorage.setItem('token', response.token);
            }
            this.cookieService.set('first_time', 'true');
            const decoded: any = jwtDecode<JwtPayload>(response.token);
            this.signalrService.Authenticate(parseInt(decoded.Id));
          }, 3000);
          this.spinnerService.isLoading.next(false);
        },
        error: (err: HttpErrorResponse) => {
          this.message = err.error.message;
          this.notificationService.Warning(this.message);
          this.form.reset();
          this.spinnerService.isLoading.next(false);
        },
      });
    }
  }
}
