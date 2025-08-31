import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, timeout } from 'rxjs/operators';
import { environment } from '../../environments/environment';

// Interface for user data
export interface User {
  id: number;
  username: string;
  email: string;
  status: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  
  // Base URL for your backend API
  private readonly baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) { }
  
  /**
   * Get all users
   * @returns Observable of user array
   */
  getUsers(): Observable<User[]> {
    const url = `${this.baseUrl}/user`;
    
    const headers = new HttpHeaders({
      'Accept': 'application/json'
    });
    
    return this.http.get<User[]>(url, { headers })
      .pipe(
        timeout(environment.apiTimeout),
        catchError(this.handleError)
      );
  }
  
  /**
   * Get user by ID
   * @param userId - User ID
   * @returns Observable of user
   */
  getUserById(userId: number): Observable<User> {
    const url = `${this.baseUrl}/users/${userId}`;
    
    const headers = new HttpHeaders({
      'Accept': 'application/json'
    });
    
    return this.http.get<User>(url, { headers });
  }
  
  /**
   * Update user
   * @param userId - User ID
   * @param userData - Updated user data
   * @returns Observable of updated user
   */
  updateUser(userId: number, userData: Partial<User>): Observable<User> {
    const url = `${this.baseUrl}/users/${userId}`;
    
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    
    return this.http.put<User>(url, userData, { headers });
  }
  
  /**
   * Delete user
   * @param userId - User ID
   * @returns Observable of deletion response
   */
  deleteUser(userId: number): Observable<any> {
    const url = `${this.baseUrl}/users/${userId}`;
    
    const headers = new HttpHeaders({
      'Accept': 'application/json'
    });
    
    return this.http.delete(url, { headers })
      .pipe(
        timeout(environment.apiTimeout),
        catchError(this.handleError)
      );
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: HttpErrorResponse) {
    if (environment.enableLogging) {
      console.error('An error occurred:', error);
    }
    
    let errorMessage = 'An error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    
    return throwError(() => new Error(errorMessage));
  }
}
