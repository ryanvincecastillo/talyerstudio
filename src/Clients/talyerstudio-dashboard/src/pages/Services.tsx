import React, { useState, useEffect } from 'react';
import serviceService, { 
  type Service, 
  type ServiceCategory, 
  type CreateServiceRequest,
  type CreateServiceCategoryRequest 
} from '../services/serviceService';
import { useAuth } from '../contexts/AuthContext';

const Services: React.FC = () => {
  const { user } = useAuth();
  const [services, setServices] = useState<Service[]>([]);
  const [categories, setCategories] = useState<ServiceCategory[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [activeTab, setActiveTab] = useState<'services' | 'categories'>('services');
  
  // Service Modal
  const [isServiceModalOpen, setIsServiceModalOpen] = useState(false);
  const [editingService, setEditingService] = useState<Service | null>(null);
  const [serviceFormData, setServiceFormData] = useState<CreateServiceRequest>({
    tenantId: user?.tenantId || '',
    name: '',
    description: '',
    categoryId: '',
    basePrice: 0,
    currency: 'PHP',
    applicability: 'BOTH',
    estimatedDurationMinutes: 60,
    isActive: true,
    displayOrder: 0
  });

  // Category Modal
  const [isCategoryModalOpen, setIsCategoryModalOpen] = useState(false);
  const [editingCategory, setEditingCategory] = useState<ServiceCategory | null>(null);
  const [categoryFormData, setCategoryFormData] = useState<CreateServiceCategoryRequest>({
    tenantId: user?.tenantId || '',
    name: '',
    description: '',
    icon: '🔧',
    displayOrder: 0,
    isActive: true
  });

  useEffect(() => {
    loadServices();
    loadCategories();
  }, []);

  const loadServices = async () => {
    try {
      setIsLoading(true);
      const data = await serviceService.getAll();
      setServices(data);
      setError('');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to load services');
    } finally {
      setIsLoading(false);
    }
  };

  const loadCategories = async () => {
    try {
      const data = await serviceService.getCategories();
      setCategories(data);
    } catch (err: any) {
      console.error('Failed to load categories:', err);
    }
  };

  // SERVICE HANDLERS
  const openServiceModal = (service?: Service) => {
    if (service) {
      setEditingService(service);
      setServiceFormData({
        tenantId: service.tenantId,
        name: service.name,
        description: service.description || '',
        categoryId: service.categoryId,
        basePrice: service.basePrice,
        currency: service.currency,
        applicability: service.applicability,
        estimatedDurationMinutes: service.estimatedDurationMinutes || 60,
        isActive: service.isActive,
        displayOrder: service.displayOrder || 0
      });
    } else {
      setEditingService(null);
      setServiceFormData({
        tenantId: user?.tenantId || '',
        name: '',
        description: '',
        categoryId: '',
        basePrice: 0,
        currency: 'PHP',
        applicability: 'BOTH',
        estimatedDurationMinutes: 60,
        isActive: true,
        displayOrder: 0
      });
    }
    setIsServiceModalOpen(true);
  };

  const closeServiceModal = () => {
    setIsServiceModalOpen(false);
    setEditingService(null);
  };

  const handleServiceSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      if (editingService) {
        await serviceService.update(editingService.id, { ...serviceFormData, id: editingService.id });
      } else {
        await serviceService.create(serviceFormData);
      }
      closeServiceModal();
      loadServices();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to save service');
    }
  };

  const handleServiceDelete = async (id: string) => {
    if (!window.confirm('Are you sure you want to delete this service?')) {
      return;
    }

    try {
      await serviceService.delete(id);
      loadServices();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to delete service');
    }
  };

  const handleServiceChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    setServiceFormData({
      ...serviceFormData,
      [name]: type === 'checkbox' 
        ? (e.target as HTMLInputElement).checked 
        : name === 'basePrice' || name === 'estimatedDurationMinutes' || name === 'displayOrder'
          ? parseFloat(value) || 0
          : value
    });
  };

  // CATEGORY HANDLERS
  const openCategoryModal = (category?: ServiceCategory) => {
    if (category) {
      setEditingCategory(category);
      setCategoryFormData({
        tenantId: category.tenantId,
        name: category.name,
        description: category.description || '',
        icon: category.icon || '🔧',
        displayOrder: category.displayOrder || 0,
        isActive: category.isActive
      });
    } else {
      setEditingCategory(null);
      setCategoryFormData({
        tenantId: user?.tenantId || '',
        name: '',
        description: '',
        icon: '🔧',
        displayOrder: 0,
        isActive: true
      });
    }
    setIsCategoryModalOpen(true);
  };

  const closeCategoryModal = () => {
    setIsCategoryModalOpen(false);
    setEditingCategory(null);
  };

  const handleCategorySubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    try {
      if (editingCategory) {
        await serviceService.updateCategory(editingCategory.id, { ...categoryFormData, id: editingCategory.id });
      } else {
        await serviceService.createCategory(categoryFormData);
      }
      closeCategoryModal();
      loadCategories();
      loadServices(); // Reload to update category names
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to save category');
    }
  };

  const handleCategoryDelete = async (id: string) => {
    if (!window.confirm('Are you sure you want to delete this category? Services in this category will need to be reassigned.')) {
      return;
    }

    try {
      await serviceService.deleteCategory(id);
      loadCategories();
      loadServices();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to delete category');
    }
  };

  const handleCategoryChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    setCategoryFormData({
      ...categoryFormData,
      [name]: type === 'checkbox' 
        ? (e.target as HTMLInputElement).checked 
        : name === 'displayOrder'
          ? parseInt(value) || 0
          : value
    });
  };

  const getCategoryName = (categoryId: string) => {
    const category = categories.find(c => c.id === categoryId);
    return category ? category.name : 'Unknown';
  };

  return (
    <div>
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Service Catalog</h1>
        <div className="flex gap-2">
          <button
            onClick={() => openCategoryModal()}
            className="px-4 py-2 bg-purple-600 text-white rounded-md hover:bg-purple-700 focus:outline-none focus:ring-2 focus:ring-purple-500"
          >
            + Add Category
          </button>
          <button
            onClick={() => openServiceModal()}
            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            + Add Service
          </button>
        </div>
      </div>

      {/* Error Message */}
      {error && (
        <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}

      {/* Tabs */}
      <div className="mb-6 border-b border-gray-200">
        <nav className="-mb-px flex space-x-8">
          <button
            onClick={() => setActiveTab('services')}
            className={`${
              activeTab === 'services'
                ? 'border-blue-500 text-blue-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            } whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm`}
          >
            Services ({services.length})
          </button>
          <button
            onClick={() => setActiveTab('categories')}
            className={`${
              activeTab === 'categories'
                ? 'border-blue-500 text-blue-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            } whitespace-nowrap py-4 px-1 border-b-2 font-medium text-sm`}
          >
            Categories ({categories.length})
          </button>
        </nav>
      </div>

      {/* SERVICES TAB */}
      {activeTab === 'services' && (
        <div className="bg-white rounded-lg shadow overflow-hidden">
          {isLoading ? (
            <div className="p-8 text-center">
              <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
            </div>
          ) : services.length === 0 ? (
            <div className="p-8 text-center text-gray-500">
              No services found. Click "Add Service" to create one.
            </div>
          ) : (
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Service Name</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Category</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Price</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Duration</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Applicability</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                  <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {services.map((service) => (
                  <tr key={service.id} className="hover:bg-gray-50">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="font-medium text-gray-900">{service.name}</div>
                      <div className="text-sm text-gray-500">{service.description}</div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {service.categoryName || getCategoryName(service.categoryId)}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                      ₱{service.basePrice.toLocaleString()}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {service.estimatedDurationMinutes} min
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                        service.applicability === 'AUTO' 
                          ? 'bg-blue-100 text-blue-800'
                          : service.applicability === 'MOTORCYCLE'
                            ? 'bg-orange-100 text-orange-800'
                            : 'bg-green-100 text-green-800'
                      }`}>
                        {service.applicability}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                        service.isActive 
                          ? 'bg-green-100 text-green-800' 
                          : 'bg-gray-100 text-gray-800'
                      }`}>
                        {service.isActive ? 'Active' : 'Inactive'}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <button
                        onClick={() => openServiceModal(service)}
                        className="text-blue-600 hover:text-blue-900 mr-4"
                      >
                        Edit
                      </button>
                      <button
                        onClick={() => handleServiceDelete(service.id)}
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
      )}

      {/* CATEGORIES TAB */}
      {activeTab === 'categories' && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {categories.map((category) => (
            <div key={category.id} className="bg-white rounded-lg shadow p-6 hover:shadow-md transition-shadow">
              <div className="flex items-start justify-between">
                <div className="flex items-center">
                  <span className="text-4xl mr-3">{category.icon}</span>
                  <div>
                    <h3 className="text-lg font-semibold text-gray-900">{category.name}</h3>
                    <p className="text-sm text-gray-500">{category.description}</p>
                    <span className={`mt-2 inline-block px-2 py-1 text-xs font-semibold rounded-full ${
                      category.isActive 
                        ? 'bg-green-100 text-green-800' 
                        : 'bg-gray-100 text-gray-800'
                    }`}>
                      {category.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </div>
                </div>
              </div>
              <div className="mt-4 flex justify-end gap-2">
                <button
                  onClick={() => openCategoryModal(category)}
                  className="text-blue-600 hover:text-blue-900 text-sm font-medium"
                >
                  Edit
                </button>
                <button
                  onClick={() => handleCategoryDelete(category.id)}
                  className="text-red-600 hover:text-red-900 text-sm font-medium"
                >
                  Delete
                </button>
              </div>
            </div>
          ))}
          {categories.length === 0 && (
            <div className="col-span-3 p-8 text-center text-gray-500">
              No categories found. Click "Add Category" to create one.
            </div>
          )}
        </div>
      )}

      {/* SERVICE MODAL */}
      {isServiceModalOpen && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-full max-w-2xl shadow-lg rounded-md bg-white">
            <div className="flex justify-between items-center mb-4">
              <h3 className="text-lg font-medium text-gray-900">
                {editingService ? 'Edit Service' : 'Add New Service'}
              </h3>
              <button
                onClick={closeServiceModal}
                className="text-gray-400 hover:text-gray-500"
              >
                <span className="text-2xl">×</span>
              </button>
            </div>

            <form onSubmit={handleServiceSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Service Name *</label>
                <input
                  type="text"
                  name="name"
                  required
                  value={serviceFormData.name}
                  onChange={handleServiceChange}
                  placeholder="e.g., Oil Change, Brake Service"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Description</label>
                <textarea
                  name="description"
                  rows={2}
                  value={serviceFormData.description}
                  onChange={handleServiceChange}
                  placeholder="Brief description of the service"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Category *</label>
                  <select
                    name="categoryId"
                    required
                    value={serviceFormData.categoryId}
                    onChange={handleServiceChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="">Select a category</option>
                    {categories.filter(c => c.isActive).map((category) => (
                      <option key={category.id} value={category.id}>
                        {category.icon} {category.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700">Base Price (₱) *</label>
                  <input
                    type="number"
                    name="basePrice"
                    required
                    value={serviceFormData.basePrice}
                    onChange={handleServiceChange}
                    min="0"
                    step="0.01"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">Applicability *</label>
                  <select
                    name="applicability"
                    required
                    value={serviceFormData.applicability}
                    onChange={handleServiceChange}
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="BOTH">Both (Auto & Motorcycle)</option>
                    <option value="AUTO">Auto Only</option>
                    <option value="MOTORCYCLE">Motorcycle Only</option>
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700">Estimated Duration (minutes)</label>
                  <input
                    type="number"
                    name="estimatedDurationMinutes"
                    value={serviceFormData.estimatedDurationMinutes}
                    onChange={handleServiceChange}
                    min="0"
                    className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
              </div>

              <div className="flex items-center">
                <input
                  type="checkbox"
                  name="isActive"
                  checked={serviceFormData.isActive}
                  onChange={handleServiceChange}
                  className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                />
                <label className="ml-2 block text-sm text-gray-900">
                  Active (Available for selection)
                </label>
              </div>

              <div className="flex justify-end gap-2 pt-4">
                <button
                  type="button"
                  onClick={closeServiceModal}
                  className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                >
                  {editingService ? 'Update' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* CATEGORY MODAL */}
      {isCategoryModalOpen && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-full max-w-md shadow-lg rounded-md bg-white">
            <div className="flex justify-between items-center mb-4">
              <h3 className="text-lg font-medium text-gray-900">
                {editingCategory ? 'Edit Category' : 'Add New Category'}
              </h3>
              <button
                onClick={closeCategoryModal}
                className="text-gray-400 hover:text-gray-500"
              >
                <span className="text-2xl">×</span>
              </button>
            </div>

            <form onSubmit={handleCategorySubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">Category Name *</label>
                <input
                  type="text"
                  name="name"
                  required
                  value={categoryFormData.name}
                  onChange={handleCategoryChange}
                  placeholder="e.g., Engine Services, Brake System"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Description</label>
                <textarea
                  name="description"
                  rows={2}
                  value={categoryFormData.description}
                  onChange={handleCategoryChange}
                  placeholder="Brief description of the category"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700">Icon (Emoji)</label>
                <input
                  type="text"
                  name="icon"
                  value={categoryFormData.icon}
                  onChange={handleCategoryChange}
                  placeholder="🔧"
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                />
                <p className="mt-1 text-xs text-gray-500">Common emojis: 🔧 🔩 ⚙️ 🛠️ 🚗 🏍️ 🔋 💨 🛞</p>
              </div>

              <div className="flex items-center">
                <input
                  type="checkbox"
                  name="isActive"
                  checked={categoryFormData.isActive}
                  onChange={handleCategoryChange}
                  className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                />
                <label className="ml-2 block text-sm text-gray-900">
                  Active
                </label>
              </div>

              <div className="flex justify-end gap-2 pt-4">
                <button
                  type="button"
                  onClick={closeCategoryModal}
                  className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 bg-purple-600 text-white rounded-md hover:bg-purple-700"
                >
                  {editingCategory ? 'Update' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Services;