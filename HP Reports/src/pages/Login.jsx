import React, { useState, useEffect } from 'react';

export default function Login({ onLogin }) {
  const [user, setUser] = useState('');
  const [pass, setPass] = useState('');
  const [error, setError] = useState('');
  const [validCredentials, setValidCredentials] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetch('/config.json')
      .then((response) => response.json())
      .then((data) => setValidCredentials(data.auth))
      .catch((err) => {
        console.error("Error loading auth config:", err);
        setError("System Error: Configuration file missing.");
      });
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    // Artificial delay for "Professional Feel"
    setTimeout(() => {
      if (!validCredentials) {
        setError('Configuration not loaded.');
        setLoading(false);
        return;
      }

      if (user === validCredentials.username && pass === validCredentials.password) {
        onLogin();
      } else {
        setError('Invalid username or password');
        setLoading(false);
      }
    }, 800);
  };

  return (
    <div className="container-fluid vh-100 d-flex align-items-center justify-content-center" 
         style={{ 
           background: 'linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%)',
           fontFamily: "'Inter', sans-serif" 
         }}>
      
      <div className="card border-0 shadow-lg p-2" style={{ maxWidth: '420px', width: '100%', borderRadius: '20px' }}>
        <div className="card-body p-4 p-sm-5">
          
          {/* Logo / Branding Section */}
          <div className="text-center mb-5">
            <div className="d-inline-flex align-items-center justify-content-center bg-primary rounded-circle shadow-sm mb-3" 
                 style={{ width: '60px', height: '60px' }}>
              <i className="bi bi-shield-lock-fill text-white fs-2"></i>
            </div>
            <h3 className="fw-bold text-dark mb-1">Welcome Back</h3>
            <p className="text-muted small">Enter your credentials to access HP Reports</p>
          </div>

          {/* Error Message */}
          {error && (
            <div className="alert alert-danger d-flex align-items-center py-2 border-0 mb-4" 
                 style={{ fontSize: '13px', borderRadius: '10px' }}>
              <i className="bi bi-exclamation-circle-fill me-2"></i>
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit}>
            {/* Username with Icon */}
            <div className="mb-4">
              <label className="form-label small fw-semibold text-secondary">Username</label>
              <div className="input-group">
                <span className="input-group-text bg-light border-0">
                  <i className="bi bi-person text-secondary"></i>
                </span>
                <input 
                  type="text" 
                  className="form-control bg-light border-0 py-2" 
                  placeholder="Enter username"
                  autoComplete="username"
                  required
                  onChange={(e) => setUser(e.target.value)} 
                />
              </div>
            </div>

            {/* Password with Icon */}
            <div className="mb-4">
              <div className="d-flex justify-content-between">
                <label className="form-label small fw-semibold text-secondary">Password</label>
              </div>
              <div className="input-group">
                <span className="input-group-text bg-light border-0">
                  <i className="bi bi-lock text-secondary"></i>
                </span>
                <input 
                  type="password" 
                  className="form-control bg-light border-0 py-2" 
                  placeholder="Enter password"
                  autoComplete="current-password"
                  required
                  onChange={(e) => setPass(e.target.value)} 
                />
              </div>
            </div>

            {/* Remember Me */}
            <div className="mb-4 form-check">
              <input type="checkbox" className="form-check-input" id="rememberMe" />
              <label className="form-check-label small text-secondary" htmlFor="rememberMe">Keep me logged in</label>
            </div>

            {/* Login Button */}
            <button 
              type="submit" 
              className="btn btn-primary w-100 py-2 fw-bold shadow-sm" 
              style={{ borderRadius: '12px', transition: '0.3s' }}
              disabled={loading}
            >
              {loading ? (
                <span className="spinner-border spinner-border-sm me-2"></span>
              ) : ("Sign In")}
            </button>
          <div className="text-center mt-1">
            <a href="#" className="text-primary text-decoration-none small" style={{ fontSize: '12px' }}>Forgot?</a>
          </div>
          </form>

          {/* Footer Info */}
          <div className="text-center mt-3">
            <p className="text-muted mb-0" style={{ fontSize: '12px' }}>Version 0.0.1</p>
          </div>
        </div>
      </div>
    </div>
  );
}