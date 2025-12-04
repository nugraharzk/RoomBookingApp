import React, { useState, useEffect } from 'react';
import { roomsAPI } from '../services/api';
import type { Room, CreateRoomData } from '../types';
import { Plus, Edit2, Trash2, Loader2, X, Save, AlertCircle } from 'lucide-react';
import { AxiosError } from 'axios';

const AdminRooms: React.FC = () => {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editingRoom, setEditingRoom] = useState<Room | null>(null);
  const [formData, setFormData] = useState<CreateRoomData>({
    name: '',
    description: '',
    capacity: 1,
    location: '',
    isAvailable: true,
    pricePerHour: 0,
  });

  useEffect(() => {
    fetchRooms();
  }, []);

  const fetchRooms = async () => {
    try {
      const response = await roomsAPI.getAll();
      setRooms(response.data);
      setLoading(false);
    } catch (err) {
      setError('Failed to fetch rooms');
      setLoading(false);
    }
  };

  const handleOpenModal = (room?: Room) => {
    if (room) {
      setEditingRoom(room);
      setFormData({
        name: room.name,
        description: room.description || '',
        capacity: room.capacity,
        location: room.location || '',
        isAvailable: room.isAvailable,
        pricePerHour: room.pricePerHour || 0,
      });
    } else {
      setEditingRoom(null);
      setFormData({
        name: '',
        description: '',
        capacity: 1,
        location: '',
        isAvailable: true,
        pricePerHour: 0,
      });
    }
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    setEditingRoom(null);
    setError('');
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    setFormData({
      ...formData,
      [name]:
        type === 'checkbox'
          ? (e.target as HTMLInputElement).checked
          : type === 'number'
          ? Number(value)
          : value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      if (editingRoom) {
        await roomsAPI.update(editingRoom.id, formData);
      } else {
        await roomsAPI.create(formData);
      }
      fetchRooms();
      handleCloseModal();
    } catch (err) {
      const axiosError = err as AxiosError<{ message: string }>;
      setError(axiosError.response?.data?.message || 'Failed to save room');
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this room?')) {
      return;
    }

    try {
      await roomsAPI.delete(id);
      fetchRooms();
    } catch (err) {
      alert('Failed to delete room');
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <Loader2 className="h-12 w-12 animate-spin text-blue-600 mx-auto mb-4" />
          <p className="text-gray-600">Loading rooms...</p>
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
              Manage Rooms
            </h1>
            <p className="text-gray-600">Add, edit, or remove rooms from the system</p>
          </div>
          <button
            onClick={() => handleOpenModal()}
            className="btn btn-success flex items-center space-x-2"
          >
            <Plus className="h-5 w-5" />
            <span>Add Room</span>
          </button>
        </div>

        {/* Rooms Table */}
        <div className="card overflow-hidden animate-slide-up">
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-gradient-to-r from-blue-600 to-purple-600 text-white">
                <tr>
                  <th className="px-6 py-4 text-left text-sm font-semibold">Name</th>
                  <th className="px-6 py-4 text-left text-sm font-semibold">Location</th>
                  <th className="px-6 py-4 text-left text-sm font-semibold">Capacity</th>
                  <th className="px-6 py-4 text-left text-sm font-semibold">Price/Hour</th>
                  <th className="px-6 py-4 text-left text-sm font-semibold">Status</th>
                  <th className="px-6 py-4 text-right text-sm font-semibold">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-200">
                {rooms.map((room) => (
                  <tr key={room.id} className="hover:bg-gray-50 transition-colors duration-150">
                    <td className="px-6 py-4">
                      <div>
                        <p className="font-semibold text-gray-900">{room.name}</p>
                        {room.description && (
                          <p className="text-sm text-gray-500 line-clamp-1">{room.description}</p>
                        )}
                      </div>
                    </td>
                    <td className="px-6 py-4 text-gray-700">{room.location || '-'}</td>
                    <td className="px-6 py-4 text-gray-700">{room.capacity}</td>
                    <td className="px-6 py-4 text-gray-700">${room.pricePerHour}</td>
                    <td className="px-6 py-4">
                      <span
                        className={`badge ${
                          room.isAvailable ? 'badge-success' : 'badge-danger'
                        }`}
                      >
                        {room.isAvailable ? 'Available' : 'Unavailable'}
                      </span>
                    </td>
                    <td className="px-6 py-4">
                      <div className="flex justify-end space-x-2">
                        <button
                          onClick={() => handleOpenModal(room)}
                          className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors duration-200"
                          title="Edit"
                        >
                          <Edit2 className="h-4 w-4" />
                        </button>
                        <button
                          onClick={() => handleDelete(room.id)}
                          className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors duration-200"
                          title="Delete"
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>

        {/* Modal */}
        {showModal && (
          <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50 animate-fade-in">
            <div className="bg-white rounded-2xl shadow-2xl max-w-2xl w-full max-h-[90vh] overflow-y-auto animate-slide-up">
              <div className="sticky top-0 bg-gradient-to-r from-blue-600 to-purple-600 text-white px-6 py-4 flex justify-between items-center rounded-t-2xl">
                <h2 className="text-2xl font-bold">
                  {editingRoom ? 'Edit Room' : 'Add New Room'}
                </h2>
                <button
                  onClick={handleCloseModal}
                  className="p-2 hover:bg-white hover:bg-opacity-20 rounded-lg transition-colors duration-200"
                >
                  <X className="h-6 w-6" />
                </button>
              </div>

              <form onSubmit={handleSubmit} className="p-6 space-y-6">
                {error && (
                  <div className="p-4 bg-red-50 border border-red-200 rounded-lg flex items-start space-x-3">
                    <AlertCircle className="h-5 w-5 text-red-600 flex-shrink-0 mt-0.5" />
                    <p className="text-sm text-red-800">{error}</p>
                  </div>
                )}

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div>
                    <label className="block text-sm font-semibold text-gray-700 mb-2">
                      Room Name *
                    </label>
                    <input
                      type="text"
                      name="name"
                      value={formData.name}
                      onChange={handleChange}
                      className="input"
                      placeholder="e.g., Conference Room A"
                      required
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-semibold text-gray-700 mb-2">
                      Location
                    </label>
                    <input
                      type="text"
                      name="location"
                      value={formData.location}
                      onChange={handleChange}
                      className="input"
                      placeholder="e.g., 1st Floor, East Wing"
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-semibold text-gray-700 mb-2">
                      Capacity *
                    </label>
                    <input
                      type="number"
                      name="capacity"
                      value={formData.capacity}
                      onChange={handleChange}
                      className="input"
                      min="1"
                      required
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-semibold text-gray-700 mb-2">
                      Price per Hour ($)
                    </label>
                    <input
                      type="number"
                      name="pricePerHour"
                      value={formData.pricePerHour}
                      onChange={handleChange}
                      className="input"
                      min="0"
                      step="0.01"
                    />
                  </div>
                </div>

                <div>
                  <label className="block text-sm font-semibold text-gray-700 mb-2">
                    Description
                  </label>
                  <textarea
                    name="description"
                    value={formData.description}
                    onChange={handleChange}
                    className="input min-h-[100px] resize-none"
                    placeholder="Describe the room and its amenities..."
                  />
                </div>

                <div className="flex items-center space-x-3">
                  <input
                    type="checkbox"
                    id="isAvailable"
                    name="isAvailable"
                    checked={formData.isAvailable}
                    onChange={handleChange}
                    className="w-5 h-5 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
                  />
                  <label htmlFor="isAvailable" className="text-sm font-medium text-gray-700">
                    Room is available for booking
                  </label>
                </div>

                <div className="flex space-x-3 pt-4">
                  <button type="submit" className="flex-1 btn btn-primary flex items-center justify-center space-x-2">
                    <Save className="h-5 w-5" />
                    <span>{editingRoom ? 'Update Room' : 'Create Room'}</span>
                  </button>
                  <button
                    type="button"
                    onClick={handleCloseModal}
                    className="flex-1 btn btn-secondary"
                  >
                    Cancel
                  </button>
                </div>
              </form>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default AdminRooms;
