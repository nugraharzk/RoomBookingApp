import React, { useState, useEffect, ReactNode } from "react";
import { authAPI } from "../services/api";
import type { LoginCredentials, RegisterData } from "../types";
import { AxiosError } from "axios";
import { AuthContext, type AuthContextType } from "./index";

export const AuthProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [user, setUser] = useState<AuthContextType["user"]>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Check if user is logged in
    const token = localStorage.getItem("token");
    const userData = localStorage.getItem("user");

    if (token && userData) {
      setUser(JSON.parse(userData));
    }
    setLoading(false);
  }, []);

  const login = async (credentials: LoginCredentials) => {
    try {
      const response = await authAPI.login(credentials);
      const { token, user } = response.data;

      localStorage.setItem("token", token);
      localStorage.setItem("user", JSON.stringify(user));
      setUser(user);

      return { success: true };
    } catch (error) {
      const axiosError = error as AxiosError<{ message: string }>;
      return {
        success: false,
        error: axiosError.response?.data?.message || "Login failed",
      };
    }
  };

  const register = async (userData: RegisterData) => {
    try {
      const response = await authAPI.register(userData);
      const { token, user } = response.data;

      localStorage.setItem("token", token);
      localStorage.setItem("user", JSON.stringify(user));
      setUser(user);

      return { success: true };
    } catch (error) {
      const axiosError = error as AxiosError<{ message: string }>;
      return {
        success: false,
        error: axiosError.response?.data?.message || "Registration failed",
      };
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
  };

  const isAdmin = () => {
    return user?.role === "Admin";
  };

  return (
    <AuthContext.Provider
      value={{ user, login, register, logout, isAdmin, loading }}
    >
      {children}
    </AuthContext.Provider>
  );
};
