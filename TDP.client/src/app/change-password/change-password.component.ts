import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../services/login.service';
import { TranslatePipe } from '../shared/pipes/translate.pipe';
import { LanguageService } from '../shared/services/language.service';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslatePipe],
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  
  // Form properties
  changePasswordForm: FormGroup;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';
  
  // Current user properties
  currentUser: any = null;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private router: Router,
    private languageService: LanguageService
  ) {
    this.changePasswordForm = this.fb.group({
      currentPassword: ['', [Validators.required, Validators.minLength(6)]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    }, { validators: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    this.getCurrentUser();
  }

  // Get current logged in user
  getCurrentUser(): void {
    this.currentUser = this.loginService.getUserData();
    if (!this.currentUser) {
      this.router.navigate(['/login']);
    }
  }

  // Check if user is logged in
  isLoggedIn(): boolean {
    return this.loginService.isLoggedIn();
  }

  // Custom validator for password confirmation
  passwordMatchValidator(form: FormGroup) {
    const newPassword = form.get('newPassword')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    
    if (newPassword === confirmPassword) {
      return null;
    } else {
      return { passwordMismatch: true };
    }
  }

  // Get form controls for easy access in template
  get f() {
    return this.changePasswordForm.controls;
  }

  // Handle form submission
  onSubmit(): void {
    if (this.changePasswordForm.valid) {
      this.isSubmitting = true;
      this.successMessage = '';
      this.errorMessage = '';

      const formData = this.changePasswordForm.value;
      
      // Check if current password matches (basic validation)
      if (formData.currentPassword === formData.newPassword) {
        this.errorMessage = this.languageService.translate('changePassword.passwordMismatchError');
        this.isSubmitting = false;
        return;
      }

      // Call backend API to change password
      const changePasswordData = {
        currentPassword: formData.currentPassword,
        newPassword: formData.newPassword,
        confirmPassword: formData.confirmPassword
      };

      this.loginService.changePassword(changePasswordData).subscribe({
        next: (response: any) => {
          this.isSubmitting = false;
          if (response.success) {
            this.successMessage = response.message || 'Password changed successfully!';
            this.changePasswordForm.reset();
            
            // Clear success message after 3 seconds
            setTimeout(() => {
              this.successMessage = '';
            }, 3000);
          } else {
            this.errorMessage = response.message || 'Failed to change password. Please try again.';
            setTimeout(() => {
              this.errorMessage = '';
            }, 5000);
          }
        },
        error: (error) => {
          this.isSubmitting = false;
          if (error.error && error.error.message) {
            this.errorMessage = error.error.message;
          } else if (error.message) {
            this.errorMessage = error.message;
          } else {
            this.errorMessage = 'Network error. Please check your connection and try again.';
          }
          setTimeout(() => {
            this.errorMessage = '';
          }, 5000);
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  // Mark all form controls as touched to show validation errors
  markFormGroupTouched(): void {
    Object.keys(this.changePasswordForm.controls).forEach(key => {
      const control = this.changePasswordForm.get(key);
      control?.markAsTouched();
    });
  }

  // Navigate back to profile
  goToProfile(): void {
    this.router.navigate(['/profile']);
  }

  // Navigate to home
  goToHome(): void {
    this.router.navigate(['/home']);
  }

  // Logout user
  logout(): void {
    this.loginService.removeToken();
    this.router.navigate(['/login']);
  }

  // Clear form
  clearForm(): void {
    this.changePasswordForm.reset();
    this.successMessage = '';
    this.errorMessage = '';
  }

  // Check if passwords match for real-time validation
  checkPasswordMatch(): boolean {
    const newPassword = this.changePasswordForm.get('newPassword')?.value;
    const confirmPassword = this.changePasswordForm.get('confirmPassword')?.value;
    return newPassword === confirmPassword && newPassword !== '';
  }
}
