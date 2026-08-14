import React from 'react';
import { Link, useLocation } from 'react-router-dom';

export default function Sidebar({ onLogout, isCollapsed, toggleCollapse, isOpen, closeMobile }) {
  const location = useLocation();
  
  // Dynamic width based on collapse state
  const sidebarWidth = isCollapsed ? '75px' : '260px';

  const getLinkClass = (path) => {
    const isActive = location.pathname === path;
    return `nav-link d-flex align-items-center py-3 border-start border-4 ${
      isCollapsed ? 'justify-content-center px-0' : 'px-4'
    } ${isActive ? 'bg-primary-subtle text-primary border-primary fw-bold' : 'text-dark border-white border-transparent'}`;
  };

  return (
    <>
      {/* Mobile Overlay - Only visible when the sidebar is slid out on mobile */}
      {isOpen && <div className="sidebar-overlay d-md-none" onClick={closeMobile}></div>}

      <div 
        className={`sidebar-container bg-white border-end shadow-sm d-flex flex-column transition-all ${isOpen ? 'mobile-open' : 'mobile-closed'}`} 
        style={{ width: sidebarWidth }}
      >
        <ul className="nav nav-pills flex-column mb-auto pt-2 overflow-hidden">
          <li className="nav-item">
            <Link to="/" className={getLinkClass('/')} onClick={() => window.innerWidth < 768 && !isCollapsed ? null : closeMobile}>
              <i className="bi bi-grid fs-5"></i>
              {!isCollapsed && <span className="ms-3">Home</span>}
            </Link>
          </li>

          <li className="nav-item">
            <Link to="/reports/sales" className={getLinkClass('/reports/sales')} onClick={() => window.innerWidth < 768 && !isCollapsed ? null : closeMobile}>
              <i className="bi bi-file-earmark-text fs-5"></i>
              {!isCollapsed && <span className="ms-3 text-nowrap">Due Pending</span>}
            </Link>
          </li>
        </ul>

        {/* Bottom Actions */}
        <div className="mt-auto border-top">
          <button onClick={onLogout} className={`btn btn-link text-danger text-decoration-none d-flex align-items-center w-100 py-3 ${isCollapsed ? 'justify-content-center px-0' : 'px-4'}`}>
            <i className="bi bi-power fs-5"></i>
            {!isCollapsed && <span className="ms-3 fw-bold small">Sign Out</span>}
          </button>
          
          {/* Toggle Button: Always triggers toggleCollapse now */}
          <button 
            onClick={toggleCollapse} 
            className="btn btn-light w-100 rounded-0 border-top py-2 text-secondary"
          >
            <i className={`bi ${isCollapsed ? 'bi-text-indent-right' : 'bi-text-indent-left'} fs-5`}></i>
          </button>
        </div>
      </div>
    </>
  );
}