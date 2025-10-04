# 📚 Library Management System  

## 📖 Overview  
The **Library Management System (LMS)** is a backend application built with **ASP.NET Core** that helps libraries manage books, users, and transactions. It automates tasks like borrowing, returning, 
fine calculation, and reporting. The project follows a **3-Tier Architecture** for scalability, maintainability, and clean separation of concerns.  

## ⚙️ Features  
- User Management (Admins & Members with role-based access)  
- Book Management (Add, Update, Delete, Search)  
- Borrowing & Returning with due dates and fine calculation  
- Authentication & Authorization using Identity Core  
- Excel and PDF report generation for library statistics  

## 🏗️ Technologies & Tools  
- **ASP.NET Core** (Backend framework)  
- **Entity Framework Core** (DbContext, Migrations, LINQ)  
- **Identity Core** (Authentication & Authorization)  
- **Generic Repository Pattern**  
- **DTOs & Request Models**  
- **Dependency Injection**  
- **Middleware Exception Handling**  
- **NLog** (Logging)  
- **Excel Report Generation**  
- **PDF Report Generation** (using HTML & CSS templates)  

## 📂 Project Architecture  
The project is structured using **3-Tier Architecture**:  
1. **Data Access Layer** – DbContext, Repositories, Migrations  
2. **Business Logic Layer** – Services, DTOs, Validation  
3. **Presentation Layer** – Controllers, API Endpoints  

## 🚀 Getting Started  

### Prerequisites  
- .NET SDK (.Net 8)  
- SQL Server  
- Visual Studio / Rider / VS Code  

### Setup & Run  
1. Clone the repository:  
   ```bash
   git clone https://github.com/RemahShayya/LibraryManagementSystem.git
