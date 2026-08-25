# 📚 Library Management System

A full-featured Library Management System built with **ASP.NET Core MVC** and **Entity Framework Core**, featuring relational database design, user authentication, live camera capture via WebRTC, and REST API documentation with Swagger.

## Features

- **Multi-entity relational database** — Author, Book, Category, Member, and BorrowRecord with One-to-Many and Many-to-Many relationships
- **Full CRUD operations** for all entities via a unified tab-based Dashboard
- **Borrow/Return business logic** — automatic tracking of available copies and overdue fine calculation
- **User Authentication** — powered by ASP.NET Core Identity (Register/Login/Logout)
- **Live Camera Capture (WebRTC)** — capture member photos directly from the browser using `getUserMedia()`, with front/back camera switching and start/stop controls
- **REST API with Swagger** — interactive API documentation and testing at `/swagger`
- **Responsive UI** — Bootstrap 5 with a Bootswatch (Flatly) theme

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core |
| Database | SQL Server LocalDB |
| Authentication | ASP.NET Core Identity |
| Frontend | Razor Views, Bootstrap 5, Bootswatch |
| Camera | WebRTC (`getUserMedia`) + HTML5 Canvas |
| API Docs | Swagger / Swashbuckle |

## Database Structure

```
Author (1) ─────< (Many) Book
                     │
                     │ (Many)──< (Many) Category   [via BookCategories]
                     │
                     └────< (Many) BorrowRecord >──── (1) Member
```

## Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET 10 SDK
- SQL Server LocalDB (included with Visual Studio)

### Setup

1. Clone the repository
   ```
   git clone https://github.com/Sonchay1/LibraryManagementSystem.git
   ```

2. Open `LibraryManagementSystem.sln` in Visual Studio

3. Restore NuGet packages
   ```
   dotnet restore
   ```

4. Update the database (creates LocalDB database and applies migrations)
   ```
   Update-Database
   ```
   *(run in Package Manager Console)*

5. Run the project (`F5` or `Ctrl+F5`)

6. Navigate to `/Dashboard` for the main application, or `/swagger` for the API documentation

## Project Structure

```
├── Controllers/        # MVC and API controllers
├── Models/              # Entity models and DTOs
├── Data/                # ApplicationDbContext (EF Core)
├── Views/               # Razor views (Dashboard, Identity pages)
├── Migrations/          # EF Core migrations
└── wwwroot/             # Static assets (CSS, JS)
```

## Key Learning Areas Demonstrated

- Relational database design with EF Core (One-to-Many, Many-to-Many)
- LINQ queries, eager loading (`Include`/`ThenInclude`), change tracking
- ASP.NET Core Identity integration
- Browser MediaStream API (WebRTC) and Canvas-based image capture
- Byte array (binary) storage and retrieval for images
- RESTful API design with Swagger/OpenAPI documentation

## License

This project was built for learning and portfolio purposes.
