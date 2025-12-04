import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { bookingsAPI, roomsAPI } from '../services/api';
import type { Room } from '../types';
import { Calendar, Clock, FileText, Loader2, CheckCircle, AlertCircle, ArrowLeft } from 'lucide-react';
import { AxiosError } from 'axios';

const CreateBooking: React.FC = () => {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [formData, setFormData] = useState({
    roomId: '',
    startTime: '',
    endTime: '',
    purpose: '',
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    fetchRooms();

    // Pre-select room if passed from Rooms page
    if (location.state?.roomId) {
      setFormData((prev) => ({ ...prev, roomId: location.state.roomId.toString() }));
    }

    // Set default start time to current time rounded to next hour
    const now = new Date();
    now.setHours(now.getHours() + 1, 0, 0, 0);
    const startTime = now.toISOString().slice(0, 16);

    // Set default end time to 1 hour after start time
    const endDate = new Date(now);
    endDate.setHours(endDate.getHours() + 1);
    const endTime = endDate.toISOString().slice(0, 16);

    setFormData((prev) => ({ ...prev, startTime, endTime }));
  }, [location]);

  const fetchRooms = async () => {
    try {
      const response = await roomsAPI.getAll();
      setRooms(response.data.filter((room) => room.isAvailable));
    } catch (err) {
      setError('Failed to fetch rooms');
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      await bookingsAPI.create({
        roomId: parseInt(formData.roomId),
        startTime: new Date(formData.startTime).toISOString(),
        endTime: new Date(formData.endTime).toISOString(),
        purpose: formData.purpose,
      });

      setSuccess('Booking created successfully!');
      setTimeout(() => {
        navigate('/bookings');
      }, 1500);
    } catch (err) {
      const axiosError = err as AxiosError<{ message: string }>;
      setError(axiosError.response?.data?.message || 'Failed to create booking');
      setLoading(false);
    }
  };

  const selectedRoom = rooms.find((r) => r.id === parseInt(formData.roomId));

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 py-8 animate-fade-in">
      <div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Back Button */}
        <button
          onClick={() => navigate('/bookings')}
          className="flex items-center space-x-2 text-gray-600 hover:text-gray-900 mb-6 transition-colors duration-200"
        >
          <ArrowLeft className="h-5 w-5" />
          <span>Back to Bookings</span>
        </button>

        {/* Header */}
        <div className="mb-8">
          <h1 className="text-4xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent mb-2">
            Create New Booking
          </h1>
          <p className="text-gray-600">Reserve a room for your upcoming event or meeting</p>
        </div>

        <div className="card p-8 animate-slide-up">
          {error && (
            <div className="mb-6 p-4 bg-red-50 border border-red-200 rounded-lg flex items-start space-x-3 animate-fade-in">
              <AlertCircle className="h-5 w-5 text-red-600 flex-shrink-0 mt-0.5" />
              <p className="text-sm text-red-800">{error}</p>
            </div>
          )}

          {success && (
            <div className="mb-6 p-4 bg-green-50 border border-green-200 rounded-lg flex items-start space-x-3 animate-fade-in">
              <CheckCircle className="h-5 w-5 text-green-600 flex-shrink-0 mt-0.5" />
              <p className="text-sm text-green-800">{success}</p>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Room Selection */}
            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Select Room *
              </label>
              <div className="relative">
                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <Calendar className="h-5 w-5 text-gray-400" />
                </div>
                <select
                  name="roomId"
                  value={formData.roomId}
                  onChange={handleChange}
                  className="input pl-10"
                  required
                >
                  <option value="">Choose a room...</option>
                  {rooms.map((room) => (
                    <option key={room.id} value={room.id}>
                      {room.name} - {room.location} (${room.pricePerHour}/hr)
                    </option>
                  ))}
                </select>
              </div>
              {selectedRoom && (
                <div className="mt-3 p-4 bg-blue-50 border border-blue-200 rounded-lg">
                  <p className="text-sm text-gray-700">
                    <span className="font-semibold">Capacity:</span> {selectedRoom.capacity} people
                  </p>
                  {selectedRoom.description && (
                    <p className="text-sm text-gray-600 mt-1">{selectedRoom.description}</p>
                  )}
                </div>
              )}
            </div>

            {/* Date and Time */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">
                  Start Time *
                </label>
                <div className="relative">
                  <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                    <Clock className="h-5 w-5 text-gray-400" />
                  </div>
                  <input
                    type="datetime-local"
                    name="startTime"
                    value={formData.startTime}
                    onChange={handleChange}
                    className="input pl-10"
                    required
                  />
                </div>
              </div>

              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">
                  End Time *
                </label>
                <div className="relative">
                  <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                    <Clock className="h-5 w-5 text-gray-400" />
                  </div>
                  <input
                    type="datetime-local"
                    name="endTime"
                    value={formData.endTime}
                    onChange={handleChange}
                    className="input pl-10"
                    required
                  />
                </div>
              </div>
            </div>

            {/* Purpose */}
            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Purpose (Optional)
              </label>
              <div className="relative">
                <div className="absolute top-3 left-3 pointer-events-none">
                  <FileText className="h-5 w-5 text-gray-400" />
                </div>
                <textarea
                  name="purpose"
                  value={formData.purpose}
                  onChange={handleChange}
                  className="input pl-10 min-h-[120px] resize-none"
                  placeholder="What is this booking for? (e.g., Team meeting, Client presentation)"
                />
              </div>
            </div>

            {/* Action Buttons */}
            <div className="flex flex-col sm:flex-row gap-3 pt-4">
              <button
                type="submit"
                disabled={loading}
                className="flex-1 btn btn-primary flex items-center justify-center space-x-2"
              >
                {loading ? (
                  <>
                    <Loader2 className="h-5 w-5 animate-spin" />
                    <span>Creating booking...</span>
                  </>
                ) : (
                  <>
                    <CheckCircle className="h-5 w-5" />
                    <span>Create Booking</span>
                  </>
                )}
              </button>
              <button
                type="button"
                onClick={() => navigate('/bookings')}
                className="flex-1 btn btn-secondary"
                disabled={loading}
              >
                Cancel
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default CreateBooking;
