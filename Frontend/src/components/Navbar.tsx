import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { LogOut, Home, Calendar, Settings, Menu, X } from 'lucide-react';

const Navbar: React.FC = () => {
  const { user, logout, isAdmin } = useAuth();
  const location = useLocation();
  const [mobileMenuOpen, setMobileMenuOpen] = React.useState(false);

  const isActive = (path: string) => location.pathname === path;

  return (
    <nav className="bg-white shadow-lg sticky top-0 z-50 backdrop-blur-lg bg-opacity-95">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-16">
          <div className="flex items-center">
            <Link to="/" className="flex items-center space-x-2 group">
              <div className="bg-gradient-to-r from-blue-600 to-purple-600 p-2 rounded-lg transform group-hover:scale-110 transition-transform duration-300">
                <Calendar className="h-6 w-6 text-white" />
              </div>
              <span className="text-2xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
                RoomBook
              </span>
            </Link>
          </div>

          {/* Desktop Navigation */}
          <div className="hidden md:flex items-center space-x-1">
            {user ? (
              <>
                <NavLink to="/" icon={<Home className="h-4 w-4" />} active={isActive('/')}>
                  Rooms
                </NavLink>
                <NavLink to="/bookings" icon={<Calendar className="h-4 w-4" />} active={isActive('/bookings')}>
                  My Bookings
                </NavLink>
                {isAdmin() && (
                  <NavLink to="/admin/rooms" icon={<Settings className="h-4 w-4" />} active={isActive('/admin/rooms')}>
                    Manage Rooms
                  </NavLink>
                )}
                <div className="ml-4 flex items-center space-x-4">
                  <div className="px-4 py-2 bg-gradient-to-r from-blue-50 to-purple-50 rounded-lg border border-blue-200">
                    <p className="text-sm font-medium text-gray-700">{user.username}</p>
                    <p className="text-xs text-gray-500">{user.role}</p>
                  </div>
                  <button
                    onClick={logout}
                    className="flex items-center space-x-2 px-4 py-2 bg-gradient-to-r from-red-600 to-red-700 text-white rounded-lg hover:from-red-700 hover:to-red-800 transition-all duration-300 transform hover:scale-105 shadow-md hover:shadow-lg"
                  >
                    <LogOut className="h-4 w-4" />
                    <span>Logout</span>
                  </button>
                </div>
              </>
            ) : (
              <>
                <Link
                  to="/login"
                  className="ml-4 px-6 py-2 text-blue-600 hover:text-blue-700 font-medium transition-colors duration-200"
                >
                  Login
                </Link>
                <Link
                  to="/register"
                  className="px-6 py-2 bg-gradient-to-r from-blue-600 to-purple-600 text-white rounded-lg hover:from-blue-700 hover:to-purple-700 transition-all duration-300 transform hover:scale-105 shadow-md hover:shadow-lg"
                >
                  Register
                </Link>
              </>
            )}
          </div>

          {/* Mobile menu button */}
          <div className="md:hidden flex items-center">
            <button
              onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
              className="p-2 rounded-lg text-gray-600 hover:text-gray-900 hover:bg-gray-100 transition-colors duration-200"
            >
              {mobileMenuOpen ? <X className="h-6 w-6" /> : <Menu className="h-6 w-6" />}
            </button>
          </div>
        </div>
      </div>

      {/* Mobile Navigation */}
      {mobileMenuOpen && (
        <div className="md:hidden bg-white border-t border-gray-200 animate-fade-in">
          <div className="px-4 py-4 space-y-2">
            {user ? (
              <>
                <MobileNavLink to="/" onClick={() => setMobileMenuOpen(false)}>
                  Rooms
                </MobileNavLink>
                <MobileNavLink to="/bookings" onClick={() => setMobileMenuOpen(false)}>
                  My Bookings
                </MobileNavLink>
                {isAdmin() && (
                  <MobileNavLink to="/admin/rooms" onClick={() => setMobileMenuOpen(false)}>
                    Manage Rooms
                  </MobileNavLink>
                )}
                <div className="pt-4 border-t border-gray-200">
                  <div className="px-4 py-2 bg-gray-50 rounded-lg mb-2">
                    <p className="text-sm font-medium text-gray-700">{user.username}</p>
                    <p className="text-xs text-gray-500">{user.role}</p>
                  </div>
                  <button
                    onClick={() => {
                      logout();
                      setMobileMenuOpen(false);
                    }}
                    className="w-full flex items-center justify-center space-x-2 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors duration-200"
                  >
                    <LogOut className="h-4 w-4" />
                    <span>Logout</span>
                  </button>
                </div>
              </>
            ) : (
              <>
                <MobileNavLink to="/login" onClick={() => setMobileMenuOpen(false)}>
                  Login
                </MobileNavLink>
                <MobileNavLink to="/register" onClick={() => setMobileMenuOpen(false)}>
                  Register
                </MobileNavLink>
              </>
            )}
          </div>
        </div>
      )}
    </nav>
  );
};

interface NavLinkProps {
  to: string;
  icon: React.ReactNode;
  active: boolean;
  children: React.ReactNode;
}

const NavLink: React.FC<NavLinkProps> = ({ to, icon, active, children }) => (
  <Link
    to={to}
    className={`flex items-center space-x-2 px-4 py-2 rounded-lg font-medium transition-all duration-200 ${
      active
        ? 'bg-gradient-to-r from-blue-600 to-purple-600 text-white shadow-md'
        : 'text-gray-700 hover:bg-gray-100'
    }`}
  >
    {icon}
    <span>{children}</span>
  </Link>
);

interface MobileNavLinkProps {
  to: string;
  onClick: () => void;
  children: React.ReactNode;
}

const MobileNavLink: React.FC<MobileNavLinkProps> = ({ to, onClick, children }) => (
  <Link
    to={to}
    onClick={onClick}
    className="block px-4 py-3 text-gray-700 hover:bg-gray-100 rounded-lg font-medium transition-colors duration-200"
  >
    {children}
  </Link>
);

export default Navbar;
