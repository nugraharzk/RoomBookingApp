import { createContext } from 'react';
import type { User, LoginCredentials, RegisterData } from '../types';

export interface AuthContextType {
  user: User | null;
  loading: boolean;
  login: (credentials: LoginCredentials) => Promise<{ success: boolean; error?: string }>;
  register: (data: RegisterData) => Promise<{ success: boolean; error?: string }>;
  logout: () => void;
  isAdmin: () => boolean;
}

export const AuthContext = createContext<AuthContextType | null>(null);
