import React, { useState, useEffect } from 'react';
import jobOrderService, { type JobOrder, type CreateJobOrderRequest, type JobOrderItem } from '../services/jobOrderService';
import customerService, { type Customer } from '../services/customerService';
import vehicleService, { type Vehicle } from '../services/vehicleService';
import serviceService, { type Service } from '../services/serviceService';
import { useAuth } from '../contexts/AuthContext';

const JobOrders: React.FC = () => {
  const { user } = useAuth();
  const [jobOrders, setJobOrders] = useState<JobOrder[]>([]);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [services, setServices] = useState<Service[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [filterStatus, setFilterStatus] = useState<string>('ALL');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingJobOrder, setEditingJobOrder] = useState<JobOrder | null>(null);

  const [formData, setFormData] = useState<CreateJobOrderRequest>({
    customerId: '',
    vehicleId: '',
    priority: 'NORMAL',
    odometerReading: 0,
    customerComplaints: '',
    inspectionNotes: '',
    estimatedCompletionTime: '',
    items: [],
    parts: []
    });

  const [selectedCustomer, setSelectedCustomer] = useState<string>('');
  const [customerVehicles, setCustomerVehicles] = useState<Vehicle[]>([]);

  useEffect(() => {
    loadJobOrders();
    loadCustomers();
    loadServices();
  }, []);

  useEffect(() => {
    if (selectedCustomer) {
      loadCustomerVehicles(selectedCustomer);
    }
  }, [selectedCustomer]);

  const loadJobOrders = async () => {
    try {
      setIsLoading(true);
      const data = await jobOrderService.getAll();
      setJobOrders(data);
      setError('');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to load job orders');
    } finally {
      setIsLoading(false);
    }
  };

  const loadCustomers = async () => {
    try {
      const data = await customerService.getAll();
      setCustomers(data);
    } catch (err: any) {
      console.error('Failed to load customers:', err);
    }
  };

  const loadServices = async () => {
    try {
      const data = await serviceService.getAll();
      setServices(data);
    } catch (err: any) {
      console.error('Failed to load services:', err);
    }
  };

  const loadCustomerVehicles = async (customerId: string) => {
    try {
      const data = await vehicleService.getByCustomerId(customerId);
      setCustomerVehicles(data);
    } catch (err: any) {
      console.error('Failed to load vehicles:', err);
      setCustomerVehicles([]);
    }
  };

  const openModal = (jobOrder?: JobOrder) => {
    if (jobOrder) {
        setEditingJobOrder(jobOrder);
        setSelectedCustomer(jobOrder.customerId);
        setFormData({
        customerId: jobOrder.customerId,
        vehicleId: jobOrder.vehicleId,
        priority: jobOrder.priority || 'NORMAL',  // Ensure it's NORMAL not MEDIUM
        odometerReading: jobOrder.odometerReading || 0,
        customerComplaints: jobOrder.customerComplaints || '',
        inspectionNotes: jobOrder.inspectionNotes || '',
        estimatedCompletionTime: jobOrder.estimatedCompletionTime || '',
        items: jobOrder.items || [],
        parts: jobOrder.parts || []
        });
    } else {
        setEditingJobOrder(null);
        setSelectedCustomer('');
        setCustomerVehicles([]);
        setFormData({
        customerId: '',
        vehicleId: '',
        priority: 'NORMAL',  // CHANGED
        odometerReading: 0,
        customerComplaints: '',
        inspectionNotes: '',
        estimatedCompletionTime: '',
        items: [],
        parts: []
        });
    }
    setIsModalOpen(true);
};

  const closeModal = () => {
    setIsModalOpen(false);
    setEditingJobOrder(null);
    setSelectedCustomer('');
    setCustomerVehicles([]);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (formData.items.length === 0) {
      setError('Please add at least one service item');
      return;
    }

    try {
      if (editingJobOrder) {
        await jobOrderService.update(editingJobOrder.id, { 
          ...formData, 
          id: editingJobOrder.id 
        });
      } else {
        await jobOrderService.create(formData);
      }
      closeModal();
      loadJobOrders();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to save job order');
    }
  };

  const handleStatusChange = async (id: string, status: string) => {
    try {
      await jobOrderService.updateStatus(id, status);
      loadJobOrders();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to update status');
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Are you sure you want to delete this job order?')) {
      return;
    }

    try {
      await jobOrderService.delete(id);
      loadJobOrders();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to delete job order');
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    
    if (name === 'customerId') {
      setSelectedCustomer(value);
      setFormData({
        ...formData,
        customerId: value,
        vehicleId: '' // Reset vehicle when customer changes
      });
    } else {
      setFormData({
        ...formData,
        [name]: name === 'odometerReading' || name === 'discountAmount' || name === 'taxAmount' 
          ? parseFloat(value) || 0 
          : value
      });
    }
  };

  const addServiceItem = () => {
    const newItem: JobOrderItem = {
        serviceId: '',
        serviceName: '',
        quantity: 1,
        unitPrice: 0,
        notes: ''
    };
    setFormData({
        ...formData,
        items: [...formData.items, newItem]
    });
  };

  const removeServiceItem = (index: number) => {
    const newItems = formData.items.filter((_, i) => i !== index);
    setFormData({
      ...formData,
      items: newItems
    });
  };

  const updateServiceItem = (index: number, field: string, value: any) => {
    const newItems = [...formData.items];
    
    // Update the field
    newItems[index] = {
        ...newItems[index],
        [field]: value
    };

    // Auto-calculate when service is selected
    if (field === 'serviceId' && value) {
        const service = services.find(s => s.id === value);
        if (service) {
        newItems[index] = {
            ...newItems[index],
            serviceId: service.id,
            serviceName: service.name,
            unitPrice: service.basePrice,
            description: service.description || '',
            quantity: newItems[index].quantity || 1
        };
        }
    }

    setFormData({
        ...formData,
        items: newItems
    });
  };

  const calculateGrandTotal = () => {
    const itemsTotal = formData.items.reduce((sum, item) => sum + (item.quantity * item.unitPrice), 0);
    const partsTotal = formData.parts.reduce((sum, part) => sum + (part.quantity * part.unitPrice), 0);
    return itemsTotal + partsTotal;
  };

  const getCustomerName = (customerId: string) => {
    const customer = customers.find(c => c.id === customerId);
    return customer ? `${customer.firstName} ${customer.lastName}` : 'Unknown';
  };

  const getVehicleInfo = (vehicleId: string) => {
    const vehicle = vehicles.find(v => v.id === vehicleId);
    return vehicle ? `${vehicle.year} ${vehicle.make} ${vehicle.model} (${vehicle.plateNumber})` : 'Unknown';
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'PENDING': return 'bg-yellow-100 text-yellow-800';
      case 'IN_PROGRESS': return 'bg-blue-100 text-blue-800';
      case 'COMPLETED': return 'bg-green-100 text-green-800';
      case 'INVOICED': return 'bg-purple-100 text-purple-800';
      case 'CANCELLED': return 'bg-red-100 text-red-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
        case 'URGENT': return 'bg-red-100 text-red-800';
        case 'HIGH': return 'bg-orange-100 text-orange-800';
        case 'NORMAL': return 'bg-yellow-100 text-yellow-800';  // CHANGED
        case 'LOW': return 'bg-green-100 text-green-800';
        default: return 'bg-gray-100 text-gray-800';
    }
 };

  const filteredJobOrders = filterStatus === 'ALL' 
    ? jobOrders 
    : jobOrders.filter(jo => jo.status === filterStatus);

  return (
    <div>
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Job Orders</h1>
        <button
          onClick={() => openModal()}
          className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          + Create Job Order
        </button>
      </div>

      {/* Error Message */}
      {error && (
        <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}

      {/* Status Filter */}
      <div className="mb-6 flex gap-2">
        {['ALL', 'PENDING', 'IN_PROGRESS', 'COMPLETED', 'INVOICED', 'CANCELLED'].map(status => (
          <button
            key={status}
            onClick={() => setFilterStatus(status)}
            className={`px-4 py-2 rounded-md font-medium ${
              filterStatus === status
                ? 'bg-blue-600 text-white'
                : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
            }`}
          >
            {status.replace('_', ' ')}
          </button>
        ))}
      </div>

      {/* Table */}
      <div className="bg-white rounded-lg shadow overflow-hidden">
        {isLoading ? (
          <div className="p-8 text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          </div>
        ) : filteredJobOrders.length === 0 ? (
          <div className="p-8 text-center text-gray-500">
            No job orders found. Click "Create Job Order" to create one.
          </div>
        ) : (
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">JO Number</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Customer</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Vehicle</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Priority</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Total</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {filteredJobOrders.map((jobOrder) => (
                <tr key={jobOrder.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="font-medium text-gray-900">{jobOrder.jobOrderNumber}</div>
                    <div className="text-xs text-gray-500">
                      {new Date(jobOrder.createdAt).toLocaleDateString()}
                    </div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {jobOrder.customerName || getCustomerName(jobOrder.customerId)}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {jobOrder.vehicleInfo || getVehicleInfo(jobOrder.vehicleId)}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <select
                      value={jobOrder.status}
                      onChange={(e) => handleStatusChange(jobOrder.id, e.target.value)}
                      className={`px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(jobOrder.status)}`}
                    >
                      <option value="PENDING">PENDING</option>
                      <option value="IN_PROGRESS">IN PROGRESS</option>
                      <option value="COMPLETED">COMPLETED</option>
                      <option value="INVOICED">INVOICED</option>
                      <option value="CANCELLED">CANCELLED</option>
                    </select>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${getPriorityColor(jobOrder.priority)}`}>
                      {jobOrder.priority}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    ₱{jobOrder.grandTotal.toLocaleString()}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <button
                      onClick={() => openModal(jobOrder)}
                      className="text-blue-600 hover:text-blue-900 mr-4"
                    >
                      View
                    </button>
                    <button
                      onClick={() => handleDelete(jobOrder.id)}
                      className="text-red-600 hover:text-red-900"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {/* Modal - Will continue in next message due to length */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-5 mx-auto p-5 border w-full max-w-4xl shadow-lg rounded-md bg-white">
            <div className="flex justify-between items-center mb-4">
              <h3 className="text-lg font-medium text-gray-900">
                {editingJobOrder ? `Job Order: ${editingJobOrder.jobOrderNumber}` : 'Create New Job Order'}
              </h3>
              <button
                onClick={closeModal}
                className="text-gray-400 hover:text-gray-500"
              >
                <span className="text-2xl">×</span>
              </button>
            </div>

            <form onSubmit={handleSubmit} className="space-y-4 max-h-[75vh] overflow-y-auto pr-2">
              {/* Customer & Vehicle Selection */}
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Customer *</label>
                  <select
                    name="customerId"
                    required
                    value={formData.customerId}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="">Select a customer</option>
                    {customers.map((customer) => (
                      <option key={customer.id} value={customer.id}>
                        {customer.firstName} {customer.lastName}
                      </option>
                    ))}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Vehicle *</label>
                  <select
                    name="vehicleId"
                    required
                    value={formData.vehicleId}
                    onChange={handleChange}
                    disabled={!selectedCustomer}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-100"
                  >
                    <option value="">Select a vehicle</option>
                    {customerVehicles.map((vehicle) => (
                      <option key={vehicle.id} value={vehicle.id}>
                        {vehicle.year} {vehicle.make} {vehicle.model} ({vehicle.plateNumber})
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              {/* Priority & Odometer */}
              <div className="grid grid-cols-2 gap-4">
                <div>
                    <label className="block text-sm font-medium text-gray-700">Priority *</label>
                    <select
                        name="priority"
                        required
                        value={formData.priority}
                        onChange={handleChange}
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    >
                        <option value="LOW">Low</option>
                        <option value="NORMAL">Normal</option>
                        <option value="HIGH">High</option>
                        <option value="URGENT">Urgent</option>
                    </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Odometer Reading (km)</label>
                  <input
                    type="number"
                    name="odometerReading"
                    value={formData.odometerReading}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              {/* Customer Complaints */}
              <div>
                <label className="block text-sm font-medium text-gray-700">Customer Complaints</label>
                <textarea
                  name="customerComplaints"
                  rows={2}
                  value={formData.customerComplaints}
                  onChange={handleChange}
                  placeholder="What is the customer complaining about?"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              {/* Service Items */}
            <div className="border-t pt-4">
                <div className="flex justify-between items-center mb-2">
                    <h4 className="text-sm font-medium text-gray-700">Services *</h4>
                    <button
                    type="button"
                    onClick={addServiceItem}
                    className="px-3 py-1 text-sm bg-green-600 text-white rounded-md hover:bg-green-700"
                    >
                    + Add Service
                    </button>
                </div>
                
                {formData.items.length === 0 ? (
                    <div className="text-sm text-gray-500 italic">No services added yet</div>
                ) : (
                    <div className="space-y-2">
                    {formData.items.map((item, index) => (
                        <div key={index} className="grid grid-cols-6 gap-2 items-center p-2 bg-gray-50 rounded">
                        <div className="col-span-2">
                            <select
                            value={item.serviceId || ''}
                            onChange={(e) => updateServiceItem(index, 'serviceId', e.target.value)}
                            required
                            className="block w-full px-2 py-1 text-sm border border-gray-300 rounded-md"
                            >
                            <option value="">Select service</option>
                            {services.map((service) => (
                                <option key={service.id} value={service.id}>
                                {service.name} - ₱{service.basePrice.toLocaleString()}
                                </option>
                            ))}
                            </select>
                        </div>
                        <div>
                            <input
                            type="number"
                            value={item.quantity || 1}
                            onChange={(e) => updateServiceItem(index, 'quantity', parseInt(e.target.value) || 1)}
                            min="1"
                            placeholder="Qty"
                            className="block w-full px-2 py-1 text-sm border border-gray-300 rounded-md"
                            />
                        </div>
                        <div>
                            <input
                            type="number"
                            value={item.unitPrice || 0}
                            onChange={(e) => updateServiceItem(index, 'unitPrice', parseFloat(e.target.value) || 0)}
                            placeholder="Price"
                            step="0.01"
                            className="block w-full px-2 py-1 text-sm border border-gray-300 rounded-md"
                            />
                        </div>
                        <div className="text-sm font-medium">
                            ₱{((item.quantity || 0) * (item.unitPrice || 0)).toLocaleString()}
                        </div>
                        <div className="text-right">
                            <button
                            type="button"
                            onClick={() => removeServiceItem(index)}
                            className="text-red-600 hover:text-red-900 text-sm"
                            >
                            Remove
                            </button>
                        </div>
                        </div>
                    ))}
                    </div>
                )}
            </div>

              {/* Grand Total */}
            <div className="border-t pt-4">
                <div className="flex justify-between text-lg font-bold">
                    <span>Grand Total:</span>
                    <span>₱{calculateGrandTotal().toLocaleString('en-PH', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
                </div>
            </div>

              {/* Inspection Notes */}
              <div>
                <label className="block text-sm font-medium text-gray-700">Inspection Notes</label>
                <textarea
                  name="inspectionNotes"
                  rows={2}
                  value={formData.inspectionNotes}
                  onChange={handleChange}
                  placeholder="Mechanic's inspection findings"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              {/* Buttons */}
              <div className="flex justify-end gap-2 pt-4 border-t sticky bottom-0 bg-white">
                <button
                  type="button"
                  onClick={closeModal}
                  className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                >
                  {editingJobOrder ? 'Update' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default JobOrders;