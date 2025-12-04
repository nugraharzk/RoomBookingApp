# Room Booking API

The backend API for the Room Booking Application, built with .NET 8 Web API.

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)

### Local Development

1.  **Navigate to the API directory:**
    ```bash
    cd API
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Configure Environment:**
    Copy `.env.example` to `.env` (if not already done in root) or ensure environment variables are set.
    ```bash
    cp ../.env.example .env
    ```

4.  **Run the application:**
    ```bash
    dotnet run
    ```
    The API will start at `http://localhost:5000`.

## 🔧 Configuration

The application uses environment variables for configuration.

| Variable | Description | Default |
|----------|-------------|---------|
| `DB_HOST` | Database host | `localhost` |
| `DB_PORT` | Database port | `5432` |
| `DB_NAME` | Database name | `roombooking` |
| `DB_USER` | Database username | `postgres` |
| `DB_PASSWORD` | Database password | `postgres123` |
| `JWT_SECRET` | Secret key for JWT | *Required* |
| `JWT_ISSUER` | Token issuer | `RoomBookingAPI` |
| `JWT_AUDIENCE` | Token audience | `RoomBookingClient` |
| `JWT_EXPIRATION_MINUTES` | Token expiration | `60` |
| `CORS_ORIGINS` | Allowed origins | `http://localhost:3000` |
| `ENABLE_SWAGGER` | Enable Swagger UI | `true` |

## 📚 Swagger Documentation

The API provides interactive documentation via Swagger UI.

- **URL:** `http://localhost:5000/swagger`
- **Features:**
    - Explore all endpoints.
    - Test API calls directly from the browser.
    - View request/response schemas.

### Authentication in Swagger
1.  Get a token via `/api/auth/login` or `/api/auth/register`.
2.  Click the **Authorize** button.
3.  Enter `Bearer <your_token>`.
4.  Click **Authorize**.

## 🏗️ Architecture

- **Framework:** ASP.NET Core 8 Web API
- **Database:** PostgreSQL with Entity Framework Core
- **Auth:** JWT (JSON Web Tokens)
- **Documentation:** Swashbuckle (Swagger)

### Key Components
- **Controllers:** Handle HTTP requests (`AuthController`, `RoomsController`, `BookingsController`).
- **Services:** Business logic (e.g., `JwtService`).
- **Data:** `ApplicationDbContext` and `DbSeeder`.
- **DTOs:** Data Transfer Objects for API contracts.

## 🗄️ Database Migrations

Migrations are applied automatically on startup in Docker. For manual development:

```bash
# Add a migration
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update
```
