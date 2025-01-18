# Bank Management System

A modern banking management system built with ASP.NET Core and Tailwind CSS, featuring secure user authentication, account management, transaction processing, and KYC document handling.

## Features

- 🔐 Secure user authentication and authorization
- 💳 Account management (create, view, and manage bank accounts)
- 💸 Transaction processing (fund transfers, NEFT, RTGS)
- 📄 KYC document upload and verification
- 📱 Responsive design with modern UI
- 🛡️ Anti-forgery protection and secure data handling

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v14 or later)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) (v8.0 or later)
- Code editor (Visual Studio Code, Visual Studio, or similar)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/Bank-Website-DotNet.git
   cd Bank-Website-DotNet
   ```

2. Restore .NET dependencies:
   ```bash
   dotnet restore
   ```

3. Install Node.js dependencies:
   ```bash
   npm install
   ```

4. Create `appsettings.Development.json` in the project root and configure your database connection:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=bankmanagement;User=your_username;Password=your_password;"
     }
   }
   ```

5. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

## Running the Application

1. Start the development server:
   ```bash
   dotnet run
   ```

2. In a separate terminal, watch for CSS changes:
   ```bash
   npm run css:watch
   ```

3. Access the application at `http://localhost:5116`

## Development

### Building CSS

- Build CSS once:
  ```bash
  npm run css:build
  ```

- Watch for CSS changes:
  ```bash
  npm run css:watch
  ```

### Database Migrations

- Create a new migration:
  ```bash
  dotnet ef migrations add MigrationName
  ```

- Update database:
  ```bash
  dotnet ef database update
  ```

## Project Structure

```
BankManagementSystem/
├── Data/                 # Database context and configurations
├── Models/              # Data models
├── Pages/               # Razor Pages
│   ├── Account/         # Account management pages
│   └── Shared/         # Shared layouts and components
├── wwwroot/            # Static files
│   ├── css/           # Compiled CSS
│   └── uploads/       # User uploads (KYC documents)
└── Services/           # Application services
```

## Security Features

- ASP.NET Core Identity for authentication
- Anti-forgery token validation
- Secure password hashing
- HTTPS enforcement
- File upload validation
- SQL injection protection through Entity Framework Core

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For support, please open an issue in the GitHub repository or contact the maintainers.

## Acknowledgments

- ASP.NET Core team for the amazing framework
- Tailwind CSS team for the utility-first CSS framework
- All contributors who have helped shape this project 