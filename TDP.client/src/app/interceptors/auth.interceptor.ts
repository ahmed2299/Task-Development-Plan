import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { LoginService } from '../services/login.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private loginService: LoginService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Get the token from localStorage
    const token = this.loginService.getToken();
    
    // If token exists, add it to the request headers
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    // Handle the request and catch any errors
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        // Check if the error is due to unauthorized access (401) or forbidden (403)
        if (error.status === 401 || error.status === 403) {
          // Token is expired or invalid
          console.log('Token expired or invalid, logging out user');
          
          // Clear the token and user data
          this.loginService.removeToken();
          
          // Redirect to login page
          this.router.navigate(['/login']);
          
          // Show a message to the user (optional)
          // You could use a toast service here if you have one
        }
        
        // Return the error to be handled by the component
        return throwError(() => error);
      })
    );
  }
}
