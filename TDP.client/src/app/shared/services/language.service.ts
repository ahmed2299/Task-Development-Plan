import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Language {
  code: string;
  name: string;
  flag: string;
  direction: 'ltr' | 'rtl';
}

export interface Translation {
  [key: string]: string;
}

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private currentLanguageSubject = new BehaviorSubject<string>('en');
  public currentLanguage$ = this.currentLanguageSubject.asObservable();

  private translations: { [lang: string]: Translation } = {
    en: {
      // Navigation
      'nav.home': 'Home',
      'nav.profile': 'Profile',
      'nav.demo': 'Standalone Demo',
      'nav.logout': 'Logout',
      'nav.login': 'Login',
      'nav.register': 'Register',

      // Common
      'common.submit': 'Submit',
      'common.cancel': 'Cancel',
      'common.save': 'Save',
      'common.edit': 'Edit',
      'common.delete': 'Delete',
      'common.loading': 'Loading...',
      'common.error': 'Error',
      'common.success': 'Success',
      'common.warning': 'Warning',
      'common.info': 'Information',

      // Forms
      'form.email': 'Email',
      'form.password': 'Password',
      'form.confirmPassword': 'Confirm Password',
      'form.firstName': 'First Name',
      'form.lastName': 'Last Name',
      'form.phone': 'Phone',
      'form.address': 'Address',
      'form.city': 'City',
      'form.country': 'Country',
      'form.required': 'This field is required',
      'form.invalidEmail': 'Please enter a valid email',
      'form.passwordMismatch': 'Passwords do not match',
      'form.minLength': 'Minimum length is {0} characters',

      // Messages
      'message.loginSuccess': 'Login successful!',
      'message.loginError': 'Login failed. Please try again.',
      'message.registerSuccess': 'Registration successful!',
      'message.registerError': 'Registration failed. Please try again.',
      'message.logoutSuccess': 'Logout successful!',
      'message.profileUpdated': 'Profile updated successfully!',
      'message.passwordChanged': 'Password changed successfully!',

      // Pages
      'page.home.title': 'Welcome to TDP',
      'page.home.subtitle': 'Your trusted development platform',
      'page.profile.title': 'User Profile',
      'page.profile.subtitle': 'Manage your account information',
      'page.login.title': 'Sign In',
      'page.login.subtitle': 'Enter your credentials to continue',
      'page.register.title': 'Create Account',
      'page.register.subtitle': 'Join our platform today',

      // Buttons
      'button.login': 'Sign In',
      'button.register': 'Create Account',
      'button.updateProfile': 'Update Profile',
      'button.changePassword': 'Change Password',
      'button.back': 'Back',
      'button.next': 'Next',
      'button.previous': 'Previous',
      'button.finish': 'Finish',

      // Validation
      'validation.required': 'This field is required',
      'validation.email': 'Please enter a valid email address',
      'validation.minLength': 'Minimum length is {0} characters',
      'validation.maxLength': 'Maximum length is {0} characters',
      'validation.pattern': 'Please enter a valid format',

      // Home Page
      'home.userManagement': 'User Management',
      'home.refresh': 'Refresh',
      'home.loading': 'Loading...',
      'home.loadingUsers': 'Loading users...',
      'home.retry': 'Retry',
      'home.noUsersFound': 'No Users Found',
      'home.noUsersMessage': 'There are no users in the system yet.',
      'home.totalUsers': 'Total Users',
      'home.username': 'Username',
      'home.email': 'Email',
      'home.status': 'Status',
      'home.actions': 'Actions',
      'home.viewUser': 'View User',
      'home.editUser': 'Edit User',
      'home.deleteUser': 'Delete User',
      'home.deleteConfirm': 'Are you sure you want to delete this user?',
      'home.deleteFailed': 'Failed to delete user. Please try again.',
      'home.loadUsersFailed': 'Failed to load users. Please try again.',

      // Profile Page
      'profile.userProfile': 'User Profile',
      'profile.backToHome': 'Back to Home',
      'profile.loadingProfile': 'Loading profile...',
      'profile.retry': 'Retry',
      'profile.personalInformation': 'Personal Information',
      'profile.fullName': 'Full Name',
      'profile.username': 'Username',
      'profile.emailAddress': 'Email Address',
      'profile.birthDate': 'Birth Date',
      'profile.department': 'Department',
      'profile.userId': 'User ID',
      'profile.notSpecified': 'Not specified',
      'profile.accountStatus': 'Account Status',
      'profile.active': 'Active',
      'profile.loggedIn': 'Logged In',
      'profile.accountActive': 'Your account is currently active',
      'profile.currentlyLoggedIn': 'You are currently logged in',
      'profile.accountActions': 'Account Actions',
      'profile.changePassword': 'Change Password',
      'profile.goToHome': 'Go to Home',
      'profile.noProfileData': 'No Profile Data',
      'profile.unableToLoad': 'Unable to load your profile information.',
      'profile.tryAgain': 'Try Again',

      // Register Page
      'register.userRegistration': 'User Registration',
      'register.loggedIn': 'Logged In',
      'register.fillDummyData': 'Fill Dummy Data for Testing',
      'register.fullName': 'Full Name',
      'register.enterFullName': 'Enter full name',
      'register.enterEmail': 'Enter email',
      'register.enterPassword': 'Enter password',
      'register.enterBirthDate': 'Enter birth date',
      'register.departmentId': 'Department ID',
      'enterDepartmentId': 'Enter department ID',
      'register.departmentIdHelp': 'Enter the department ID (e.g., 1, 2, 3)',
      'register.register': 'Register',
      'register.registering': 'Registering...',
      'register.backToLogin': 'Back to Login',
      'register.registrationSuccessful': 'Registration successful!',

      // Change Password Page
      'changePassword.changePassword': 'Change Password',
      'changePassword.backToProfile': 'Back to Profile',
      'changePassword.backToHome': 'Back to Home',
      'changePassword.changingPasswordFor': 'Changing Password for:',
      'changePassword.currentPassword': 'Current Password',
      'changePassword.enterCurrentPassword': 'Enter your current password',
      'changePassword.newPassword': 'New Password',
      'changePassword.enterNewPassword': 'Enter your new password',
      'changePassword.confirmNewPassword': 'Confirm New Password',
      'changePassword.confirmNewPasswordPlaceholder': 'Confirm your new password',
      'changePassword.currentPasswordRequired': 'Current password is required.',
      'changePassword.newPasswordRequired': 'New password is required.',
      'changePassword.confirmPasswordRequired': 'Password confirmation is required.',
      'changePassword.passwordMinLength': 'Password must be at least 6 characters.',
      'changePassword.passwordMinLengthInfo': 'Password must be at least 6 characters long.',
      'changePassword.passwordsMatch': 'Passwords match',
      'changePassword.passwordsDoNotMatch': 'Passwords do not match',
      'changePassword.passwordMismatchError': 'New password and confirmation password do not match.',
      'changePassword.changing': 'Changing...',
      'changePassword.clearForm': 'Clear Form',
      'changePassword.passwordSecurityTips': 'Password Security Tips',
      'changePassword.useAtLeast8Chars': 'Use at least 8 characters',
      'changePassword.includeUppercaseLowercase': 'Include uppercase and lowercase letters',
      'changePassword.addNumbersSpecialChars': 'Add numbers and special characters',
      'changePassword.avoidCommonWords': 'Avoid common words',
      'changePassword.dontUsePersonalInfo': 'Don\'t use personal information',
      'changePassword.neverSharePassword': 'Never share your password',

      // Common UI Elements
      'ui.loading': 'Loading...',
      'ui.error': 'Error',
      'ui.success': 'Success',
      'ui.warning': 'Warning',
      'ui.info': 'Information',
      'ui.confirm': 'Confirm',
      'ui.cancel': 'Cancel',
      'ui.save': 'Save',
      'ui.edit': 'Edit',
      'ui.delete': 'Delete',
      'ui.view': 'View',
      'ui.next': 'Next',
      'ui.previous': 'Previous',
      'ui.finish': 'Finish',
      'ui.close': 'Close',
      'ui.yes': 'Yes',
      'ui.no': 'No',
      'ui.ok': 'OK'
    },
    ar: {
      // Navigation
      'nav.home': 'الرئيسية',
      'nav.profile': 'الملف الشخصي',
      'nav.demo': 'عرض تجريبي',
      'nav.logout': 'تسجيل الخروج',
      'nav.login': 'تسجيل الدخول',
      'nav.register': 'إنشاء حساب',

      // Common
      'common.submit': 'إرسال',
      'common.cancel': 'إلغاء',
      'common.save': 'حفظ',
      'common.edit': 'تعديل',
      'common.delete': 'حذف',
      'common.loading': 'جاري التحميل...',
      'common.error': 'خطأ',
      'common.success': 'نجح',
      'common.warning': 'تحذير',
      'common.info': 'معلومات',

      // Forms
      'form.email': 'البريد الإلكتروني',
      'form.password': 'كلمة المرور',
      'form.confirmPassword': 'تأكيد كلمة المرور',
      'form.firstName': 'الاسم الأول',
      'form.lastName': 'اسم العائلة',
      'form.phone': 'رقم الهاتف',
      'form.address': 'العنوان',
      'form.city': 'المدينة',
      'form.country': 'البلد',
      'form.required': 'هذا الحقل مطلوب',
      'form.invalidEmail': 'يرجى إدخال بريد إلكتروني صحيح',
      'form.passwordMismatch': 'كلمات المرور غير متطابقة',
      'form.minLength': 'الحد الأدنى للطول هو {0} أحرف',

      // Messages
      'message.loginSuccess': 'تم تسجيل الدخول بنجاح!',
      'message.loginError': 'فشل تسجيل الدخول. يرجى المحاولة مرة أخرى.',
      'message.registerSuccess': 'تم إنشاء الحساب بنجاح!',
      'message.registerError': 'فشل إنشاء الحساب. يرجى المحاولة مرة أخرى.',
      'message.logoutSuccess': 'تم تسجيل الخروج بنجاح!',
      'message.profileUpdated': 'تم تحديث الملف الشخصي بنجاح!',
      'message.passwordChanged': 'تم تغيير كلمة المرور بنجاح!',

      // Pages
      'page.home.title': 'مرحباً بك في TDP',
      'page.home.subtitle': 'منصة التطوير الموثوقة لديك',
      'page.profile.title': 'الملف الشخصي للمستخدم',
      'page.profile.subtitle': 'إدارة معلومات حسابك',
      'page.login.title': 'تسجيل الدخول',
      'page.login.subtitle': 'أدخل بيانات الاعتماد الخاصة بك للمتابعة',
      'page.register.title': 'إنشاء حساب',
      'page.register.subtitle': 'انضم إلى منصتنا اليوم',

      // Buttons
      'button.login': 'تسجيل الدخول',
      'button.register': 'إنشاء حساب',
      'button.updateProfile': 'تحديث الملف الشخصي',
      'button.changePassword': 'تغيير كلمة المرور',
      'button.back': 'رجوع',
      'button.next': 'التالي',
      'button.previous': 'السابق',
      'button.finish': 'إنهاء',

      // Validation
      'validation.required': 'هذا الحقل مطلوب',
      'validation.email': 'يرجى إدخال عنوان بريد إلكتروني صحيح',
      'validation.minLength': 'الحد الأدنى للطول هو {0} أحرف',
      'validation.maxLength': 'الحد الأقصى للطول هو {0} أحرف',
      'validation.pattern': 'يرجى إدخال تنسيق صحيح',

      // Home Page
      'home.userManagement': 'إدارة المستخدمين',
      'home.refresh': 'تحديث',
      'home.loading': 'جاري التحميل...',
      'home.loadingUsers': 'جاري تحميل المستخدمين...',
      'home.retry': 'إعادة المحاولة',
      'home.noUsersFound': 'لم يتم العثور على مستخدمين',
      'home.noUsersMessage': 'لا يوجد مستخدمون في النظام بعد.',
      'home.totalUsers': 'إجمالي المستخدمين',
      'home.username': 'اسم المستخدم',
      'home.email': 'البريد الإلكتروني',
      'home.status': 'الحالة',
      'home.actions': 'الإجراءات',
      'home.viewUser': 'عرض المستخدم',
      'home.editUser': 'تعديل المستخدم',
      'home.deleteUser': 'حذف المستخدم',
      'home.deleteConfirm': 'هل أنت متأكد من أنك تريد حذف هذا المستخدم؟',
      'home.deleteFailed': 'فشل في حذف المستخدم. يرجى المحاولة مرة أخرى.',
      'home.loadUsersFailed': 'فشل في تحميل المستخدمين. يرجى المحاولة مرة أخرى.',

      // Profile Page
      'profile.userProfile': 'الملف الشخصي للمستخدم',
      'profile.backToHome': 'العودة إلى الرئيسية',
      'profile.loadingProfile': 'جاري تحميل الملف الشخصي...',
      'profile.retry': 'إعادة المحاولة',
      'profile.personalInformation': 'المعلومات الشخصية',
      'profile.fullName': 'الاسم الكامل',
      'profile.username': 'اسم المستخدم',
      'profile.emailAddress': 'عنوان البريد الإلكتروني',
      'profile.birthDate': 'تاريخ الميلاد',
      'profile.department': 'القسم',
      'profile.userId': 'معرف المستخدم',
      'profile.notSpecified': 'غير محدد',
      'profile.accountStatus': 'حالة الحساب',
      'profile.active': 'نشط',
      'profile.loggedIn': 'تم تسجيل الدخول',
      'profile.accountActive': 'حسابك نشط حالياً',
      'profile.currentlyLoggedIn': 'أنت مسجل الدخول حالياً',
      'profile.accountActions': 'إجراءات الحساب',
      'profile.changePassword': 'تغيير كلمة المرور',
      'profile.goToHome': 'الذهاب إلى الرئيسية',
      'profile.noProfileData': 'لا توجد بيانات للملف الشخصي',
      'profile.unableToLoad': 'تعذر تحميل معلومات ملفك الشخصي.',
      'profile.tryAgain': 'حاول مرة أخرى',

      // Register Page
      'register.userRegistration': 'تسجيل المستخدم',
      'register.loggedIn': 'تم تسجيل الدخول',
      'register.fillDummyData': 'ملء بيانات تجريبية للاختبار',
      'register.fullName': 'الاسم الكامل',
      'register.enterFullName': 'أدخل الاسم الكامل',
      'register.enterEmail': 'أدخل البريد الإلكتروني',
      'register.enterPassword': 'أدخل كلمة المرور',
      'register.enterBirthDate': 'أدخل تاريخ الميلاد',
      'register.departmentId': 'معرف القسم',
      'enterDepartmentId': 'أدخل معرف القسم',
      'register.departmentIdHelp': 'أدخل معرف القسم (مثل 1، 2، 3)',
      'register.register': 'تسجيل',
      'register.registering': 'جاري التسجيل...',
      'register.backToLogin': 'العودة إلى تسجيل الدخول',
      'register.registrationSuccessful': 'تم التسجيل بنجاح!',

      // Change Password Page
      'changePassword.changePassword': 'تغيير كلمة المرور',
      'changePassword.backToProfile': 'العودة إلى الملف الشخصي',
      'changePassword.backToHome': 'العودة إلى الرئيسية',
      'changePassword.changingPasswordFor': 'تغيير كلمة المرور لـ:',
      'changePassword.currentPassword': 'كلمة المرور الحالية',
      'changePassword.enterCurrentPassword': 'أدخل كلمة المرور الحالية',
      'changePassword.newPassword': 'كلمة المرور الجديدة',
      'changePassword.enterNewPassword': 'أدخل كلمة المرور الجديدة',
      'changePassword.confirmNewPassword': 'تأكيد كلمة المرور الجديدة',
      'changePassword.confirmNewPasswordPlaceholder': 'أكد كلمة المرور الجديدة',
      'changePassword.currentPasswordRequired': 'كلمة المرور الحالية مطلوبة.',
      'changePassword.newPasswordRequired': 'كلمة المرور الجديدة مطلوبة.',
      'changePassword.confirmPasswordRequired': 'تأكيد كلمة المرور مطلوب.',
      'changePassword.passwordMinLength': 'يجب أن تكون كلمة المرور 6 أحرف على الأقل.',
      'changePassword.passwordMinLengthInfo': 'يجب أن تكون كلمة المرور 6 أحرف على الأقل.',
      'changePassword.passwordsMatch': 'كلمات المرور متطابقة',
      'changePassword.passwordsDoNotMatch': 'كلمات المرور غير متطابقة',
      'changePassword.passwordMismatchError': 'كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين.',
      'changePassword.changing': 'جاري التغيير...',
      'changePassword.clearForm': 'مسح النموذج',
      'changePassword.passwordSecurityTips': 'نصائح أمان كلمة المرور',
      'changePassword.useAtLeast8Chars': 'استخدم 8 أحرف على الأقل',
      'changePassword.includeUppercaseLowercase': 'أدخل أحرف كبيرة وصغيرة',
      'changePassword.addNumbersSpecialChars': 'أضف أرقام وأحرف خاصة',
      'changePassword.avoidCommonWords': 'تجنب الكلمات الشائعة',
      'changePassword.dontUsePersonalInfo': 'لا تستخدم معلومات شخصية',
      'changePassword.neverSharePassword': 'لا تشارك كلمة المرور أبداً',

      // Common UI Elements
      'ui.loading': 'جاري التحميل...',
      'ui.error': 'خطأ',
      'ui.success': 'نجح',
      'ui.warning': 'تحذير',
      'ui.info': 'معلومات',
      'ui.confirm': 'تأكيد',
      'ui.cancel': 'إلغاء',
      'ui.save': 'حفظ',
      'ui.edit': 'تعديل',
      'ui.delete': 'حذف',
      'ui.view': 'عرض',
      'ui.next': 'التالي',
      'ui.previous': 'السابق',
      'ui.finish': 'إنهاء',
      'ui.close': 'إغلاق',
      'ui.yes': 'نعم',
      'ui.no': 'لا',
      'ui.ok': 'موافق'
    }
  };

  public availableLanguages: Language[] = [
    { code: 'en', name: 'English', flag: '🇺🇸', direction: 'ltr' },
    { code: 'ar', name: 'العربية', flag: '🇸🇦', direction: 'rtl' }
  ];

  constructor() {
    // Load saved language from localStorage
    const savedLanguage = localStorage.getItem('preferredLanguage');
    if (savedLanguage && this.translations[savedLanguage]) {
      this.setLanguage(savedLanguage);
    }
  }

  /**
   * Get current language code
   */
  getCurrentLanguage(): string {
    return this.currentLanguageSubject.value;
  }

  /**
   * Set current language
   */
  setLanguage(languageCode: string): void {
    if (this.translations[languageCode]) {
      this.currentLanguageSubject.next(languageCode);
      localStorage.setItem('preferredLanguage', languageCode);
      
      // Set document direction for RTL languages
      const language = this.availableLanguages.find(lang => lang.code === languageCode);
      if (language) {
        document.documentElement.dir = language.direction;
        document.documentElement.lang = languageCode;
      }
    }
  }

  /**
   * Get translation for a key
   */
  translate(key: string, params?: any[]): string {
    const currentLang = this.getCurrentLanguage();
    const translation = this.translations[currentLang]?.[key] || key;

    if (params && params.length > 0) {
      return translation.replace(/\{(\d+)\}/g, (match, index) => {
        return params[parseInt(index)] || match;
      });
    }

    return translation;
  }

  /**
   * Get all translations for current language
   */
  getCurrentTranslations(): Translation {
    const currentLang = this.getCurrentLanguage();
    return this.translations[currentLang] || {};
  }

  /**
   * Check if a translation key exists
   */
  hasTranslation(key: string): boolean {
    const currentLang = this.getCurrentLanguage();
    return !!this.translations[currentLang]?.[key];
  }
}
