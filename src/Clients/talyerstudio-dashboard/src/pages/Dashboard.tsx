import React from 'react';
import { useAuth } from '../contexts/AuthContext';

const Dashboard: React.FC = () => {
  const { user } = useAuth();

  return (
    <div>
      <h1 className="text-3xl font-bold text-gray-900 mb-6">
        Welcome back, {user?.firstName}! 👋
      </h1>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-blue-500 rounded-md p-3">
              <span className="text-2xl text-white">👥</span>
            </div>
            <div className="ml-5 w-0 flex-1">
              <dl>
                <dt className="text-sm font-medium text-gray-500 truncate">Total Customers</dt>
                <dd className="text-2xl font-semibold text-gray-900">0</dd>
              </dl>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-green-500 rounded-md p-3">
              <span className="text-2xl text-white">📋</span>
            </div>
            <div className="ml-5 w-0 flex-1">
              <dl>
                <dt className="text-sm font-medium text-gray-500 truncate">Active Job Orders</dt>
                <dd className="text-2xl font-semibold text-gray-900">0</dd>
              </dl>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-yellow-500 rounded-md p-3">
              <span className="text-2xl text-white">🧾</span>
            </div>
            <div className="ml-5 w-0 flex-1">
              <dl>
                <dt className="text-sm font-medium text-gray-500 truncate">Pending Invoices</dt>
                <dd className="text-2xl font-semibold text-gray-900">0</dd>
              </dl>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-purple-500 rounded-md p-3">
              <span className="text-2xl text-white">💰</span>
            </div>
            <div className="ml-5 w-0 flex-1">
              <dl>
                <dt className="text-sm font-medium text-gray-500 truncate">Monthly Revenue</dt>
                <dd className="text-2xl font-semibold text-gray-900">₱0</dd>
              </dl>
            </div>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="bg-white rounded-lg shadow p-6">
        <h2 className="text-xl font-semibold text-gray-900 mb-4">Quick Actions</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <button className="p-4 border-2 border-dashed border-gray-300 rounded-lg hover:border-blue-500 hover:bg-blue-50 transition-colors">
            <span className="text-3xl mb-2 block">➕</span>
            <span className="font-medium text-gray-700">Add Customer</span>
          </button>
          <button className="p-4 border-2 border-dashed border-gray-300 rounded-lg hover:border-blue-500 hover:bg-blue-50 transition-colors">
            <span className="text-3xl mb-2 block">📝</span>
            <span className="font-medium text-gray-700">Create Job Order</span>
          </button>
          <button className="p-4 border-2 border-dashed border-gray-300 rounded-lg hover:border-blue-500 hover:bg-blue-50 transition-colors">
            <span className="text-3xl mb-2 block">🧾</span>
            <span className="font-medium text-gray-700">Generate Invoice</span>
          </button>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;