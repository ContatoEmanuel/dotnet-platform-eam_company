# EAM Company - Enterprise Platform

![.NET Version](https://img.shields.io/badge/.NET-8.0%20%7C%209.0-purple) ![Architecture](https://img.shields.io/badge/Architecture-Clean%20Arch%20%2B%20DDD-green) ![Status](https://img.shields.io/badge/Status-In%20Development-yellow)

The official monolithic platform for **EAM Company**, orchestrating the Institutional Website, Client Portal, and Central API.
Designed to showcase high-level software engineering capabilities using the full Microsoft Stack.

## 🏗️ Architectural Overview

This solution follows the **Clean Architecture** principles, enforcing strict separation of concerns and dependency rules.

```mermaid
graph TD
    User[User / Client] --> Web[Presentation Layer (MVC / Blazor)]
    Web --> API[Central API]
    API --> App[Application Layer (Use Cases)]
    App --> Domain[Domain Layer (Entities & Rules)]
    App --> Infra[Infrastructure (Data & IoC)]
    Infra --> DB[(SQL Server)]
