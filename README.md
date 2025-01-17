# Bank Management System

A modern and secure banking management system built with ASP.NET Core 8.0 and Tailwind CSS. This web application provides essential banking features including user authentication, account management, and a responsive user interface.

## Features

- 🔐 Secure user authentication and authorization
- 💼 Account management dashboard
- 💰 Transaction history and fund transfers
- 📱 Responsive design with modern UI
- 🎨 Beautiful gradients and Tailwind CSS styling
- 🔒 Anti-forgery token protection
- 📄 Dynamic content rendering with Razor Pages

## Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (Latest LTS version recommended)
- [npm](https://www.npmjs.com/) (Comes with Node.js)
- A code editor (Visual Studio Code, Visual Studio, or similar)

## Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/BankManagementSystem.git
cd BankManagementSystem
```

2. Install .NET dependencies:
```bash
dotnet restore
```

3. Install Node.js dependencies:
```bash
npm install
```

4. Build Tailwind CSS:
```bash
npm run build:css
```

5. Update the database with migrations:
```bash
dotnet ef database update
```

## Configuration

1. Update the connection string in `appsettings.json` to point to your database:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BankManagementSystem;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

## Running the Application

1. Start the development server:
```bash
dotnet watch run
```

2. In a separate terminal, watch for CSS changes:
```bash
npm run watch:css
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## Project Structure

- `/Pages` - Razor Pages for the web interface
- `/Models` - Data models and database entities
- `/Data` - Database context and migrations
- `/wwwroot` - Static files (CSS, JavaScript, images)

## Development

To modify the Tailwind CSS styles:
1. Edit the styles in the source files
2. Run `npm run build:css` to rebuild the CSS
3. For development, use `npm run watch:css` to automatically rebuild on changes

## Database Migrations

To create a new migration:
```bash
dotnet ef migrations add MigrationName
```

To update the database:
```bash
dotnet ef database update
```

## Security Features

- ASP.NET Core Identity for authentication
- Anti-forgery token validation
- Secure password hashing
- HTTPS enforcement
- XSS protection

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support, please open an issue in the GitHub repository or contact the maintainers.

## Acknowledgments

- ASP.NET Core team
- Tailwind CSS team
- All contributors to this project 