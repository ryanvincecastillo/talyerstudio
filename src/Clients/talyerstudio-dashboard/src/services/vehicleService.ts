import api, { API_URLS } from './api';

export interface Vehicle {
  id: string;
  tenantId: string;
  customerId: string;
  customerName?: string;
  make: string;
  model: string;
  year: number;
  color?: string;
  plateNumber: string;
  engineNumber?: string;
  chassisNumber?: string;
  vehicleType: 'AUTO' | 'MOTORCYCLE';
  vehicleCategory?: string;
  displacement?: string;
  fuelType?: string;
  transmission?: string;
  currentOdometer?: number;
  orNumber?: string;
  orExpiryDate?: string;
  crNumber?: string;
  crExpiryDate?: string;
  frontTireSize?: string;
  rearTireSize?: string;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateVehicleRequest {
  tenantId: string;
  customerId: string;
  make: string;
  model: string;
  year: number;
  color?: string;
  plateNumber: string;
  engineNumber?: string;
  chassisNumber?: string;
  vehicleType: 'AUTO' | 'MOTORCYCLE';
  vehicleCategory?: string;
  displacement?: string;
  fuelType?: string;
  transmission?: string;
  currentOdometer?: number;
  orNumber?: string;
  orExpiryDate?: string;
  crNumber?: string;
  crExpiryDate?: string;
  frontTireSize?: string;
  rearTireSize?: string;
  notes?: string;
}

export interface UpdateVehicleRequest extends CreateVehicleRequest {
  id: string;
}

const vehicleService = {
  async getAll(): Promise<Vehicle[]> {
    const response = await api.get(`${API_URLS.VEHICLE}/vehicles`);
    return response.data;
  },

  async getById(id: string): Promise<Vehicle> {
    const response = await api.get(`${API_URLS.VEHICLE}/vehicles/${id}`);
    return response.data;
  },

  async getByCustomerId(customerId: string): Promise<Vehicle[]> {
    const response = await api.get(`${API_URLS.VEHICLE}/vehicles/customer/${customerId}`);
    return response.data;
  },

  async create(data: CreateVehicleRequest): Promise<Vehicle> {
    const response = await api.post(`${API_URLS.VEHICLE}/vehicles`, data);
    return response.data;
  },

  async update(id: string, data: UpdateVehicleRequest): Promise<Vehicle> {
    const response = await api.put(`${API_URLS.VEHICLE}/vehicles/${id}`, data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`${API_URLS.VEHICLE}/vehicles/${id}`);
  },

  async search(searchTerm: string): Promise<Vehicle[]> {
    const response = await api.get(`${API_URLS.VEHICLE}/vehicles?search=${searchTerm}`);
    return response.data;
  }
};

export default vehicleService;