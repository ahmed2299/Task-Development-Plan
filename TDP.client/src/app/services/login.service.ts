import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

// Interface for login request
export interface LoginRequest {
  Email: string;
  Password: string;
}

// Interface for login response
export interface LoginResponse {
  success: boolean;
  message: string;
  token?: string;
  tokenExpiration?: string;
  user?: {
    id: number;
    username: string;
    email: string;
    name: string;
    departmentID: number;
    birthDate: string;
  };
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export interface ChangePasswordResponse {
  success: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  
  // Base URL for your backend API
  private readonly baseUrl = environment.apiUrl;
  private readonly loginEndpoint = '/user/login';
  
  constructor(private http: HttpClient) { }
  
  /**
   * Login user and get JWT token
   * @param loginData - User login credentials
   * @returns Observable of login response
   */
  loginUser(loginData: LoginRequest): Observable<LoginResponse> {
    const url = `${this.baseUrl}${this.loginEndpoint}`;
    
    // Set headers
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    
    return this.http.post<LoginResponse>(url, loginData, { headers });
  }

  /**
   * Change user password
   * @param changePasswordData - Current and new password data
   * @returns Observable with change password response
   */
  changePassword(changePasswordData: ChangePasswordRequest): Observable<ChangePasswordResponse> {
    const url = `${this.baseUrl}/user/change-password`;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    // Note: Authorization header will be automatically added by AuthInterceptor
    return this.http.put<ChangePasswordResponse>(url, changePasswordData, { headers });
  }

  /**
   * Store JWT token and user data in localStorage
   * @param token - JWT token from backend
   * @param user - User data from backend
   */
  storeToken(token: string, user?: any): void {
    localStorage.setItem('authToken', token);
    if (user) {
      localStorage.setItem('userData', JSON.stringify(user));
    }
  }

  /**
   * Get stored JWT token
   * @returns JWT token or null if not found
   */
  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  /**
   * Get stored user data
   * @returns User data or null if not found
   */
  getUserData(): any {
    const userData = localStorage.getItem('userData');
    return userData ? JSON.parse(userData) : null;
  }

  /**
   * Remove stored JWT token and user data (logout)
   */
  removeToken(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userData');
  }

  /**
   * Check if user is logged in
   * @returns true if token exists, false otherwise
   */
  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    // Check if token is expired
    if (this.isTokenExpired()) {
      this.removeToken();
      return false;
    }
    
    return true;
  }

  /**
   * Check if the JWT token is expired
   * @returns true if token is expired, false otherwise
   */
  private isTokenExpired(): boolean {
    try {
      const token = this.getToken();
      if (!token) return true;
      
      // Decode the JWT token (JWT tokens are base64 encoded)
      const payload = JSON.parse(atob(token.split('.')[1]));
      
      // Check if token has expired
      const currentTime = Date.now() / 1000;
      return payload.exp < currentTime;
    } catch (error) {
      console.error('Error checking token expiration:', error);
      return true; // If we can't decode the token, consider it expired
    }
  }

  /**
   * Force logout user (used when token expires)
   */
  forceLogout(): void {
    this.removeToken();
    // Note: Router navigation will be handled by the interceptor
  }

  /**
   * Get token expiration time (for debugging purposes)
   * @returns Date when token expires or null if no token
   */
  getTokenExpirationTime(): Date | null {
    try {
      const token = this.getToken();
      if (!token) return null;
      
      const payload = JSON.parse(atob(token.split('.')[1]));
      return new Date(payload.exp * 1000);
    } catch (error) {
      console.error('Error getting token expiration time:', error);
      return null;
    }
  }

  /**
   * Check if token will expire soon (within 5 minutes)
   * @returns true if token expires soon, false otherwise
   */
  isTokenExpiringSoon(): boolean {
    try {
      const token = this.getToken();
      if (!token) return true;
      
      const payload = JSON.parse(atob(token.split('.')[1]));
      const currentTime = Date.now() / 1000;
      const fiveMinutesFromNow = currentTime + (5 * 60); // 5 minutes in seconds
      
      return payload.exp < fiveMinutesFromNow;
    } catch (error) {
      console.error('Error checking if token expires soon:', error);
      return true;
    }
  }
}
