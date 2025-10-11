import api, { API_URLS } from './api';

export interface Service {
  id: string;
  tenantId: string;
  name: string;
  description?: string;
  categoryId: string;
  categoryName?: string;
  basePrice: number;
  currency: string;
  applicability: string;
  estimatedDurationMinutes?: number;
  isActive: boolean;
  displayOrder?: number;
  createdAt: string;
  updatedAt?: string;
}

export interface ServiceCategory {
  id: string;
  tenantId: string;
  name: string;
  description?: string;
  icon?: string;
  displayOrder?: number;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateServiceRequest {
  tenantId: string;
  name: string;
  description?: string;
  categoryId: string;
  basePrice: number;
  currency: string;
  applicability: string;
  estimatedDurationMinutes?: number;
  isActive: boolean;
  displayOrder?: number;
}

export interface UpdateServiceRequest extends CreateServiceRequest {
  id: string;
}

export interface CreateServiceCategoryRequest {
  tenantId: string;
  name: string;
  description?: string;
  icon?: string;
  displayOrder?: number;
  isActive: boolean;
}

export interface UpdateServiceCategoryRequest extends CreateServiceCategoryRequest {
  id: string;
}

const serviceService = {
  // Services
  async getAll(): Promise<Service[]> {
    const response = await api.get(`${API_URLS.CUSTOMER}/services`);
    return response.data;
  },

  async getById(id: string): Promise<Service> {
    const response = await api.get(`${API_URLS.CUSTOMER}/services/${id}`);
    return response.data;
  },

  async create(data: CreateServiceRequest): Promise<Service> {
    const response = await api.post(`${API_URLS.CUSTOMER}/services`, data);
    return response.data;
  },

  async update(id: string, data: UpdateServiceRequest): Promise<Service> {
    const response = await api.put(`${API_URLS.CUSTOMER}/services/${id}`, data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`${API_URLS.CUSTOMER}/services/${id}`);
  },

  // Service Categories
  async getCategories(): Promise<ServiceCategory[]> {
    const response = await api.get(`${API_URLS.CUSTOMER}/servicecategories`);
    return response.data;
  },

  async getCategoryById(id: string): Promise<ServiceCategory> {
    const response = await api.get(`${API_URLS.CUSTOMER}/servicecategories/${id}`);
    return response.data;
  },

  async createCategory(data: CreateServiceCategoryRequest): Promise<ServiceCategory> {
    const response = await api.post(`${API_URLS.CUSTOMER}/servicecategories`, data);
    return response.data;
  },

  async updateCategory(id: string, data: UpdateServiceCategoryRequest): Promise<ServiceCategory> {
    const response = await api.put(`${API_URLS.CUSTOMER}/servicecategories/${id}`, data);
    return response.data;
  },

  async deleteCategory(id: string): Promise<void> {
    await api.delete(`${API_URLS.CUSTOMER}/servicecategories/${id}`);
  }
};

export default serviceService;