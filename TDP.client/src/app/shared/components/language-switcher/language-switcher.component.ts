import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LanguageService, Language } from '../../services/language.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-language-switcher',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './language-switcher.component.html',
  styleUrls: ['./language-switcher.component.css']
})
export class LanguageSwitcherComponent implements OnInit, OnDestroy {
  currentLanguage: string = 'en';
  availableLanguages: Language[] = [];
  isOpen = false;
  private subscription: Subscription = new Subscription();

  constructor(private languageService: LanguageService) {}

  ngOnInit(): void {
    this.availableLanguages = this.languageService.availableLanguages;
    
    this.subscription.add(
      this.languageService.currentLanguage$.subscribe(lang => {
        this.currentLanguage = lang;
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  /**
   * Change language
   */
  changeLanguage(languageCode: string): void {
    this.languageService.setLanguage(languageCode);
    this.isOpen = false;
  }

  /**
   * Toggle language dropdown
   */
  toggleDropdown(): void {
    this.isOpen = !this.isOpen;
  }

  /**
   * Close dropdown when clicking outside
   */
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.language-switcher')) {
      this.isOpen = false;
    }
  }

  /**
   * Get current language info
   */
  getCurrentLanguageInfo(): Language | undefined {
    return this.availableLanguages.find(lang => lang.code === this.currentLanguage);
  }
}
