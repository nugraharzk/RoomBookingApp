# Room Booking Frontend

The frontend application for the Room Booking System, built with React, TypeScript, and Tailwind CSS.

## 🚀 Getting Started

### Prerequisites
- [Node.js 18+](https://nodejs.org/)

### Local Development

1.  **Navigate to the Frontend directory:**
    ```bash
    cd Frontend
    ```

2.  **Install dependencies:**
    ```bash
    npm install
    ```

3.  **Configure Environment:**
    Create a `.env` file:
    ```bash
    cp .env.example .env
    ```
    Set `VITE_API_URL` to your API endpoint (e.g., `http://localhost:5000/api`).

4.  **Run the development server:**
    ```bash
    npm run dev
    ```
    The app will be available at `http://localhost:3000`.

## 🔐 Authentication

All routes in the application require authentication, except for:
- `/login`
- `/register`

The root path `/` redirects to `/login` if the user is not authenticated.

### Auth Flow
1.  User logs in or registers.
2.  JWT token is stored in `localStorage`.
3.  `AuthContext` updates the user state.
4.  Axios interceptor attaches the token to every API request.

## 🏗️ Architecture

- **Framework:** React 18 with Vite
- **Language:** TypeScript
- **Styling:** Tailwind CSS
- **State Management:** React Context API (`AuthContext`)
- **Routing:** React Router v6
- **HTTP Client:** Axios

### Context Structure
- **`src/context/index.ts`**: Defines context types and creates the context.
- **`src/context/AuthContext.tsx`**: Implements the `AuthProvider`.
- **`src/hooks/useAuth.ts`**: Custom hook for consuming the auth context.

## 🛠️ Code Quality

### ESLint
The project uses ESLint with TypeScript and React rules.

```bash
# Run linting
npm run lint

# Fix auto-fixable issues
npm run lint:fix
```

### Key Rules
- No `any` types.
- React Hooks rules enforced.
- No unused variables.

## 📦 Build & Deployment

The application is built using Vite.

```bash
# Build for production
npm run build
```

In production (Docker), Nginx is used to serve the static files and proxy API requests to the backend.
