export const environment = {
  // Build Configuration
  production: false,
  
  // API Configuration
  apiUrl: 'http://localhost:5006/api',
  apiTimeout: 30000, // 30 seconds
  
  // App Information
  appName: 'TDP Client',
  version: '1.0.0',
  
  // Feature Flags (Development)
  enableDebugMode: true,
  enableLogging: true,
  enableMockData: true,
  
  // UI Configuration
  defaultLanguage: 'en',
  supportedLanguages: ['en', 'ar'],
  
  // Security (Development)
  enableHttps: false,
  sessionTimeout: 3600000, // 1 hour
  
  // External Services (Development)
  googleAnalyticsId: 'dev-ga-id',
  sentryDsn: 'dev-sentry-dsn'
};
