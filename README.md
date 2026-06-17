# E-Commerce Backend API

A scalable E-Commerce Backend API built with ASP.NET Core and Clean Architecture principles. The project provides secure authentication and authorization, product and category management, shopping cart functionality, and order processing through a clean and maintainable architecture.

---

## Overview

This project was developed as a practical backend application to apply software engineering concepts and backend development best practices using ASP.NET Core.

The application supports the core workflows of an e-commerce platform, including user authentication, product management, cart operations, and checkout/order creation.

---

## Features

### Authentication & Authorization

- User Registration
- User Login
- JWT Authentication
- Role-Based Authorization

### Product Management

- Create Product
- Update Product
- Delete Product
- Get Product By Id
- Get All Products

### Category Management

- Create Category
- Update Category
- Delete Category
- Get Category By Id
- Get All Categories

### Shopping Cart

- Create Cart Automatically
- Add Products To Cart
- Update Product Quantity
- Remove Products From Cart
- View User Cart

### Orders & Checkout

- Checkout Cart
- Create Orders
- View User Orders
- Get Order Details

---

## Architecture

The project follows Clean Architecture principles and is organized into separate layers:

### Domain Layer

Contains:

- Entities
- Domain Models
- Business Rules

### Application Layer

Contains:

- DTOs
- Interfaces
- Service Contracts
- Business Logic

### Infrastructure Layer

Contains:

- Entity Framework Core
- Repository Implementations
- Database Access Logic

### API Layer

Contains:

- Controllers
- API Endpoints
- Authentication Configuration
- Dependency Injection Setup

---

## Technologies Used

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT Authentication
- Clean Architecture
- Repository Pattern

---

## Project Structure

```text
src
│
├── ECommerce.API
├── ECommerce.Application
├── ECommerce.Domain
└── ECommerce.Infrastructure
```

---

## Database

- SQL Server
- Entity Framework Core (Code First)
- Migrations

---

## API Modules

### Authentication

| Endpoint | Description |
|-----------|------------|
| Register | Create New User |
| Login | Authenticate User |

### Categories

| Endpoint | Description |
|-----------|------------|
| GET | Get Categories |
| GET/{id} | Get Category By Id |
| POST | Create Category |
| PUT | Update Category |
| DELETE | Delete Category |

### Products

| Endpoint | Description |
|-----------|------------|
| GET | Get Products |
| GET/{id} | Get Product By Id |
| POST | Create Product |
| PUT | Update Product |
| DELETE | Delete Product |

### Cart

| Endpoint | Description |
|-----------|------------|
| GET | Get Cart |
| POST | Add Item To Cart |
| PUT | Update Quantity |
| DELETE | Remove Item |

### Orders

| Endpoint | Description |
|-----------|------------|
| POST | Checkout |
| GET | Get User Orders |
| GET/{id} | Get Order Details |

---

## Getting Started

### 1. Clone Repository

```bash
git clone https://github.com/belalnaser7/ECommerce.git
```

### 2. Navigate To Project

```bash
cd ECommerce
```

### 3. Configure Database

Update the connection string inside:

```json
appsettings.json
```

```json
"ConnectionStrings": {
  "DefaultConnection": "YOUR_CONNECTION_STRING"
}
```

### 4. Apply Migrations

```bash
dotnet ef database update
```

### 5. Run The Application

```bash
dotnet run
```

The API will start and Swagger UI will be available for testing endpoints.

---

## Future Improvements

- Async/Await Refactoring
- Result Pattern Implementation
- Global Exception Handling Middleware
- Pagination
- Product Search & Filtering
- Refresh Tokens
- Caching
- Unit Testing
- Integration Testing
- Payment Gateway Integration

---

## What I Learned

Through this project I practiced:

- ASP.NET Core Web API Development
- Clean Architecture
- Entity Framework Core
- Repository Pattern
- JWT Authentication
- Authorization & Security
- API Design
- Database Modeling
- Backend Project Structure

---

## Author

**Belal Nasser**

GitHub:
https://github.com/belalnaser7

LinkedIn:
https://www.linkedin.com/in/belal-nasser-0b475022a/
