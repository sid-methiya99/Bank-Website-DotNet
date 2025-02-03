# Bank Management System

A modern banking management system built with ASP.NET Core and Tailwind CSS, featuring secure user authentication, account management, transaction processing, and KYC document handling.

## Features

- 🔐 Secure user authentication and authorization
- 💳 Account management (create, view, and manage bank accounts)
- 💸 Transaction processing (fund transfers, NEFT, RTGS)
- 📄 KYC document upload and verification
- 📱 Responsive design with modern UI
- 🛡️ Anti-forgery protection and secure data handling
- 👨‍💼 Admin dashboard for system management

## Admin Dashboard

### Accessing Admin Panel
- URL: `/admin` or `/Admin/Login`
- Default Admin Credentials:
  - Email: admin@bank.com
  - Password: Admin@123

### Admin Features
1. **User Management** (`/Admin/Users`)
   - View all users
   - Activate/Deactivate users
   - View user profiles and details
   - Filter users by role and status

2. **Account Management** (`/Admin/Accounts`)
   - View all bank accounts
   - Activate/Suspend accounts
   - Monitor account balances
   - Filter accounts by status

3. **KYC Management** (`/Admin/KYC`)
   - Review KYC documents
   - Approve/Reject KYC submissions
   - View user verification status
   - Download submitted documents

### Admin Security
- Separate admin login portal
- Role-based access control
- Audit logging for admin actions
- Secure session management

## Database Migration Steps

### For First-Time Setup
1. Ensure MySQL server is running
2. Update connection string in `appsettings.json`
3. Run migrations:
   ```bash
   dotnet ef database update
   ```

### Updating Existing Database
1. Backup your existing database
   ```bash
   mysqldump -u your_username -p bankmanagement > backup.sql
   ```

2. Update to latest code
   ```bash
   git pull origin main
   ```

3. Check pending migrations
   ```bash
   dotnet ef migrations list
   ```

4. Apply pending migrations
   ```bash
   dotnet ef database update
   ```

### Troubleshooting Database Issues
1. If database is corrupted or needs reset:
   ```bash
   # Drop existing database
   dotnet ef database drop --force
   
   # Recreate and apply all migrations
   dotnet ef database update
   ```

2. If you need to roll back to a specific migration:
   ```bash
   dotnet ef database update MigrationName
   ```

3. To remove last migration (if not applied to database):
   ```bash
   dotnet ef migrations remove
   ```

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

4. Create `appsettings.json` in the project root and configure your database connection:
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