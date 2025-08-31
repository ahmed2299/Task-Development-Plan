import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoginService } from '../services/login.service';
import { TranslatePipe } from '../shared/pipes/translate.pipe';
import { LanguageService } from '../shared/services/language.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, TranslatePipe],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  
  // Profile properties
  currentUser: any = null;
  isLoading = false;
  errorMessage = '';

  constructor(
    private loginService: LoginService,
    private router: Router,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  // Load user profile data
  loadProfile(): void {
    this.isLoading = true;
    this.errorMessage = '';
    
    try {
      this.currentUser = this.loginService.getUserData();
      if (!this.currentUser) {
        this.errorMessage = this.languageService.translate('profile.noProfileData');
      }
      this.isLoading = false;
    } catch (error) {
      this.errorMessage = this.languageService.translate('profile.unableToLoad');
      this.isLoading = false;
      console.error('Error loading profile:', error);
    }
  }

  // Check if user is logged in
  isLoggedIn(): boolean {
    return this.loginService.isLoggedIn();
  }

  // Logout user
  logout() {
    this.loginService.removeToken();
    this.router.navigate(['/login']);
  }

  // Navigate back to home
  goToHome() {
    this.router.navigate(['/home']);
  }

  // Navigate to change password
  goToChangePassword() {
    this.router.navigate(['/change-password']);
  }

  // Format date for display
  formatDate(dateString: string): string {
    if (!dateString) return 'Not specified';
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    } catch {
      return 'Invalid date';
    }
  }

  // Get department name based on ID
  getDepartmentName(departmentId: number): string {
    const departments: { [key: number]: string } = {
      1: 'Information Technology',
      2: 'Human Resources',
      3: 'Finance',
      4: 'Marketing',
      5: 'Operations',
      6: 'Sales',
      7: 'Research & Development',
      8: 'Customer Support'
    };
    return departments[departmentId] || `Department ${departmentId}`;
  }
}
