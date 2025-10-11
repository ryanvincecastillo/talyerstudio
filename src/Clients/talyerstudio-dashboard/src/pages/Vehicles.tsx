import React, { useState, useEffect } from 'react';
import vehicleService, { type Vehicle, type CreateVehicleRequest } from '../services/vehicleService';
import customerService, { type Customer } from '../services/customerService';
import { useAuth } from '../contexts/AuthContext';

const Vehicles: React.FC = () => {
  const { user } = useAuth();
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingVehicle, setEditingVehicle] = useState<Vehicle | null>(null);

  const [formData, setFormData] = useState<CreateVehicleRequest>({
    tenantId: user?.tenantId || '',
    customerId: '',
    make: '',
    model: '',
    year: new Date().getFullYear(),
    color: '',
    plateNumber: '',
    engineNumber: '',
    chassisNumber: '',
    vehicleType: 'AUTO',
    vehicleCategory: '',
    displacement: '',
    fuelType: 'GASOLINE',
    transmission: 'MANUAL',
    currentOdometer: 0,
    orNumber: '',
    orExpiryDate: '',
    crNumber: '',
    crExpiryDate: '',
    frontTireSize: '',
    rearTireSize: '',
    notes: ''
  });

  useEffect(() => {
    loadVehicles();
    loadCustomers();
  }, []);

  const loadVehicles = async () => {
    try {
      setIsLoading(true);
      const data = await vehicleService.getAll();
      setVehicles(data);
      setError('');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to load vehicles');
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

  const handleSearch = async () => {
    if (!searchTerm) {
      loadVehicles();
      return;
    }

    try {
      setIsLoading(true);
      const data = await vehicleService.search(searchTerm);
      setVehicles(data);
      setError('');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Search failed');
    } finally {
      setIsLoading(false);
    }
  };

  const openModal = (vehicle?: Vehicle) => {
    if (vehicle) {
      setEditingVehicle(vehicle);
      setFormData({
        tenantId: vehicle.tenantId,
        customerId: vehicle.customerId,
        make: vehicle.make,
        model: vehicle.model,
        year: vehicle.year,
        color: vehicle.color || '',
        plateNumber: vehicle.plateNumber,
        engineNumber: vehicle.engineNumber || '',
        chassisNumber: vehicle.chassisNumber || '',
        vehicleType: vehicle.vehicleType,
        vehicleCategory: vehicle.vehicleCategory || '',
        displacement: vehicle.displacement || '',
        fuelType: vehicle.fuelType || 'GASOLINE',
        transmission: vehicle.transmission || 'MANUAL',
        currentOdometer: vehicle.currentOdometer || 0,
        orNumber: vehicle.orNumber || '',
        orExpiryDate: vehicle.orExpiryDate || '',
        crNumber: vehicle.crNumber || '',
        crExpiryDate: vehicle.crExpiryDate || '',
        frontTireSize: vehicle.frontTireSize || '',
        rearTireSize: vehicle.rearTireSize || '',
        notes: vehicle.notes || ''
      });
    } else {
      setEditingVehicle(null);
      setFormData({
        tenantId: user?.tenantId || '',
        customerId: '',
        make: '',
        model: '',
        year: new Date().getFullYear(),
        color: '',
        plateNumber: '',
        engineNumber: '',
        chassisNumber: '',
        vehicleType: 'AUTO',
        vehicleCategory: '',
        displacement: '',
        fuelType: 'GASOLINE',
        transmission: 'MANUAL',
        currentOdometer: 0,
        orNumber: '',
        orExpiryDate: '',
        crNumber: '',
        crExpiryDate: '',
        frontTireSize: '',
        rearTireSize: '',
        notes: ''
      });
    }
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setEditingVehicle(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      if (editingVehicle) {
        await vehicleService.update(editingVehicle.id, { ...formData, id: editingVehicle.id });
      } else {
        await vehicleService.create(formData);
      }
      closeModal();
      loadVehicles();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to save vehicle');
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Are you sure you want to delete this vehicle?')) {
      return;
    }

    try {
      await vehicleService.delete(id);
      loadVehicles();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to delete vehicle');
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: name === 'year' || name === 'currentOdometer' ? parseInt(value) || 0 : value
    });
  };

  const getCustomerName = (customerId: string) => {
    const customer = customers.find(c => c.id === customerId);
    return customer ? `${customer.firstName} ${customer.lastName}` : 'Unknown';
  };

  return (
    <div>
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Vehicles</h1>
        <button
          onClick={() => openModal()}
          className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          + Add Vehicle
        </button>
      </div>

      {/* Error Message */}
      {error && (
        <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}

      {/* Search Bar */}
      <div className="mb-6 flex gap-2">
        <input
          type="text"
          placeholder="Search by plate number, make, model..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
          className="flex-1 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <button
          onClick={handleSearch}
          className="px-6 py-2 bg-gray-600 text-white rounded-md hover:bg-gray-700"
        >
          Search
        </button>
        <button
          onClick={loadVehicles}
          className="px-6 py-2 bg-gray-400 text-white rounded-md hover:bg-gray-500"
        >
          Clear
        </button>
      </div>

      {/* Table */}
      <div className="bg-white rounded-lg shadow overflow-hidden">
        {isLoading ? (
          <div className="p-8 text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          </div>
        ) : vehicles.length === 0 ? (
          <div className="p-8 text-center text-gray-500">
            No vehicles found. Click "Add Vehicle" to create one.
          </div>
        ) : (
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Plate Number</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Vehicle</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Owner</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Type</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Color</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {vehicles.map((vehicle) => (
                <tr key={vehicle.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="font-medium text-gray-900">{vehicle.plateNumber}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm text-gray-900">{vehicle.year} {vehicle.make} {vehicle.model}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {getCustomerName(vehicle.customerId)}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      vehicle.vehicleType === 'MOTORCYCLE' 
                        ? 'bg-orange-100 text-orange-800' 
                        : 'bg-blue-100 text-blue-800'
                    }`}>
                      {vehicle.vehicleType}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {vehicle.color || '-'}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <button
                      onClick={() => openModal(vehicle)}
                      className="text-blue-600 hover:text-blue-900 mr-4"
                    >
                      Edit
                    </button>
                    <button
                      onClick={() => handleDelete(vehicle.id)}
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

      {/* Modal */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-10 mx-auto p-5 border w-full max-w-3xl shadow-lg rounded-md bg-white">
            <div className="flex justify-between items-center mb-4">
              <h3 className="text-lg font-medium text-gray-900">
                {editingVehicle ? 'Edit Vehicle' : 'Add New Vehicle'}
              </h3>
              <button
                onClick={closeModal}
                className="text-gray-400 hover:text-gray-500"
              >
                <span className="text-2xl">×</span>
              </button>
            </div>

            <form onSubmit={handleSubmit} className="space-y-4 max-h-[70vh] overflow-y-auto pr-2">
              {/* Customer Selection */}
              <div>
                <label className="block text-sm font-medium text-gray-700">Owner (Customer) *</label>
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
                      {customer.firstName} {customer.lastName} {customer.phoneNumber ? `(${customer.phoneNumber})` : ''}
                    </option>
                  ))}
                </select>
              </div>

              {/* Vehicle Type */}
              <div>
                <label className="block text-sm font-medium text-gray-700">Vehicle Type *</label>
                <select
                  name="vehicleType"
                  required
                  value={formData.vehicleType}
                  onChange={handleChange}
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                >
                  <option value="AUTO">Auto / Car</option>
                  <option value="MOTORCYCLE">Motorcycle</option>
                </select>
              </div>

              {/* Basic Info */}
              <div className="grid grid-cols-3 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Make *</label>
                  <input
                    type="text"
                    name="make"
                    required
                    value={formData.make}
                    onChange={handleChange}
                    placeholder="e.g., Toyota, Honda"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Model *</label>
                  <input
                    type="text"
                    name="model"
                    required
                    value={formData.model}
                    onChange={handleChange}
                    placeholder="e.g., Vios, Click"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Year *</label>
                  <input
                    type="number"
                    name="year"
                    required
                    value={formData.year}
                    onChange={handleChange}
                    min="1900"
                    max={new Date().getFullYear() + 1}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              {/* Plate & Color */}
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Plate Number *</label>
                  <input
                    type="text"
                    name="plateNumber"
                    required
                    value={formData.plateNumber}
                    onChange={handleChange}
                    placeholder="ABC1234"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Color</label>
                  <input
                    type="text"
                    name="color"
                    value={formData.color}
                    onChange={handleChange}
                    placeholder="e.g., White, Black"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              {/* Engine & Chassis */}
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Engine Number</label>
                  <input
                    type="text"
                    name="engineNumber"
                    value={formData.engineNumber}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Chassis Number</label>
                  <input
                    type="text"
                    name="chassisNumber"
                    value={formData.chassisNumber}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              {/* Specs */}
              <div className="grid grid-cols-3 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Fuel Type</label>
                  <select
                    name="fuelType"
                    value={formData.fuelType}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="GASOLINE">Gasoline</option>
                    <option value="DIESEL">Diesel</option>
                    <option value="ELECTRIC">Electric</option>
                    <option value="HYBRID">Hybrid</option>
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Transmission</label>
                  <select
                    name="transmission"
                    value={formData.transmission}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="MANUAL">Manual</option>
                    <option value="AUTOMATIC">Automatic</option>
                    <option value="CVT">CVT</option>
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700">Odometer (km)</label>
                  <input
                    type="number"
                    name="currentOdometer"
                    value={formData.currentOdometer}
                    onChange={handleChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              {/* Philippines-specific: OR/CR */}
              <div className="border-t pt-4">
                <h4 className="text-sm font-medium text-gray-700 mb-2">Registration (Philippines)</h4>
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700">OR Number</label>
                    <input
                      type="text"
                      name="orNumber"
                      value={formData.orNumber}
                      onChange={handleChange}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700">OR Expiry Date</label>
                    <input
                      type="date"
                      name="orExpiryDate"
                      value={formData.orExpiryDate}
                      onChange={handleChange}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700">CR Number</label>
                    <input
                      type="text"
                      name="crNumber"
                      value={formData.crNumber}
                      onChange={handleChange}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700">CR Expiry Date</label>
                    <input
                      type="date"
                      name="crExpiryDate"
                      value={formData.crExpiryDate}
                      onChange={handleChange}
                      className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                </div>
              </div>

              {/* Motorcycle-specific: Tire Sizes */}
              {formData.vehicleType === 'MOTORCYCLE' && (
                <div className="border-t pt-4">
                  <h4 className="text-sm font-medium text-gray-700 mb-2">Motorcycle Specifics</h4>
                  <div className="grid grid-cols-3 gap-4">
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Displacement</label>
                      <input
                        type="text"
                        name="displacement"
                        value={formData.displacement}
                        onChange={handleChange}
                        placeholder="e.g., 125cc, 150cc"
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Front Tire Size</label>
                      <input
                        type="text"
                        name="frontTireSize"
                        value={formData.frontTireSize}
                        onChange={handleChange}
                        placeholder="e.g., 70/90-17"
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Rear Tire Size</label>
                      <input
                        type="text"
                        name="rearTireSize"
                        value={formData.rearTireSize}
                        onChange={handleChange}
                        placeholder="e.g., 80/90-17"
                        className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                      />
                    </div>
                  </div>
                </div>
              )}

              {/* Notes */}
              <div>
                <label className="block text-sm font-medium text-gray-700">Notes</label>
                <textarea
                  name="notes"
                  rows={3}
                  value={formData.notes}
                  onChange={handleChange}
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
                  {editingVehicle ? 'Update' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Vehicles;