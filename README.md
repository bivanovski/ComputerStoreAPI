# Computer Store API – ASP.NET Core Proof of Concept

This project is a backend web API for a fictional computer store, built as a proof of concept using ASP.NET Core, Entity Framework Core, and Clean Architecture principles. It showcases category and product management, stock import functionality, and discount calculation logic — all covered with unit and integration tests.

---

##  Features

- ** Category Management**
  - CRUD operations for categories (name & optional description)

- ** Product Management**
  - CRUD operations for products with multiple category support

- ** Stock Import**
  - Accepts JSON input to import stock
  - Automatically creates missing products or categories

- ** Discount Calculation**
  - 5% discount on the first unit of any product purchased in quantity > 1
  - Ensures stock availability before purchase
  - Returns original vs discounted total and breakdown of discounts

---

## 🧱 Architecture

This project follows a layered (Clean) architecture:

ComputerStore.WebApi → Presentation Layer (Controllers)
ComputerStore.Service → Business Logic Layer (Services, DTOs)
ComputerStore.Data → Data Access Layer (EF Core, DbContext, Entities)

yaml
Copy
Edit

---

## 🧪 Test Coverage

- ✅ **Unit Tests** with `xUnit` for core business logic (`DiscountService`)
- ✅ **Integration Tests** using `WebApplicationFactory` and `EF Core InMemory` to test real API endpoints

---

## 🛠 Tech Stack

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core (with SQL Server & InMemory for tests)
- AutoMapper for object-to-object mapping
- xUnit for testing
- Swagger/OpenAPI for API docs

---

## 📦 Example: Stock Import Input

```json
[
  {
    "name": "Intel's Core i9-9900K",
    "categories": ["CPU"],
    "price": 475.99,
    "quantity": 2
  },
  {
    "name": "Razer BlackWidow Keyboard",
    "categories": ["Keyboard", "Periphery"],
    "price": 89.99,
    "quantity": 10
  }
]

