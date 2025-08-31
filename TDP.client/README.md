# TDP Client

A simple Angular application for the TDP project.

## Features

- Angular 19+
- TypeScript
- Bootstrap 5 styling
- Routing
- Component-based architecture
- Responsive design

## Prerequisites

- Node.js (version 18 or higher)
- npm (comes with Node.js)

## Installation

1. Navigate to the project directory:
   ```bash
   cd TDP.client
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

## Development

To start the development server:

```bash
npm start
```

This will:
- Start the development server
- Open your browser to `http://localhost:4200`
- Enable hot reload for development

## Build

To build the project for production:

```bash
npm run build
```

The built files will be in the `dist/tdp-client` directory.

## Project Structure

```
src/
├── app/
│   ├── home/
│   │   └── home.component.ts
│   ├── app.component.ts
│   └── app.module.ts
├── assets/
├── index.html
├── main.ts
├── polyfills.ts
└── styles.css
```

## Available Scripts

- `npm start` - Start development server and open browser
- `npm run serve` - Start development server
- `npm run build` - Build for production
- `npm run watch` - Build and watch for changes
- `npm test` - Run tests

## Technologies Used

- **Angular**: Frontend framework
- **TypeScript**: Programming language
- **Bootstrap**: CSS framework for styling
- **RxJS**: Reactive programming library
- **Zone.js**: Angular zone implementation
