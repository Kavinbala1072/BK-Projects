import React from 'react';

export default function Header({ toggleSidebar }) {
  return (
    <header className="navbar navbar-expand navbar-light bg-white border-bottom px-3 sticky-top shadow-sm" style={{ height: '65px' }}>
      <div className="container-fluid">
        <div className="navbar-brand d-flex align-items-center">
          <div className="d-flex flex-column">
            <span className="fw-bold fs-4 lh-1 text-primary" style={{ letterSpacing: '-1px' }}>BK Softwares</span>
            <span className="text-secondary small fw-medium" style={{ fontSize: '9px', letterSpacing: '1.5px' }}>HP Reports</span>
          </div>
        </div>

        {/* Right Side Icons (Hidden on extra small screens if needed) */}
        <div className="d-flex align-items-center gap-2 ms-auto">
          <div className="px-2 py-2 bg-light rounded-pill border small text-muted">
            <i className="bi bi-calendar3 me-1"></i>
            {new Date().toLocaleDateString('en-GB', { day: 'numeric', month: 'long', year: 'numeric' })}
          </div>
        </div>
      </div>
    </header>
  );
}