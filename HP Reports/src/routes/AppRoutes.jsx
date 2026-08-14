import { Routes, Route, Navigate } from 'react-router-dom';
import Login from '../pages/Login';
import Dashboard from '../pages/Dashboard';
import Layout from '../components/Layout/Layout';
import SalesReport from '../pages/Reports/SalesReport';
import DBConfig from '../pages/DatabaseConfig';

const PrivateRoute = ({ children }) => {
  const isAuthenticated = localStorage.getItem('auth');
  return isAuthenticated ? children : <Navigate to="/login" />;
};

export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/" element={<PrivateRoute><Layout /></PrivateRoute>}>
        <Route index element={<Dashboard />} />
        <Route path="database-config" element={<DBConfig />} />
        <Route path="reports/sales" element={<SalesReport />} />
        {/* Add other report routes here */}
      </Route>
    </Routes>
  );
}