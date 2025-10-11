import api, { API_URLS } from './api';

export interface Customer {
  id: string;
  tenantId: string;
  firstName: string;
  lastName: string;
  email?: string;
  phoneNumber?: string;
  address?: {
    street?: string;
    barangay?: string;
    municipality?: string;
    province?: string;
    zipCode?: string;
  };
  dateOfBirth?: string;
  loyaltyPoints: number;
  tags?: string[];
  notes?: string;
  customerType: 'INDIVIDUAL' | 'CORPORATE';
  createdAt: string;
  updatedAt?: string;
}

export interface CreateCustomerRequest {
  tenantId: string;
  firstName: string;
  lastName: string;
  email?: string;
  phoneNumber?: string;
  address?: {
    street?: string;
    barangay?: string;
    municipality?: string;
    province?: string;
    zipCode?: string;
  };
  dateOfBirth?: string;
  tags?: string[];
  notes?: string;
  customerType: 'INDIVIDUAL' | 'CORPORATE';
}

export interface UpdateCustomerRequest extends CreateCustomerRequest {
  id: string;
}

const customerService = {
  async getAll(): Promise<Customer[]> {
    const response = await api.get(`${API_URLS.CUSTOMER}/customers`);
    return response.data;
  },

  async getById(id: string): Promise<Customer> {
    const response = await api.get(`${API_URLS.CUSTOMER}/customers/${id}`);
    return response.data;
  },

  async create(data: CreateCustomerRequest): Promise<Customer> {
    const response = await api.post(`${API_URLS.CUSTOMER}/customers`, data);
    return response.data;
  },

  async update(id: string, data: UpdateCustomerRequest): Promise<Customer> {
    const response = await api.put(`${API_URLS.CUSTOMER}/customers/${id}`, data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`${API_URLS.CUSTOMER}/customers/${id}`);
  },

  async search(searchTerm: string, tag?: string): Promise<Customer[]> {
    const params = new URLSearchParams();
    if (searchTerm) params.append('search', searchTerm);
    if (tag) params.append('tag', tag);
    
    const response = await api.get(`${API_URLS.CUSTOMER}/customers?${params.toString()}`);
    return response.data;
  }
};

export default customerService;