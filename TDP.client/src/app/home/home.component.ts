import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LoginService } from '../services/login.service';
import { UserService, User } from '../services/user.service';
import { TranslatePipe } from '../shared/pipes/translate.pipe';
import { LanguageService } from '../shared/services/language.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, TranslatePipe],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  // User listing properties
  users: User[] = [];
  isLoading = false;
  errorMessage = '';
  
  // Logged in user properties
  currentUser: any = null;
  
  constructor(
    private loginService: LoginService,
    private router: Router,
    private userService: UserService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.getCurrentUser();
    this.loadUsers();
  }

  // Get current logged in user
  getCurrentUser(): void {
    this.currentUser = this.loginService.getUserData();
    console.log('Current user:', this.currentUser);
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

  // Load users from backend
  loadUsers(): void {
    this.isLoading = true;
    this.errorMessage = '';
    
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
        console.log('Users loaded:', users);
      },
      error: (error) => {
        this.errorMessage = this.languageService.translate('home.loadUsersFailed');
        this.isLoading = false;
        console.error('Error loading users:', error);
      }
    });
  }

  // Refresh users list
  refreshUsers(): void {
    this.loadUsers();
  }

  // Delete user
  deleteUser(userId: number): void {
    if (confirm(this.languageService.translate('home.deleteConfirm'))) {
      this.userService.deleteUser(userId).subscribe({
        next: () => {
          console.log('User deleted successfully');
          this.loadUsers(); // Reload the list
        },
        error: (error) => {
          console.error('Error deleting user:', error);
          alert(this.languageService.translate('home.deleteFailed'));
        }
      });
    }
  }
}
