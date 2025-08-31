import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { Router } from '@angular/router';
import { LoginService } from './services/login.service';
import { LanguageService } from './shared/services/language.service';
import { LanguageSwitcherComponent } from './shared/components/language-switcher';
import { TranslatePipe } from './shared/pipes/translate.pipe';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive, LanguageSwitcherComponent, TranslatePipe],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'TDP';
  private tokenCheckSubscription?: Subscription;

  constructor(
    private loginService: LoginService,
    public router: Router,
    public languageService: LanguageService
  ) {
    // Debug: Log when component initializes
    console.log('AppComponent initialized');
    
    // Debug: Check if navigation bar element exists
    setTimeout(() => {
      const navbar = document.getElementById('mainNavbar');
      console.log('Navigation bar element:', navbar);
      if (navbar) {
        console.log('Navigation bar styles:', window.getComputedStyle(navbar));
      }
    }, 1000);
  }

  ngOnInit(): void {
    // Start periodic token check every 30 seconds
    this.startTokenCheck();
  }

  ngOnDestroy(): void {
    // Clean up subscription when component is destroyed
    if (this.tokenCheckSubscription) {
      this.tokenCheckSubscription.unsubscribe();
    }
  }

  /**
   * Start periodic token validation check
   */
  private startTokenCheck(): void {
    // Check token every 30 seconds (30000ms)
    this.tokenCheckSubscription = interval(30000).subscribe(() => {
      if (this.loginService.isLoggedIn()) {
        // Check if token is expiring soon
        if (this.loginService.isTokenExpiringSoon()) {
          console.warn('Token validation check: Token expires soon');
          // You could show a toast notification here
        } else {
          console.log('Token validation check: Token is valid');
        }
      } else {
        // Token has expired, redirect to login
        console.log('Token validation check: Token expired, redirecting to login');
        this.router.navigate(['/login']);
      }
    });
  }

  // Check if user is logged in
  isLoggedIn(): boolean {
    const status = this.loginService.isLoggedIn();
    console.log('Navigation - User logged in:', status);
    return status;
  }

  // Logout user
  logout() {
    console.log('Navigation - Logging out user');
    this.loginService.removeToken();
    this.router.navigate(['/login']);
  }
}
