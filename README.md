# Task Management System

A clean-architecture-based Task Management System built with **.NET 6** and **C#**, designed following **SOLID principles**. This backend-only application provides a RESTful API for managing tasks and users, suitable for extensible enterprise-grade systems.

## ✨ Features

- Create a new task (title, description, due date, priority, status)
- Assign a task to a user
- Update task details
- Delete a task
- Retrieve all tasks or filter by assigned user
- Validate task data (e.g., due date cannot be in the past)
- Logging for critical operations
- Unit tests for core functionalities

## 🏗 Architecture

This project is structured using **Clean Architecture** with the following layers:

- **Presentation Layer**: RESTful API controllers
- **Application Layer**: Use cases and business logic
- **Domain Layer**: Core entities and interfaces
- **Infrastructure Layer**: Data access using in-memory storage (can be swapped for a real database)

## ✅ SOLID Principles Applied

- **SRP**: Each class has a single responsibility
- **OCP**: Easy to extend functionality without modifying existing code
- **LSP**: Subtypes can substitute their base types
- **ISP**: Interfaces are client-specific and minimal
- **DIP**: High-level modules depend on abstractions, not concrete implementations

## 🔧 Installation

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### Steps

1. Clone the repository:
   ```bash
   git git@github.com:azkahrtmi/Task-Management.git
   cd task-management
