import React, { useState } from 'react';

export default function DBConfig({ onComplete, isInitialSetup }) {
  const [isLocalMode, setIsLocalMode] = useState(true);
  const [isTesting, setIsTesting] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');

  const [config, setConfig] = useState({
    server: 'ABC/SQLEXPRESS',
    port: '',
    database: 'DB',
    username: 'sa',
    password: 'abc123'
  });

  const toggleMode = (mode) => {
    setIsLocalMode(mode);
    setConfig(mode ? {
      server: 'ABC/SQLEXPRESS', port: '', database: 'DB', username: 'sa', password: 'abc123'
    } : {
      server: '192.168.423.43', port: '1433', database: 'DB', username: 'sa', password: 'abc123'
    });
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setConfig({ ...config, [name]: value });
  };

  const handleConnect = async (e) => {
    e.preventDefault();
    setIsTesting(true);
    setErrorMessage('');
    setSuccessMessage('');

    try {
      // 1. Attempt to contact the Gateway Service
      const response = await fetch('http://localhost:5000/api/test-connection', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ...config, mode: isLocalMode ? 'Local' : 'Web' })
      });

      // 2. Parse the result from SQL Server
      const result = await response.json();

      if (response.ok && result.success) {
        // SUCCESS: Show message, save config, and route to Dashboard
        setSuccessMessage(`Success! Connection established to ${config.server}`);
        localStorage.setItem('sql_config', JSON.stringify(config));
        localStorage.setItem('isDbConfigured', 'true'); // Required for App.jsx routing
        
        setTimeout(() => {
          onComplete(); // This triggers the page change in App.jsx
        }, 1200);
      } else {
        // SQL ERROR: Show the specific error from SQL Server (e.g. Invalid Password)
        setErrorMessage(result.message || "Database login failed.");
      }
    } catch (err) {
      // NETWORK ERROR: Gateway service is not running
      setErrorMessage("Connection Failed: Ensure the SQL Gateway Service is running on Port 5000.");
    } finally {
      setIsTesting(false);
    }
  };

  return (
    <div className="container h-100 d-flex flex-column justify-content-center align-items-center py-5" style={{fontFamily: "'Inter', sans-serif"}}>
      <div className="card shadow-lg border-0" style={{ maxWidth: '500px', width: '100%', borderRadius: '20px' }}>
        
        <div className="card-header bg-white border-0 pt-4 px-4 d-flex justify-content-between align-items-center">
          <h5 className="fw-bold mb-0">SQL Connection</h5>
          <div className="btn-group btn-group-sm bg-light rounded-pill p-1">
            <button className={`btn rounded-pill px-3 ${isLocalMode ? 'btn-primary shadow-sm' : 'btn-light'}`} onClick={() => toggleMode(true)}>Local</button>
            <button className={`btn rounded-pill px-3 ${!isLocalMode ? 'btn-primary shadow-sm' : 'btn-light'}`} onClick={() => toggleMode(false)}>Web</button>
          </div>
        </div>

        <div className="card-body p-4 pt-2">
          {errorMessage && <div className="alert alert-danger border-0 small py-2 mb-3"><i className="bi bi-x-circle me-2"></i>{errorMessage}</div>}
          {successMessage && <div className="alert alert-success border-0 small py-2 mb-3"><i className="bi bi-check-circle me-2"></i>{successMessage}</div>}

          <form onSubmit={handleConnect}>
            <div className="row g-3">
              <div className={isLocalMode ? "col-12" : "col-md-8"}>
                <label className="form-label small fw-bold">Server / Host</label>
                <input type="text" name="server" className="form-control bg-light border-0" value={config.server} onChange={handleInputChange} required />
              </div>
              {!isLocalMode && (
                <div className="col-md-4">
                  <label className="form-label small fw-bold">Port</label>
                  <input type="text" name="port" className="form-control bg-light border-0" value={config.port} onChange={handleInputChange} />
                </div>
              )}
              <div className="col-12">
                <label className="form-label small fw-bold">Database</label>
                <input type="text" name="database" className="form-control bg-light border-0" value={config.database} onChange={handleInputChange} required />
              </div>
              <div className="col-md-6">
                <label className="form-label small fw-bold">SQL Username</label>
                <input type="text" name="username" className="form-control bg-light border-0" value={config.username} onChange={handleInputChange} required />
              </div>
              <div className="col-md-6">
                <label className="form-label small fw-bold">SQL Password</label>
                <input type="password" name="password" className="form-control bg-light border-0" value={config.password} onChange={handleInputChange} required />
              </div>
              <div className="col-12 mt-4">
                <button type="submit" className="btn btn-primary w-100 py-2 fw-bold shadow-sm" disabled={isTesting} style={{borderRadius: '10px'}}>
                  {isTesting ? <><span className="spinner-border spinner-border-sm me-2"></span>Connecting...</> : "Verify & Open Dashboard"}
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}