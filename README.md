# Loan Management & Calculation Demo

A full-stack demo application for managing loans, viewing loan details, calculating monthly payments, and emailing loan calculation results.

This project uses **.NET 8 Web API** for the backend and **Angular 21** for the frontend, with **PrimeNG** and **Tailwind CSS** for the UI. It was built as a portfolio project to demonstrate modern full-stack development, REST API integration, responsive SPA design, IIS deployment, and email workflow integration. :contentReference[oaicite:0]{index=0}:contentReference[oaicite:1]{index=1}

---

## Tech Stack

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- MailKit
- IIS hosting

### Frontend
- Angular 21
- TypeScript
- Tailwind CSS
- PrimeNG
- RxJS

---

## Features

- View a paginated list of loan records
- View loan detail and backend-calculated monthly payment
- Calculate custom loan payments on the client side
- Send loan calculation results by email
- Responsive UI with modern dark-themed styling
- IIS-ready deployment setup for both frontend and backend

---

## Project Structure

```text
LoanDemo/
├── LoanServiceApp.Api/        # .NET 8 Web API
├── LoanServiceApp.Core/       # Business logic and interfaces
├── LoanServiceApp.Data/       # Data access and repositories
└── loan.webSPA/               # Angular 21 frontend