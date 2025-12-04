import axios from 'axios';
import type {
  AuthResponse,
  LoginCredentials,
  RegisterData,
  Room,
  CreateRoomData,
  UpdateRoomData,
  Booking,
  CreateBookingData,
  UpdateBookingData
} from '../types';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add token to requests
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Auth API
export const authAPI = {
  register: (data: RegisterData) => api.post<AuthResponse>('/auth/register', data),
  login: (data: LoginCredentials) => api.post<AuthResponse>('/auth/login', data),
};

// Rooms API
export const roomsAPI = {
  getAll: () => api.get<Room[]>('/rooms'),
  getById: (id: number) => api.get<Room>(`/rooms/${id}`),
  create: (data: CreateRoomData) => api.post<Room>('/rooms', data),
  update: (id: number, data: UpdateRoomData) => api.put(`/rooms/${id}`, data),
  delete: (id: number) => api.delete(`/rooms/${id}`),
};

// Bookings API
export const bookingsAPI = {
  getAll: () => api.get<Booking[]>('/bookings'),
  getById: (id: number) => api.get<Booking>(`/bookings/${id}`),
  create: (data: CreateBookingData) => api.post<Booking>('/bookings', data),
  update: (id: number, data: UpdateBookingData) => api.put(`/bookings/${id}`, data),
  delete: (id: number) => api.delete(`/bookings/${id}`),
};

export default api;
