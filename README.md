# 🚢 Markeby (Sea Trip Management System)

**Markeby** is a robust, scalable, and high-performance RESTful API built with **ASP.NET Core** to manage sea trips, boats, captains, and bookings. Designed with **Clean Architecture** and best practices, it provides a seamless backend solution for sea tourism companies.

---

## 🌟 Features

### ⛵ Core Business Logic
* **Trip Management:** Create, schedule, and manage sea trips (Snorkeling, Diving, Fishing, Party, Private).
* **Boat & Captain Management:** Manage fleet and staff details, capacities, and statuses.
* **Booking System:** Users can browse available trips and book tickets. Includes "Soft Delete" (Cancel) logic to preserve historical financial data.
* **Automated State Management:** A custom **Background Service** runs continuously to update the statuses of Trips, Boats, and Captains based on real-time schedules (e.g., automatically marking a trip as "Ongoing" or "Completed").

### ⚙️ Technical Highlights
* **Performance Optimization:** * Implemented **Pagination**, `AsNoTracking`, and **Eager Loading** strategies to handle large datasets efficiently.
    * Integrated high-performance **Caching (Cache-Aside pattern)** using `MemoryCache` to reduce database load and improve response times for frequently accessed data (e.g., active boats, available trips).
* **File Management:** Centralized `IFileService` to handle image uploads for user profiles, boats, captains, and trips directly to the server's `wwwroot`.
* **Dashboards & Analytics:** Comprehensive endpoints for admin dashboards providing paginated statistics and filtering for Trips, Boats, Captains, and Users.

### 🔐 Security
* **Authentication & Authorization:** Secure RESTful APIs using **JWT (JSON Web Tokens)**.
* **Role-Based Access Control (RBAC):** Distinct roles (`Admin`, `User`) to restrict sensitive endpoints and dashboard access.

---

## 🛠️ Technologies & Tools

* **Framework:** .NET 8 (ASP.NET Core Web API)
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core (EF Core)
* **Identity:** ASP.NET Core Identity
* **Architecture:** Clean Architecture (Domain, Application, Infrastructure, Presentation)
* **Design Patterns:** CQRS, MediatR, Repository Pattern, Unit of Work, Singleton (Background Services)
* **Mapping:** AutoMapper
* **Documentation:** Swagger / OpenAPI

---

## 🏗️ Architecture & Patterns

Markeby strictly follows **Clean Architecture** principles, ensuring a separation of concerns and high maintainability:
1.  **Domain Layer:** Contains core entities (`Boat`, `Captain`, `Trip`, `Booking`, `AppUser`) and Enums.
2.  **Application Layer:** Contains interfaces, DTOs, and the CQRS implementation (Commands & Queries) using **MediatR**.
3.  **Infrastructure Layer:** Implements Data Access (EF Core DbContext), Repositories, Unit of Work, Identity, and external services (File Uploads, Caching).
4.  **Presentation Layer:** The API Controllers handling HTTP requests and responses.

---

## 🚀 Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Local or Cloud)
* IDE (Visual Studio 2022, VS Code, or Rider)

### Setup Instructions

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/your-username/markeby.git
    cd markeby
    ```

2.  **Configure the Database & JWT:**
    Open `appsettings.json` or `appsettings.Development.json` and update the `ConnectionStrings` and `JWT` settings:
    ```json
    "ConnectionStrings": {
      "MyConnectionStr": "Server=YOUR_SERVER; Database=MarkebyDb; User Id=YOUR_USER; Password=YOUR_PASSWORD; Encrypt=True; TrustServerCertificate=True;"
    },
    "JWT": {
      "Key": "your-very-secure-and-long-secret-key-here!",
      "Issuer": "MarkebyAPI",
      "Audience": "MarkebyUsers"
    }
    ```

3.  **Apply Migrations:**
    Open the Package Manager Console (PMC) or terminal and run:
    ```bash
    dotnet ef database update
    ```

4.  **Run the Application:**
    ```bash
    dotnet run
    ```
    Navigate to `https://localhost:port/swagger` to explore the API endpoints.

---

## 📝 API Endpoints Overview

* `POST /api/Auth/register` - Register a new user.
* `POST /api/Auth/login` - Authenticate and get a JWT token.
* `GET /api/Users/profile` - Get the current user's profile and bookings.
* `GET /api/Trips` - Get paginated trips (supports filters for Admin/User views).
* `GET /api/Boats/dashboard` - Get paginated boat statistics for Admin.
* `POST /api/Boats` - Add a new boat with images (Admin only).

---

> **Note:** The `wwwroot/images` folder is automatically generated upon the first image upload to store static assets.

