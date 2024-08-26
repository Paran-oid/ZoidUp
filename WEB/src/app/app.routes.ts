import { Routes, CanDeactivateFn } from '@angular/router';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';
import { authGuard } from './guards/auth.guard';
import { formGuard } from './guards/form.guard';

export const routes: Routes = [
  { path: 'home', component: HomeComponent, canActivate: [authGuard] },
  { path: '', component: WelcomeComponent },
  {
    path: 'register',
    component: RegisterComponent,
    canDeactivate: [formGuard],
  },
];
