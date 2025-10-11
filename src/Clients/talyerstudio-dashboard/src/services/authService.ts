import axios from 'axios';

const API_URL = 'http://localhost:5230/api/auth';

export interface LoginRequest {
  email: string;
  password: string;
  tenantId: string;
}

export interface RegisterRequest {
  email: string;
  password: password;
  firstName: string;
  lastName: string;
  tenantId: string;
}

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  tenantId: string;
  roles: string[];
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
}

const authService = {
  async login(data: LoginRequest): Promise<AuthResponse> {
    const response = await axios.post(`${API_URL}/login`, data);
    return response.data;
  },

  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await axios.post(`${API_URL}/register`, data);
    return response.data;
  },

  async refreshToken(refreshToken: string): Promise<AuthResponse> {
    const response = await axios.post(`${API_URL}/refresh`, { refreshToken });
    return response.data;
  },

  async logout(refreshToken: string): Promise<void> {
    await axios.post(`${API_URL}/revoke`, { refreshToken });
  },

  async getCurrentUser(token: string): Promise<User> {
    const response = await axios.get(`${API_URL}/me`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    return response.data;
  }
};

export default authService;