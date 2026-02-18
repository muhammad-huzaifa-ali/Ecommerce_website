# Technical Specifications

## Platform & Framework

- **Backend:** ASP.NET Core MVC (C#)
- **Frontend:** HTML, CSS, Razor Views
- **Database:** SQL Server (Entity Framework Core ORM)
- **Web Server:** Kestrel (via .NET), IIS compatible

## Core Technologies

- **.NET 6 SDK** (see Program.cs)
- **Entity Framework Core**
  - Used for data access and ORM mapping (`AddDbContext<MyContext>`)
- **Session Management** (ASP.NET built-in)
- **MVC Pattern**
  - Controllers for business logic (`AdminController`, `UserController`)
  - Models for data representation (`Product`, `Order`, `Category`, etc.)
  - Views as Razor Pages

## Frontend Design

- **HTML5/CSS3**
- **Bootstrap** and **Bootstrap Icons**
- **Font Awesome** (For icons)
- **Responsive Layout** (Custom CSS + Bootstrap)

## Key Configuration

- `Program.cs`: Application bootstrap and middleware setup
- `appsettings.json`: Database connection string and global settings (not shown, but standard for .NET apps)
- Session timeout: 10 minutes (in Program.cs)

## Folder Structure

- `/Controllers`: Application logic (Admin, User, Product, Order)
- `/Models`: Data models and view models
- `/Views`: Razor (HTML) pages for both User and Admin panels
- `/Product_images`: Static assets for product listings

## Dependencies

- Microsoft.EntityFrameworkCore
- Microsoft.Extensions.DependencyInjection
- System.IO/File handling (for images, uploads, etc.)

## Security

- Admin/user authentication and session management
- Image upload validation and cleanup

---

**Note:** For full dependency information, refer to your `.csproj` file and `appsettings.json` (if available).
