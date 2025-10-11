import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import authService, { type LoginRequest, type RegisterRequest, type AuthResponse, type User } from '../services/authService';
import { jwtDecode } from 'jwt-decode';

interface AuthContextType {
  user: User | null;
  accessToken: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (data: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => Promise<void>;
  refreshAccessToken: () => Promise<string | null>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  const isTokenExpired = (token: string): boolean => {
    try {
      const decoded: any = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      return decoded.exp < currentTime;
    } catch {
      return true;
    }
  };

  useEffect(() => {
    const loadUser = async () => {
      const storedToken = localStorage.getItem('accessToken');
      const storedUser = localStorage.getItem('user');
      const refreshToken = localStorage.getItem('refreshToken');

      if (storedToken && storedUser) {
        if (isTokenExpired(storedToken)) {
          if (refreshToken) {
            try {
              const newToken = await refreshAccessToken();
              if (newToken) {
                setAccessToken(newToken);
                setUser(JSON.parse(storedUser));
              } else {
                clearAuth();
              }
            } catch {
              clearAuth();
            }
          } else {
            clearAuth();
          }
        } else {
          setAccessToken(storedToken);
          setUser(JSON.parse(storedUser));
        }
      }
      setIsLoading(false);
    };

    loadUser();
  }, []);

  const clearAuth = () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
    setAccessToken(null);
    setUser(null);
  };

  const login = async (data: LoginRequest) => {
    try {
      const response: AuthResponse = await authService.login(data);
      
      localStorage.setItem('accessToken', response.accessToken);
      localStorage.setItem('refreshToken', response.refreshToken);
      localStorage.setItem('user', JSON.stringify(response.user));

      setAccessToken(response.accessToken);
      setUser(response.user);
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Login failed');
    }
  };

  const register = async (data: RegisterRequest) => {
    try {
      const response: AuthResponse = await authService.register(data);
      
      localStorage.setItem('accessToken', response.accessToken);
      localStorage.setItem('refreshToken', response.refreshToken);
      localStorage.setItem('user', JSON.stringify(response.user));

      setAccessToken(response.accessToken);
      setUser(response.user);
    } catch (error: any) {
      throw new Error(error.response?.data?.message || 'Registration failed');
    }
  };

  const logout = async () => {
    try {
      const refreshToken = localStorage.getItem('refreshToken');
      if (refreshToken) {
        await authService.logout(refreshToken);
      }
    } catch (error) {
      console.error('Logout error:', error);
    } finally {
      clearAuth();
    }
  };

  const refreshAccessToken = async (): Promise<string | null> => {
    try {
      const refreshToken = localStorage.getItem('refreshToken');
      if (!refreshToken) {
        return null;
      }

      const response: AuthResponse = await authService.refreshToken(refreshToken);
      
      localStorage.setItem('accessToken', response.accessToken);
      localStorage.setItem('refreshToken', response.refreshToken);
      localStorage.setItem('user', JSON.stringify(response.user));

      setAccessToken(response.accessToken);
      setUser(response.user);

      return response.accessToken;
    } catch (error) {
      clearAuth();
      return null;
    }
  };

  const value: AuthContextType = {
    user,
    accessToken,
    isAuthenticated: !!user && !!accessToken,
    isLoading,
    login,
    register,
    logout,
    refreshAccessToken
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};