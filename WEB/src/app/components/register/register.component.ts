import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/backend/auth.service';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterEntry } from '../../models/auth/register-entry.model';
import { mergeMap, of, throwError } from 'rxjs';
import { HttpErrorResponse, HttpResponseBase } from '@angular/common/http';
import { SpinnerComponent } from '../../shared/components/spinner/spinner.component';
import { NotificationService } from '../../services/frontend/notification.service';
import { SpinnerService } from '../../services/frontend/spinner.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, CommonModule, SpinnerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  message: string = '';
  hasSubmitted: boolean = false;
  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router,
    private notificationService: NotificationService,
    public spinnerService: SpinnerService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  get username() {
    return this.form.get('username');
  }

  get password() {
    return this.form.get('password');
  }

  Register() {
    this.spinnerService.isLoading.next(true);
    this.hasSubmitted = true;
    if (this.form.invalid) {
      this.message =
        'All fields are required and must be at least 6 characters long.';
      this.spinnerService.isLoading.next(false);
      return;
    } else {
      const model: RegisterEntry = {
        username: this.form.get('username')?.value,
        password: this.form.get('password')?.value,
      };
      this.authService.Register(model).subscribe({
        next: (response) => {
          this.notificationService.Success(
            'Your account was created! login now'
          );
          this.router.navigate(['']);
        },
        error: (err: HttpErrorResponse) => {
          this.message = err.error.message;
          this.form.reset();
          this.notificationService.Warning(this.message);
          this.spinnerService.isLoading.next(false);
        },
        complete: () => {
          this.spinnerService.isLoading.next(false);
        },
      });
    }
  }
}
