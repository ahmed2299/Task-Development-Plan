import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterService, RegisterResponse } from '../services/register.service';
import { Register } from '../shared/models/Register';
import { LoginService } from '../services/login.service';
import { TranslatePipe } from '../shared/pipes/translate.pipe';
import { LanguageService } from '../shared/services/language.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslatePipe],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  
  // Register Form
  registerForm: FormGroup;
  isSubmitting = false;
  registerSuccess = false;
  registerError = '';
  
  constructor(
    private fb: FormBuilder,
    private registerService: RegisterService,
    private loginService: LoginService,
    private router: Router,
    private languageService: LanguageService
  ) {
    this.registerForm = this.fb.group({
      name: [''],
      email: [''],
      password: [''],
      birthDate: [''],
      departmentID: ['']
    });
  }
  
  // Register Action Methods
  onRegister() {
    if (this.registerForm.valid) {
      this.isSubmitting = true;
      
      // Create Register from form values
      const registerData = new Register();
      registerData.Name = this.registerForm.value.name;
      registerData.Email = this.registerForm.value.email;
      registerData.Password = this.registerForm.value.password;
      registerData.BirthDate = new Date(this.registerForm.value.birthDate);
      registerData.DepartmentID = parseInt(this.registerForm.value.departmentID);
      
      this.registerService.registerUser(registerData).subscribe({
        next: (response: RegisterResponse) => {
          this.isSubmitting = false;
          
          // Check the success field in the response body
          if (response.success) {
            // Success case
            this.registerSuccess = true;
            this.registerError = ''; // Clear any previous errors
            this.registerForm.reset();
            
            // Hide success message after 3 seconds
            setTimeout(() => {
              this.registerSuccess = false;
            }, 3000);
          } else {
            // Error case (backend sends 200 OK but success: false)
            this.registerError = response.message || 'Registration failed. Please try again.';
            
            // Clear error after 5 seconds
            setTimeout(() => {
              this.registerError = '';
            }, 5000);
          }
        },
        error: (error) => {
          this.isSubmitting = false;
          
          // Handle network/HTTP errors (not backend business logic errors)
          if (error.error && error.error.message) {
            this.registerError = error.error.message;
          } else if (error.message) {
            this.registerError = error.message;
          } else {
            this.registerError = 'Network error. Please check your connection and try again.';
          }
          
          // Clear error after 5 seconds
          setTimeout(() => {
            this.registerError = '';
          }, 5000);
        }
      });
    }
  }

  // Test with dummy data
  fillDummyData() {
    this.registerForm.patchValue({
      name: 'John Doe',
      email: 'john.doe@example.com',
      password: 'TestPassword123',
      birthDate: '1990-05-15',
      departmentID: 1
    });
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

  // Navigate back to login page
  goToLogin() {
    this.router.navigate(['/login']);
  }
}
