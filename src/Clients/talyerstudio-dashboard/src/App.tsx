import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import DashboardLayout from './components/DashboardLayout';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import Customers from './pages/Customers';
import Vehicles from './pages/Vehicles';
import JobOrders from './pages/JobOrders';
import Services from './pages/Services';

function App() {
  return (
    <Router>
      <AuthProvider>
        <Routes>
          {/* Public Routes */}
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          {/* Redirect root to /dashboard */}
          <Route path="/" element={<Navigate to="/dashboard" replace />} />

          {/* Protected Routes - All wrapped in DashboardLayout */}
          <Route
            path="/*"
            element={
              <ProtectedRoute>
                <DashboardLayout />
              </ProtectedRoute>
            }
          >
            {/* Dashboard Home */}
            <Route path="dashboard" element={<Dashboard />} />
            
            {/* Main app routes - Flat structure */}
            <Route path="customers" element={<Customers />} />
            <Route path="vehicles" element={<Vehicles />} />
            <Route path="services" element={<Services />} />
            <Route path="joborders" element={<JobOrders />} />
            <Route path="inventory" element={<div>Inventory Page (Coming Soon)</div>} />
            <Route path="invoices" element={<div>Invoices Page (Coming Soon)</div>} />
            
            {/* 404 - Catch all unknown routes */}
            <Route path="*" element={<Navigate to="/dashboard" replace />} />
          </Route>
        </Routes>
      </AuthProvider>
    </Router>
  );
}

export default App;