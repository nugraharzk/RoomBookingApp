import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import Navbar from "./components/Navbar";
import PrivateRoute from "./components/PrivateRoute";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Rooms from "./pages/Rooms";
import Bookings from "./pages/Bookings";
import CreateBooking from "./pages/CreateBooking";
import AdminRooms from "./pages/AdminRooms";

function App() {
  return (
    <Router>
      <AuthProvider>
        <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100">
          <Navbar />
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route
              path="/"
              element={
                <PrivateRoute>
                  <Rooms />
                </PrivateRoute>
              }
            />
            <Route
              path="/bookings"
              element={
                <PrivateRoute>
                  <Bookings />
                </PrivateRoute>
              }
            />
            <Route
              path="/bookings/create"
              element={
                <PrivateRoute>
                  <CreateBooking />
                </PrivateRoute>
              }
            />
            <Route
              path="/admin/rooms"
              element={
                <PrivateRoute adminOnly>
                  <AdminRooms />
                </PrivateRoute>
              }
            />
          </Routes>
        </div>
      </AuthProvider>
    </Router>
  );
}

export default App;
