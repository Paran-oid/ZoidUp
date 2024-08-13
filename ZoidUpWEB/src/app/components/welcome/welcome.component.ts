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

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink, SpinnerComponent],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.scss',
})
export class WelcomeComponent implements OnInit {
  isLoading: boolean = false;
  isSubmitted: boolean = false;
  message = '';
  test: string = '';
  form: FormGroup = new FormGroup({});
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
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
    this.isLoading = true;
    this.isSubmitted = true;
    if (this.form.invalid) {
      this.message = 'Please enter the following fields';
      this.isLoading = false;
      return;
    } else {
      const model: LoginEntry = {
        username: this.form.get('username')?.value,
        password: this.form.get('password')?.value,
      };
      this.authService.Login(model).subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);
          window.location.reload();
        },
        error: (err: HttpErrorResponse) => {
          this.message = err.error;
          this.form.reset();
          this.isLoading = false;
        },
      });
    }
  }
}
