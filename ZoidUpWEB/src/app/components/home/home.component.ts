import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { SpinnerComponent } from '../../shared/spinner/spinner.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SpinnerComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}
  ngOnInit() {
    if (!localStorage.getItem('token')) {
      this.router.navigate(['/']);
      return;
    }
  }
  SignOut() {
    this.authService.Logout();
    window.location.reload();
  }
}
