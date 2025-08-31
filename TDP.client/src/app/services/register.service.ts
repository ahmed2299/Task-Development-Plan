import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Register } from '../shared/models/Register';

// Interface for registration response
export interface RegisterResponse {
  success: boolean;
  message: string;
  user?: {
    id: number;
    username: string;
    email: string;
    name: string;
    departmentID: number;
    birthDate: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  
  // Base URL for your backend API
  private readonly baseUrl = environment.apiUrl;
  private readonly registerEndpoint = '/user/register';
  
  constructor(private http: HttpClient) { }
  
  /**
   * Register a new user
   * @param registerData - User registration data
   * @returns Observable of registration response
   */
  registerUser(registerData: Register): Observable<RegisterResponse> {
    const url = `${this.baseUrl}${this.registerEndpoint}`;
    
    // Set headers
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    
    return this.http.post<RegisterResponse>(url, registerData, { headers });
  }
}
