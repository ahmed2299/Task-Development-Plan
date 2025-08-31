export const environment = {
  // Build Configuration
  production: true,
  
  // API Configuration
  apiUrl: 'https://your-production-domain.com/api', // Update with your production URL
  apiTimeout: 15000, // 15 seconds (faster for production)
  
  // App Information
  appName: 'TDP Client',
  version: '1.0.0',
  
  // Feature Flags (Production)
  enableDebugMode: false,
  enableLogging: false,
  enableMockData: false,
  
  // UI Configuration
  defaultLanguage: 'en',
  supportedLanguages: ['en', 'ar'],
  
  // Security (Production)
  enableHttps: true,
  sessionTimeout: 1800000, // 30 minutes (more secure)
  
  // External Services (Production)
  googleAnalyticsId: 'GA-XXXXXXXXX', // Your real GA ID
  sentryDsn: 'https://your-sentry-dsn@sentry.io/project' // Your real Sentry DSN
};
