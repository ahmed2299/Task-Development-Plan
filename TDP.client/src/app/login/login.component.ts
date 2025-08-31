import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService, LoginRequest, LoginResponse } from '../services/login.service';
import { TranslatePipe } from '../shared/pipes/translate.pipe';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslatePipe],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  
  // Login Form
  loginForm: FormGroup;
  isSubmitting = false;
  loginSuccess = false;
  loginError = '';
  
  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }
  
  // Login Action Methods
  onLogin() {
    if (this.loginForm.valid) {
      this.isSubmitting = true;
      
      // Create LoginRequest from form values
      const loginData: LoginRequest = {
        Email: this.loginForm.value.email,
        Password: this.loginForm.value.password
      };
      
      this.loginService.loginUser(loginData).subscribe({
        next: (response: LoginResponse) => {
          this.isSubmitting = false;
          
          // Check the success field in the response body
          if (response.success) {
            // Success case
            this.loginSuccess = true;
            this.loginError = ''; // Clear any previous errors
            
            // Store the JWT token and user data
            if (response.token) {
              this.loginService.storeToken(response.token, response.user);
            }
            
            // Show success message briefly, then redirect
            setTimeout(() => {
              this.router.navigate(['/home']);
            }, 1500);
          } else {
            // Error case (backend sends 200 OK but success: false)
            this.loginError = response.message || 'Login failed. Please try again.';
            
            // Clear error after 5 seconds
            setTimeout(() => {
              this.loginError = '';
            }, 5000);
          }
        },
        error: (error) => {
          this.isSubmitting = false;
          
          // Handle network/HTTP errors (not backend business logic errors)
          if (error.error && error.error.message) {
            this.loginError = error.error.message;
          } else if (error.message) {
            this.loginError = error.message;
          } else {
            this.loginError = 'Network error. Please check your connection and try again.';
          }
          
          // Clear error after 5 seconds
          setTimeout(() => {
            this.loginError = '';
          }, 5000);
        }
      });
    }
  }

  // Test with dummy data
  fillDummyData() {
    this.loginForm.patchValue({
      email: 'john.doe@example.com',
      password: 'TestPassword123'
    });
  }

  // Navigate to registration page
  goToRegister() {
    this.router.navigate(['/register']);
  }
}
