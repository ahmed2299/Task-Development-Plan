import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentService {

  constructor() { }

  // Get all environment variables
  getEnvironment() {
    return environment;
  }

  // API Configuration
  getApiUrl(): string {
    return environment.apiUrl;
  }

  getApiTimeout(): number {
    return environment.apiTimeout;
  }

  // App Information
  getAppName(): string {
    return environment.appName;
  }

  getVersion(): string {
    return environment.version;
  }

  // Feature Flags
  isDebugModeEnabled(): boolean {
    return environment.enableDebugMode;
  }

  isLoggingEnabled(): boolean {
    return environment.enableLogging;
  }

  isMockDataEnabled(): boolean {
    return environment.enableMockData;
  }

  // UI Configuration
  getDefaultLanguage(): string {
    return environment.defaultLanguage;
  }

  getSupportedLanguages(): string[] {
    return environment.supportedLanguages;
  }

  // Security
  isHttpsEnabled(): boolean {
    return environment.enableHttps;
  }

  getSessionTimeout(): number {
    return environment.sessionTimeout;
  }

  // External Services
  getGoogleAnalyticsId(): string {
    return environment.googleAnalyticsId;
  }

  getSentryDsn(): string {
    return environment.sentryDsn;
  }

  // Utility Methods
  isProduction(): boolean {
    return environment.production;
  }

  isDevelopment(): boolean {
    return !environment.production;
  }

  // Get environment info for debugging
  getEnvironmentInfo(): string {
    return `${environment.appName} v${environment.version} - ${environment.production ? 'Production' : 'Development'}`;
  }
}
