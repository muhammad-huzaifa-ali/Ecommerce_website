# Install & Setup Guide

Follow these steps to start the shope ease platform.

## Prerequisites

- .NET 6 SDK
- SQL Server (for database)
- Visual Studio or VS Code

## Instructions

1. Clone repository:
    ```
    git clone https://github.com/muhammad-huzaifa-ali/shope-ease.git
    ```
2. Configure database:
    - Edit `appsettings.json` with your SQL Server connection string.

3. Restore Dependencies:
    ```
    dotnet restore
    ```

4. Run Project:
    ```
    dotnet run
    ```
    - Or use Visual Studio's run/play button.

## Database Setup

- The project uses Entity Framework.
- Migrations will set up the schema automatically.  
- Start with a blank database matching your connection string.
