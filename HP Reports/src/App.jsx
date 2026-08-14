import React, { useState, useEffect } from 'react';
import { Routes, Route, Navigate, useNavigate } from 'react-router-dom';

import Login from './pages/Login.jsx';
import DBConfig from './pages/DBConfig.jsx';
import Dashboard from './pages/Dashboard.jsx';
import ReportPage from './pages/ReportPage.jsx';
import Sidebar from './components/Sidebar.jsx';
import Header from './components/Header.jsx';

export default function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isDbConfigured, setIsDbConfigured] = useState(false);
  
  // Sidebar States
  const [isCollapsed, setIsCollapsed] = useState(false); // For Desktop
  const [isMobileOpen, setIsMobileOpen] = useState(false); // For Mobile

  
  const navigate = useNavigate();

  useEffect(() => {
    const auth = localStorage.getItem('isLoggedIn');
    const db = localStorage.getItem('isDbConfigured');
    if (auth === 'true') setIsAuthenticated(true);
    if (db === 'true') setIsDbConfigured(true);
  }, []);

  const handleLogin = () => {
    setIsAuthenticated(true);
    localStorage.setItem('isLoggedIn', 'true');
    // navigate('/db-setup'); 
    navigate('/');
  };

  // const handleDbComplete = () => {
  //   setIsDbConfigured(true);
  //   localStorage.setItem('isDbConfigured', 'true');
  //   navigate('/'); // Finally go to Dashboard
  // };

  const handleLogout = () => {
    setIsAuthenticated(false);
    setIsDbConfigured(false);
    localStorage.clear();
    navigate('/login');
  };

  // 1. LOGIN SCREEN
  if (!isAuthenticated) {
    return (
      <Routes>
        <Route path="/login" element={<Login onLogin={handleLogin} />} />
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    );
  }

  // 2. DB CONFIG SCREEN (Shows after login, before dashboard)
  // if (!isDbConfigured) {
  //   return (
  //     <div className="bg-light vh-100 p-0 p-md-4">
  //       <Routes>
  //         <Route path="/db-setup" element={<DBConfig onComplete={handleDbComplete} isInitialSetup={true} />} />
  //         <Route path="*" element={<Navigate to="/db-setup" />} />
  //       </Routes>
  //     </div>
  //   );
  // }

  // 3. MAIN APPLICATION LAYOUT
// App.jsx


return (
  <div className="d-flex flex-column vh-100 overflow-hidden">
    {/* The Header button sets isMobileOpen to TRUE */}
    <Header toggleSidebar={() => setIsMobileOpen(true)} /> 

    <div className="d-flex flex-grow-1 overflow-hidden">
      <Sidebar 
        isOpen={isMobileOpen} // Pass the false state
        closeMobile={() => setIsMobileOpen(false)} // Function to hide it
        isCollapsed={isCollapsed}
        toggleCollapse={() => setIsCollapsed(!isCollapsed)}
        onLogout={handleLogout}
      />
      <main className="flex-grow-1 bg-light overflow-auto p-3">
       <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/reports/:type" element={<ReportPage />} />
            <Route path="/config" element={<DBConfig onComplete={() => {}} isInitialSetup={false} />} />
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
      </main>
    </div>
  </div>
);
}