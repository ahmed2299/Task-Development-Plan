# 🚀 TDP (Task Development Platform)

A modern, enterprise-grade full-stack web application built with .NET 8 Web API backend and Angular 19 frontend, featuring comprehensive user management, JWT authentication, and advanced development patterns.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-19-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Documentation](#api-documentation)
- [Database Schema](#database-schema)
- [Security Features](#security-features)
- [Development Workflow](#development-workflow)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

TDP (Task Development Platform) is a comprehensive web application designed to streamline team development processes, user management, and task coordination. Built with modern technologies and best practices, it provides a robust foundation for enterprise applications.

### Key Benefits
- **Scalable Architecture**: Built with .NET 8 and Angular 19 for optimal performance
- **Secure Authentication**: JWT-based authentication with role-based access control
- **Real-time Processing**: Background job processing with Hangfire
- **Modern UI/UX**: Responsive Angular frontend with Material Design principles
- **Database Efficiency**: Entity Framework Core with lazy loading and pagination

## ✨ Features

### 🔐 Authentication & Authorization
- JWT-based secure authentication
- Role-based access control
- Refresh token mechanism
- Secure password hashing with BCrypt

### 👥 User Management
- User registration and login
- Profile management and updates
- Password change functionality
- User role management
- Pagination and lazy loading support

### 🗄️ Data Management
- Entity Framework Core with SQL Server
- Lazy loading for optimal performance
- Pagination for large datasets
- Comprehensive data validation

### 📧 Communication
- Email service integration
- SMTP configuration support
- Template-based email sending

### ⚡ Background Processing
- Hangfire integration for task scheduling
- Background job processing
- Job monitoring and management

### 🌐 Frontend Features
- Responsive Angular 19 application
- Internationalization (i18n) support
- Modern component architecture
- Route guards for protected routes
- HTTP interceptors for authentication

## 🏗️ Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Angular 19    │    │   .NET 8 API    │    │   SQL Server    │
│   Frontend      │◄──►│   Backend       │◄──►│   Database      │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Components    │    │   Controllers   │    │   Entity Models │
│   Services      │    │   Services      │    │   Migrations    │
│   Guards        │    │   Repository    │    │   Seed Data     │
│   Interceptors  │    │   Validation    │    └─────────────────┘
└─────────────────┘    └─────────────────┘
```

## 🛠️ Tech Stack

### Backend
- **Framework**: .NET 8 Web API
- **ORM**: Entity Framework Core 9.0
- **Database**: SQL Server
- **Authentication**: JWT Bearer Tokens
- **Validation**: FluentValidation
- **Background Jobs**: Hangfire
- **Logging**: log4net
- **Mapping**: AutoMapper
- **Password Hashing**: BCrypt.Net-Next

### Frontend
- **Framework**: Angular 19
- **Language**: TypeScript 5.5
- **Styling**: CSS3 with modern design principles
- **State Management**: Angular Services with RxJS
- **Routing**: Angular Router with Guards

### Development Tools
- **Package Manager**: NuGet (.NET), npm (Angular)
- **Build Tools**: MSBuild, Angular CLI
- **Version Control**: Git
- **IDE Support**: Visual Studio, VS Code, Rider

## 📁 Project Structure

```
TDP/
├── TDP/                          # Main Web API Project
│   ├── Controllers/              # API Controllers
│   ├── Program.cs               # Application entry point
│   ├── appsettings.json         # Configuration (gitignored)
│   └── appsettings.Development.json
├── TDP.BLL/                     # Business Logic Layer
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Models/                   # Entity Models
│   ├── Services/                 # Business Services
│   ├── Repository/               # Data Access Layer
│   ├── Validators/               # FluentValidation Rules
│   ├── Mapping/                  # AutoMapper Profiles
│   └── Migrations/               # Database Migrations
└── TDP.client/                   # Angular Frontend
    ├── src/
    │   ├── app/
    │   │   ├── components/       # Angular Components
    │   │   ├── services/         # Angular Services
    │   │   ├── guards/           # Route Guards
    │   │   └── shared/           # Shared Components
    │   ├── environments/         # Environment Configs
    │   └── assets/               # Static Assets
    └── package.json
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (Express or higher)
- Node.js 18+ and npm
- Angular CLI 19

### Backend Setup
1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/TDP-FullStack-WebApp.git
   cd TDP-FullStack-WebApp
   ```

2. **Configure database connection**
   ```bash
   # Copy template configuration
   cp TDP/appsettings.Example.json TDP/appsettings.json
   # Edit with your database details
   ```

3. **Run database migrations**
   ```bash
   cd TDP
   dotnet ef database update
   ```

4. **Start the API**
   ```bash
   dotnet run
   ```

### Frontend Setup
1. **Install dependencies**
   ```bash
   cd TDP.client
   npm install
   ```

2. **Start development server**
   ```bash
   npm start
   ```

3. **Build for production**
   ```bash
   npm run build
   ```

## ⚙️ Configuration

### Required Configuration Files
- `appsettings.json` - Main configuration (copy from `appsettings.Example.json`)
- `appsettings.Development.json` - Development overrides

### Key Configuration Sections
```json
{
  "ConnectionStrings": {
    "TdpDatabase": "your-connection-string"
  },
  "JwtSettings": {
    "SecretKey": "your-128-char-secret",
    "Issuer": "TDP.Web",
    "Audience": "TDP.Client",
    "ExpirationInMinutes": 60
  }
}
```

### Security Notes
- ⚠️ Never commit `appsettings.json` to version control
- 🔐 Use strong, random JWT secret keys
- 🗄️ Use Windows Authentication when possible
- 📧 Configure SMTP settings for email functionality

## 📚 API Documentation

### Authentication Endpoints
- `POST /api/User/register` - User registration
- `POST /api/User/login` - User authentication
- `POST /api/User/change-password` - Password change

### User Management Endpoints
- `GET /api/User` - Get all users
- `GET /api/User/{id}` - Get user by ID
- `GET /api/User/paginated` - Get paginated users
- `PUT /api/User/{id}` - Update user information

### Swagger Documentation
Access API documentation at: `https://localhost:5001/swagger`

## 🗄️ Database Schema

### Core Entities
- **Users**: User accounts and profiles
- **Departments**: Organizational structure
- **Employees**: Employee information and relationships

### Key Features
- **Lazy Loading**: Related data loaded on demand
- **Pagination**: Efficient data retrieval for large datasets
- **Migrations**: Version-controlled database schema changes
- **Seed Data**: Initial data population

## 🔒 Security Features

### Authentication
- JWT token-based authentication
- Secure password hashing with BCrypt
- Token expiration and refresh mechanisms
- Role-based access control

### Data Protection
- Input validation with FluentValidation
- SQL injection prevention with Entity Framework
- CORS configuration for Angular frontend
- Secure configuration management

### Best Practices
- Secrets stored in configuration files (gitignored)
- HTTPS enforcement in production
- Regular security audits
- Secure development guidelines

## 🔄 Development Workflow

### Code Quality
- **Validation**: FluentValidation for DTOs
- **Mapping**: AutoMapper for object transformations
- **Logging**: Comprehensive logging with log4net
- **Error Handling**: Structured error responses

### Testing Strategy
- Unit testing for business logic
- Integration testing for API endpoints
- Frontend component testing
- Database migration testing

### Deployment
- **Development**: Local development with hot reload
- **Staging**: Environment-specific configurations
- **Production**: Secure deployment with environment variables

## 🤝 Contributing

### Development Guidelines
1. Follow .NET and Angular coding standards
2. Use meaningful commit messages
3. Test changes before submitting
4. Update documentation as needed

### Pull Request Process
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request
5. Ensure all tests pass

### Code Review
- All changes require review
- Security-focused code review
- Performance considerations
- Documentation updates

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

### Getting Help
- Check the [Configuration Setup Guide](CONFIGURATION_SETUP.md)
- Review the [Security Checklist](SECURITY_CHECKLIST.md)
- Open an issue on GitHub
- Contact the development team

### Common Issues
- Database connection problems
- JWT token validation errors
- Angular build issues
- Configuration setup challenges

---

## 🎉 Acknowledgments

- Built with modern web technologies
- Follows industry best practices
- Designed for scalability and maintainability
- Community-driven development approach

---

**TDP - Empowering Teams Through Modern Development** 🚀

*Built with ❤️ using .NET 8 and Angular 19*
