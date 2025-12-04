# 🏢 Room Booking Application

A full-stack room booking application built with **.NET 8 Web API** and **React with TypeScript**. Features include user authentication, room management, booking system, and role-based authorization.

![Tech Stack](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![React](https://img.shields.io/badge/React-18-61DAFB?style=for-the-badge&logo=react)
![TypeScript](https://img.shields.io/badge/TypeScript-5.3-3178C6?style=for-the-badge&logo=typescript)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?style=for-the-badge&logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?style=for-the-badge&logo=docker)

## ✨ Features

### 🔐 Authentication & Authorization
- JWT-based authentication
- Role-based access control (User/Admin)
- Secure password hashing with BCrypt
- Protected routes and API endpoints

### 🏠 Room Management
- CRUD operations for rooms
- Room availability status
- Capacity and pricing information
- Location tracking
- Admin-only room management interface

### 📅 Booking System
- Create, view, and cancel bookings
- Conflict detection (prevent double bookings)
- Time slot validation
- Booking history
- Purpose/notes for bookings

### 🎨 Modern UI/UX
- Sleek, responsive design with Tailwind CSS
- Smooth animations and transitions
- Mobile-friendly interface
- Gradient backgrounds and modern card layouts
- Icon integration with Lucide React

## 🛠️ Tech Stack

### Backend
- **.NET 8** - Web API framework
- **Entity Framework Core** - ORM
- **PostgreSQL** - Database
- **JWT** - Authentication
- **BCrypt** - Password hashing
- **Swagger** - API documentation

### Frontend
- **React 18** - UI library
- **TypeScript** - Type safety
- **Tailwind CSS** - Styling
- **React Router** - Navigation
- **Axios** - HTTP client
- **Lucide React** - Icons
- **Vite** - Build tool

### DevOps
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **Nginx** - Frontend web server

## 🚀 Getting Started

### Option 1: Using Docker (Recommended)

1.  **Clone the repository**
    ```bash
    git clone <repository-url>
    cd RoomBookingApp
    ```

2.  **Create environment file**
    ```bash
    cp .env.example .env
    # Edit .env file with your configuration if needed
    ```

3.  **Start the application**
    ```bash
    docker-compose up --build
    ```

4.  **Access the application**
    - **Frontend:** http://localhost:3000
    - **Backend API:** http://localhost:5000
    - **Swagger UI:** http://localhost:5000/swagger

### Option 2: Manual Setup

See [API/README.md](API/README.md) and [Frontend/README.md](Frontend/README.md) for detailed manual setup instructions.

## 👤 Default Users

The application comes with seeded users for testing:

### Admin Account
- **Email:** `admin@roombooking.com`
- **Password:** `Admin123!`
- **Role:** Admin (can manage rooms)

### User Account
- **Email:** `john@example.com`
- **Password:** `User123!`
- **Role:** User (can book rooms)

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────────────────┐
│                          ROOM BOOKING APPLICATION                        │
└─────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────┐
│                            FRONTEND LAYER                                │
│                    (React + TypeScript + Tailwind)                       │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐                  │
│  │    Login     │  │   Bookings   │  │    Rooms     │                  │
│  └──────────────┘  └──────────────┘  └──────────────┘                  │
│                                                                           │
└───────────────────────────────┬─────────────────────────────────────────┘
                                │ HTTP/HTTPS + JWT Token
                                ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                            API LAYER                                     │
│                        (.NET 8 Web API)                                  │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐                  │
│  │  Auth Ctrl   │  │ Rooms Ctrl   │  │ Bookings Ctrl│                  │
│  └──────────────┘  └──────────────┘  └──────────────┘                  │
│                                                                           │
└───────────────────────────────┬─────────────────────────────────────────┘
                                │ Entity Framework Core
                                ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                        DATABASE LAYER                                    │
│                        (PostgreSQL 15)                                   │
└─────────────────────────────────────────────────────────────────────────┘
```

## 🔧 Configuration

The application is configured using environment variables. See `.env.example` for all available options.

| Variable | Description | Default |
|----------|-------------|---------|
| `POSTGRES_USER` | Database superuser | `postgres` |
| `POSTGRES_PASSWORD` | Database password | `postgres123` |
| `POSTGRES_DB` | Database name | `roombooking` |
| `JWT_SECRET` | Secret key for tokens | *Required* |
| `CORS_ORIGINS` | Allowed origins | `http://localhost:3000` |
| `VITE_API_URL` | Frontend API URL | `http://localhost:5000/api` |

## 🗄️ Database Setup

The project is configured to **automatically migrate and seed the database** when you run `docker-compose up`.

- **Migrations:** Applied automatically on startup.
- **Seeding:** Default users and rooms are added if the database is empty.
- **Health Checks:** The API waits for the database to be healthy before starting.

To reset the database:
```bash
docker-compose down -v
docker-compose up --build
```

## 📁 Project Structure

```
RoomBookingApp/
├── API/                          # .NET Web API
│   ├── Controllers/              # API Controllers
│   ├── Models/                   # Data Models
│   ├── Data/                     # Database Context & Seeder
│   └── README.md                 # API Documentation
│
├── Frontend/                     # React Frontend
│   ├── src/
│   │   ├── components/           # Reusable Components
│   │   ├── pages/                # Page Components
│   │   └── context/              # React Context
│   └── README.md                 # Frontend Documentation
│
├── docker-compose.yml            # Docker Compose Configuration
├── .env.example                  # Environment Variables Template
└── README.md                     # This File
```

## 📄 License

This project is licensed under the MIT License.
