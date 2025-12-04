export interface User {
  id: number;
  username: string;
  email: string;
  role: string;
}

export interface Room {
  id: number;
  name: string;
  description?: string;
  capacity: number;
  location?: string;
  isAvailable: boolean;
  pricePerHour?: number;
  createdAt: string;
  updatedAt?: string;
}

export interface Booking {
  id: number;
  roomId: number;
  userId: number;
  startTime: string;
  endTime: string;
  purpose?: string;
  status: string;
  createdAt: string;
  updatedAt?: string;
  room?: Room;
  user?: User;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface LoginCredentials {
  emailOrUsername: string;
  password: string;
}

export interface RegisterData {
  username: string;
  email: string;
  password: string;
}

export interface CreateRoomData {
  name: string;
  description?: string;
  capacity: number;
  location?: string;
  isAvailable: boolean;
  pricePerHour?: number;
}

export interface UpdateRoomData {
  name?: string;
  description?: string;
  capacity?: number;
  location?: string;
  isAvailable?: boolean;
  pricePerHour?: number;
}

export interface CreateBookingData {
  roomId: number;
  startTime: string;
  endTime: string;
  purpose?: string;
}

export interface UpdateBookingData {
  startTime?: string;
  endTime?: string;
  purpose?: string;
  status?: string;
}
