import React from 'react';

export default function Dashboard() {
  return (
    <div className="container-fluid py-5">
      <div className="row justify-content-center align-items-center" style={{ minHeight: '70vh' }}>
        <div className="col-md-8 text-center">
          {/* Welcome Text */}
          <div className="card border-0 shadow-sm p-5 rounded-4 bg-white">
            <h1 className="display-5 fw-bold text-dark mb-3">
              Welcome to <span className="text-primary">HP Reports</span>
            </h1>
            <p className="text-secondary mb-4">
              Your centralized portal for Due Pending reports and database analytics.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}