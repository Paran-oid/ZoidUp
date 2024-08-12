import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterEntry } from '../../models/other/register-entry.model';
import { mergeMap, of, throwError } from 'rxjs';
import { HttpErrorResponse, HttpResponseBase } from '@angular/common/http';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  message: string = '';
  hasSubmitted: boolean = false;
  isLoading: boolean = false;
  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(6)]],
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
    this.isLoading = true;
    this.hasSubmitted = true;
    if (this.form.invalid) {
      this.message =
        'All fields are required and must be at least 6 characters long.';
      this.isLoading = false;
      return;
    } else {
      const model: RegisterEntry = {
        username: this.form.get('username')?.value,
        password: this.form.get('password')?.value,
      };
      this.authService.Register(model).subscribe({
        next: (response) => {
          localStorage.setItem('token', response.token);
          this.router.navigate(['/home']);
        },
        error: (err: HttpErrorResponse) => {
          this.message = err.error;
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        },
      });
    }
  }
}
