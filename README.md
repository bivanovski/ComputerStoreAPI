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

