import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LoginService } from '../services/login.service';

@Injectable({
  providedIn: 'root'
})
export class PublicGuard implements CanActivate {

  constructor(
    private loginService: LoginService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (this.loginService.isLoggedIn()) {
      // User is logged in, redirect to home page
      this.router.navigate(['/home']);
      return false;
    } else {
      // User is not logged in, allow access to public routes
      return true;
    }
  }
}
