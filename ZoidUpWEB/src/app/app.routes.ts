import { Routes } from '@angular/router';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { HomeComponent } from './components/home/home.component';
import { RegisterComponent } from './components/register/register.component';

export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', component: WelcomeComponent },
  { path: 'register', component: RegisterComponent },
];
