import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { LogOut, Home, Calendar, Settings, Menu, X, ChevronLeft, ChevronRight, UserPlus } from 'lucide-react';

const Sidebar: React.FC = () => {
  const { user, logout, isAdmin } = useAuth();
  const location = useLocation();
  const [isOpen, setIsOpen] = useState(false); // Default to collapsed
  const [isHovered, setIsHovered] = useState(false);
  const [mobileOpen, setMobileOpen] = useState(false);

  const isActive = (path: string) => location.pathname === path;

  const toggleSidebar = () => setIsOpen(!isOpen);
  const toggleMobileSidebar = () => setMobileOpen(!mobileOpen);

  const isExpanded = isOpen || isHovered;

  const NavItem = ({ to, icon, label }: { to: string; icon: React.ReactNode; label: string }) => (
    <Link
      to={to}
      className={`flex items-center space-x-3 px-4 py-3 rounded-lg transition-all duration-200 group ${
        isActive(to)
          ? 'bg-gradient-to-r from-blue-600 to-purple-600 text-white shadow-md'
          : 'text-gray-600 hover:bg-gray-100 hover:text-blue-600'
      }`}
    >
      <div className={`${isActive(to) ? 'text-white' : 'text-gray-500 group-hover:text-blue-600'}`}>
        {icon}
      </div>
      <span className={`font-medium whitespace-nowrap transition-opacity duration-200 ${
        !isExpanded && !mobileOpen ? 'opacity-0 w-0 hidden' : 'opacity-100'
      }`}>
        {label}
      </span>
    </Link>
  );

  return (
    <>
      {/* Mobile Menu Button */}
      <div className="md:hidden fixed top-4 left-4 z-50">
        <button
          onClick={toggleMobileSidebar}
          className="p-2 rounded-lg bg-white shadow-md text-gray-600 hover:text-blue-600 transition-colors"
        >
          {mobileOpen ? <X size={24} /> : <Menu size={24} />}
        </button>
      </div>

      {/* Overlay for mobile */}
      {mobileOpen && (
        <div 
          className="md:hidden fixed inset-0 bg-black bg-opacity-50 z-40"
          onClick={() => setMobileOpen(false)}
        />
      )}

      {/* Layout Placeholder - Reserves space in the flex container */}
      <div 
        className={`hidden md:block flex-shrink-0 transition-all duration-300 ${isOpen ? 'w-64' : 'w-20'}`}
      />

      {/* Actual Sidebar */}
      <aside
        onMouseEnter={() => !isOpen && setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
        className={`
          fixed inset-y-0 left-0 z-40
          bg-white shadow-xl border-r border-gray-100
          transition-all duration-300 ease-in-out
          ${mobileOpen ? 'translate-x-0 w-64' : '-translate-x-full md:translate-x-0'}
          ${isExpanded ? 'w-64' : 'w-20'}
          flex flex-col
          h-full
        `}
      >
        {/* Header */}
        <div className="h-16 flex items-center justify-between px-4 border-b border-gray-100 relative">
          <div className={`flex items-center space-x-2 ${!isExpanded && 'justify-center w-full'}`}>
            <div className="bg-gradient-to-r from-blue-600 to-purple-600 p-2 rounded-lg shrink-0">
              <Calendar className="h-6 w-6 text-white" />
            </div>
            <span className={`text-xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent whitespace-nowrap ${!isExpanded && 'hidden'}`}>
              RoomBook
            </span>
          </div>
          
          {/* Toggle Button - Absolute Positioned */}
          <button
            onClick={toggleSidebar}
            className={`
              hidden md:flex items-center justify-center
              absolute -right-3 top-6
              w-6 h-6 rounded-full 
              bg-white border border-gray-200 shadow-md 
              text-gray-500 hover:text-blue-600 hover:border-blue-200
              transition-all duration-200 z-50
            `}
          >
            {isOpen ? <ChevronLeft size={14} /> : <ChevronRight size={14} />}
          </button>
        </div>

        {/* Navigation Links */}
        <div className="flex-1 overflow-y-auto py-6 px-3 space-y-2 overflow-x-hidden">
          {user ? (
            <>
              <NavItem to="/" icon={<Home size={20} />} label="Rooms" />
              <NavItem to="/bookings" icon={<Calendar size={20} />} label="My Bookings" />
              {isAdmin() && (
                <NavItem to="/admin/rooms" icon={<Settings size={20} />} label="Manage Rooms" />
              )}
            </>
          ) : (
            <>
              <NavItem to="/login" icon={<LogOut size={20} />} label="Login" />
              <NavItem to="/register" icon={<UserPlus size={20} />} label="Register" />
            </>
          )}
        </div>

        {/* User Profile / Logout */}
        {user && (
          <div className="p-4 border-t border-gray-100 bg-gray-50">
            <div className={`flex items-center ${isExpanded ? 'justify-between' : 'justify-center'}`}>
              <div className={`flex items-center space-x-3 ${!isExpanded && 'hidden'}`}>
                <div className="w-10 h-10 rounded-full bg-gradient-to-r from-blue-100 to-purple-100 flex items-center justify-center text-blue-600 font-bold shrink-0">
                  {user.username.charAt(0).toUpperCase()}
                </div>
                <div className="overflow-hidden">
                  <p className="text-sm font-medium text-gray-700 truncate">{user.username}</p>
                  <p className="text-xs text-gray-500 truncate">{user.role}</p>
                </div>
              </div>
              
              <button
                onClick={logout}
                className={`
                  p-2 rounded-lg text-gray-500 hover:bg-red-50 hover:text-red-600 transition-colors
                  ${!isExpanded && 'w-full flex justify-center'}
                `}
                title="Logout"
              >
                <LogOut size={20} />
              </button>
            </div>
          </div>
        )}
      </aside>
    </>
  );
};

export default Sidebar;
