import React, { useState, useEffect } from 'react';
import { bookingsAPI } from '../services/api';
import { useNavigate } from 'react-router-dom';
import type { Booking } from '../types';
import { Calendar, Clock, MapPin, Trash2, Loader2, Plus, AlertCircle } from 'lucide-react';

const Bookings: React.FC = () => {
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    fetchBookings();
  }, []);

  const fetchBookings = async () => {
    try {
      const response = await bookingsAPI.getAll();
      setBookings(response.data);
      setLoading(false);
    } catch (err) {
      setError('Failed to fetch bookings');
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Are you sure you want to cancel this booking?')) {
      return;
    }

    try {
      await bookingsAPI.delete(id);
      fetchBookings();
    } catch (err) {
      alert('Failed to cancel booking');
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      weekday: 'short',
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  };

  const formatTime = (dateString: string) => {
    return new Date(dateString).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const getStatusBadgeClass = (status: string) => {
    switch (status.toLowerCase()) {
      case 'confirmed':
        return 'badge-success';
      case 'pending':
        return 'badge-warning';
      case 'cancelled':
        return 'badge-danger';
      default:
        return 'badge-info';
    }
  };

  const isPastBooking = (endTime: string) => {
    return new Date(endTime) < new Date();
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <Loader2 className="h-12 w-12 animate-spin text-blue-600 mx-auto mb-4" />
          <p className="text-gray-600">Loading bookings...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="bg-red-50 border border-red-200 rounded-lg p-4 text-red-800 flex items-center space-x-3">
          <AlertCircle className="h-5 w-5" />
          <span>{error}</span>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 py-8 animate-fade-in">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Header */}
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-8 gap-4">
          <div>
            <h1 className="text-4xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent mb-2">
              My Bookings
            </h1>
            <p className="text-gray-600">Manage your room reservations</p>
          </div>
          <button
            onClick={() => navigate('/')}
            className="btn btn-success flex items-center space-x-2"
          >
            <Plus className="h-5 w-5" />
            <span>New Booking</span>
          </button>
        </div>

        {/* Bookings List */}
        {bookings.length === 0 ? (
          <div className="card p-12 text-center animate-slide-up">
            <Calendar className="h-16 w-16 text-gray-400 mx-auto mb-4" />
            <h3 className="text-xl font-semibold text-gray-700 mb-2">No bookings yet</h3>
            <p className="text-gray-500 mb-6">Start by booking a room for your next meeting</p>
            <button onClick={() => navigate('/')} className="btn btn-primary inline-flex items-center space-x-2">
              <Plus className="h-5 w-5" />
              <span>Book a Room</span>
            </button>
          </div>
        ) : (
          <div className="space-y-4 animate-slide-up">
            {bookings.map((booking) => (
              <div
                key={booking.id}
                className={`card p-6 ${
                  isPastBooking(booking.endTime) ? 'opacity-75' : ''
                }`}
              >
                <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4">
                  {/* Booking Info */}
                  <div className="flex-1 space-y-3">
                    <div className="flex items-start justify-between">
                      <div>
                        <h3 className="text-xl font-bold text-gray-800 mb-1">
                          {booking.room?.name}
                        </h3>
                        <div className="flex items-center text-gray-600 text-sm">
                          <MapPin className="h-4 w-4 mr-1" />
                          <span>{booking.room?.location || 'Location not specified'}</span>
                        </div>
                      </div>
                      <span className={`badge ${getStatusBadgeClass(booking.status)}`}>
                        {booking.status}
                      </span>
                    </div>

                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                      <div className="flex items-center text-gray-700">
                        <Calendar className="h-5 w-5 text-blue-600 mr-2" />
                        <div>
                          <p className="text-xs text-gray-500">Date</p>
                          <p className="font-semibold">{formatDate(booking.startTime)}</p>
                        </div>
                      </div>
                      <div className="flex items-center text-gray-700">
                        <Clock className="h-5 w-5 text-blue-600 mr-2" />
                        <div>
                          <p className="text-xs text-gray-500">Time</p>
                          <p className="font-semibold">
                            {formatTime(booking.startTime)} - {formatTime(booking.endTime)}
                          </p>
                        </div>
                      </div>
                    </div>

                    {booking.purpose && (
                      <div className="bg-gray-50 rounded-lg p-3">
                        <p className="text-xs text-gray-500 mb-1">Purpose</p>
                        <p className="text-sm text-gray-700">{booking.purpose}</p>
                      </div>
                    )}

                    {isPastBooking(booking.endTime) && (
                      <div className="inline-flex items-center space-x-2 text-sm text-gray-500">
                        <Clock className="h-4 w-4" />
                        <span>This booking has ended</span>
                      </div>
                    )}
                  </div>

                  {/* Actions */}
                  <div className="flex lg:flex-col gap-2">
                    <button
                      onClick={() => handleDelete(booking.id)}
                      className="btn btn-danger btn-small flex items-center space-x-2"
                      disabled={isPastBooking(booking.endTime)}
                    >
                      <Trash2 className="h-4 w-4" />
                      <span>Cancel</span>
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default Bookings;
