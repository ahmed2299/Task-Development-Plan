import 'zone.js';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AppComponent } from './app/app.component';
import { HomeComponent } from './app/home/home.component';
import { RegisterComponent } from './app/register/register.component';
import { LoginComponent } from './app/login/login.component';
import { ProfileComponent } from './app/profile/profile.component';
import { ChangePasswordComponent } from './app/change-password/change-password.component';
import { AuthGuard } from './app/guards/auth.guard';
import { PublicGuard } from './app/guards/public.guard';
import { AuthInterceptor } from './app/interceptors/auth.interceptor';

const routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' as const },
  { path: 'login', component: LoginComponent, canActivate: [PublicGuard] },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [PublicGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'change-password', component: ChangePasswordComponent, canActivate: [AuthGuard] }
];

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    { provide: AuthInterceptor, useClass: AuthInterceptor }
  ]
}).catch(err => console.error(err));
