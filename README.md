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

## 📝 Some API Endpoints Overview

* `POST /api/Authentication/create` - Register a new user.
* `POST /api/Authentication/login` - Authenticate and get a JWT token.
* `GET /api/Users/profile` - Get the current user's profile and bookings.
* `GET /api/Trips/all` - Get paginated trips (supports filters for Admin/User views).
* `GET /api/Boats/dashboard-stats` - Get paginated boat statistics for Admin.
* `POST /api/Boats` - Add a new boat with images (Admin only).

---

> **Note:** The `wwwroot/images` folder is automatically generated upon the first image upload to store static assets.

