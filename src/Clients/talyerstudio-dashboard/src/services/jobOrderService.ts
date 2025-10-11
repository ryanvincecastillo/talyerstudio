import api, { API_URLS } from './api';

export interface JobOrderItem {
  id?: string;
  serviceId: string;
  serviceName?: string;
  description?: string;
  quantity: number;
  unitPrice: number;
  notes?: string;
}

export interface JobOrderPart {
  id?: string;
  productId: string;
  productName?: string;
  productSku?: string;
  quantity: number;
  unitPrice: number;
  notes?: string;
}

export interface JobOrder {
  id: string;
  jobOrderNumber: string;
  tenantId: string;
  branchId?: string;
  customerId: string;
  customerName?: string;
  vehicleId: string;
  vehicleInfo?: string;
  status: 'PENDING' | 'IN_PROGRESS' | 'COMPLETED' | 'INVOICED' | 'CANCELLED';
  priority: 'LOW' | 'NORMAL' | 'HIGH' | 'URGENT';  // CHANGED
  odometerReading?: number;
  customerComplaints?: string;
  inspectionNotes?: string;
  assignedMechanicIds?: string[];
  startTime?: string;
  endTime?: string;
  estimatedCompletionTime?: string;
  items: JobOrderItem[];
  parts: JobOrderPart[];
  totalAmount: number;
  discountAmount: number;
  taxAmount: number;
  grandTotal: number;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateJobOrderRequest {
  customerId: string;
  vehicleId: string;
  priority: 'LOW' | 'NORMAL' | 'HIGH' | 'URGENT';  // CHANGED
  odometerReading?: number;
  customerComplaints?: string;
  inspectionNotes?: string;
  estimatedCompletionTime?: string;
  items: JobOrderItem[];
  parts: JobOrderPart[];
}
export interface UpdateJobOrderRequest extends CreateJobOrderRequest {
  id: string;
  status?: string;
}

const jobOrderService = {
  async getAll(): Promise<JobOrder[]> {
    const response = await api.get(`${API_URLS.JOBORDER}/job-orders`);
    return response.data;
  },

  async getById(id: string): Promise<JobOrder> {
    const response = await api.get(`${API_URLS.JOBORDER}/job-orders/${id}`);
    return response.data;
  },

  async getByCustomer(customerId: string): Promise<JobOrder[]> {
    const response = await api.get(`${API_URLS.JOBORDER}/job-orders/customer/${customerId}`);
    return response.data;
  },

  async getByStatus(status: string): Promise<JobOrder[]> {
    const response = await api.get(`${API_URLS.JOBORDER}/job-orders?status=${status}`);
    return response.data;
  },

  async create(data: CreateJobOrderRequest): Promise<JobOrder> {
    // Transform to match backend DTO exactly
    const payload = {
      customerId: data.customerId,
      vehicleId: data.vehicleId,
      priority: data.priority || 'NORMAL',
      odometerReading: data.odometerReading || 0,
      customerComplaints: data.customerComplaints || '',
      inspectionNotes: data.inspectionNotes || null,
      estimatedCompletionTime: data.estimatedCompletionTime || null,
      items: data.items.map(item => ({
        serviceId: item.serviceId,
        serviceName: item.serviceName || '',
        description: item.description || null,
        quantity: item.quantity,
        unitPrice: item.unitPrice,
        notes: item.notes || null
      })),
      parts: data.parts.map(part => ({
        productId: part.productId,
        productName: part.productName || '',
        productSku: part.productSku || null,
        quantity: part.quantity,
        unitPrice: part.unitPrice,
        notes: part.notes || null
      }))
    };

    console.log('Sending payload:', payload);
    
    const response = await api.post(`${API_URLS.JOBORDER}/job-orders`, payload);
    return response.data;
  },

  async update(id: string, data: UpdateJobOrderRequest): Promise<JobOrder> {
    const payload = {
      customerId: data.customerId,
      vehicleId: data.vehicleId,
      priority: data.priority || 'NORMAL',
      odometerReading: data.odometerReading || 0,
      customerComplaints: data.customerComplaints || '',
      inspectionNotes: data.inspectionNotes || null,
      estimatedCompletionTime: data.estimatedCompletionTime || null,
      items: data.items.map(item => ({
        serviceId: item.serviceId,
        serviceName: item.serviceName || '',
        description: item.description || null,
        quantity: item.quantity,
        unitPrice: item.unitPrice,
        notes: item.notes || null
      })),
      parts: data.parts.map(part => ({
        productId: part.productId,
        productName: part.productName || '',
        productSku: part.productSku || null,
        quantity: part.quantity,
        unitPrice: part.unitPrice,
        notes: part.notes || null
      }))
    };

    const response = await api.put(`${API_URLS.JOBORDER}/job-orders/${id}`, payload);
    return response.data;
  },

  async updateStatus(id: string, status: string): Promise<JobOrder> {
    const response = await api.patch(`${API_URLS.JOBORDER}/job-orders/${id}/status`, { status });
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`${API_URLS.JOBORDER}/job-orders/${id}`);
  }
};

export default jobOrderService;