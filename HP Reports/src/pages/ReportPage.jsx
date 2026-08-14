import React, { useState, useMemo } from 'react';

export default function ReportPage({ type = "Sales" }) {
  const [currentPage, setCurrentPage] = useState(1);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [searchQuery, setSearchQuery] = useState('');

  // Dummy Data Generation
  const allRecords = useMemo(() => {
    return Array.from({ length: 55 }, (_, i) => ({
      id: i + 1,
      date: '22 Aug 2024',
      item: i % 3 === 0 ? 'HP LaserJet Pro M404n' : 'Canon ImageClass MF',
      ref: `#INV-990${10 + i}`,
      customer: i % 2 === 0 ? 'Global Solutions Ltd' : 'Tech Dynamics Inc',
      qty: Math.floor(Math.random() * 10) + 1,
      status: i % 2 === 0 ? 'Completed' : 'Pending',
      amount: (Math.random() * 2000 + 500).toFixed(2)
    }));
  }, []);

  const filteredData = allRecords.filter(item => 
    item.item.toLowerCase().includes(searchQuery.toLowerCase()) ||
    item.customer.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const totalRecords = filteredData.length;
  const effectiveRowsPerPage = rowsPerPage === 'All' ? totalRecords : parseInt(rowsPerPage);
  const totalPages = Math.ceil(totalRecords / effectiveRowsPerPage);
  const indexOfLastRecord = currentPage * effectiveRowsPerPage;
  const indexOfFirstRecord = indexOfLastRecord - effectiveRowsPerPage;
  const currentRecords = filteredData.slice(indexOfFirstRecord, indexOfLastRecord);

  const handlePageChange = (page) => page >= 1 && page <= totalPages && setCurrentPage(page);

  return (
    <div className="container-fluid py-3 py-md-4" style={{ fontFamily: "'Inter', sans-serif" }}>
      
      {/* 1. Header: Stacks on mobile */}
      <div className="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center mb-4">
        <h4 className="fw-bold text-dark mb-3 mb-md-0">{type} Report</h4>
        <div className="d-flex align-items-center gap-2 w-100 w-md-auto">
           <span className="small text-secondary fw-bold text-nowrap">Show</span>
           <select 
             className="form-select form-select-sm shadow-sm" 
             style={{ width: '80px' }}
             value={rowsPerPage}
             onChange={(e) => {setRowsPerPage(e.target.value); setCurrentPage(1);}}
           >
             <option value="10">10</option>
             <option value="20">20</option>
             <option value="50">50</option>
             <option value="All">All</option>
           </select>
           <span className="small text-secondary fw-bold text-nowrap">Entries</span>
        </div>
      </div>

      {/* 2. Filter Bar: Full width inputs on mobile */}
      <div className="card border-0 shadow-sm mb-4 rounded-4">
        <div className="card-body p-3">
          <div className="row g-3 align-items-end">
            <div className="col-6 col-md-2">
              <label className="form-label small fw-bold text-secondary">From</label>
              <input type="date" className="form-control form-control-sm bg-light border-0" />
            </div>
            <div className="col-6 col-md-2">
              <label className="form-label small fw-bold text-secondary">To</label>
              <input type="date" className="form-control form-control-sm bg-light border-0" />
            </div>
            <div className="col-12 col-md-2">
              <label className="form-label small fw-bold text-secondary">Status</label>
              <select className="form-select form-select-sm bg-light border-0">
                <option>All Records</option>
                <option>Pending</option>
                <option>Completed</option>
              </select>
            </div>
            <div className="col-12 col-md-4">
              <label className="form-label small fw-bold text-secondary">Search</label>
              <div className="input-group input-group-sm">
                <span className="input-group-text bg-light border-0"><i className="bi bi-search"></i></span>
                <input 
                  type="text" 
                  className="form-control bg-light border-0" 
                  placeholder="Search..." 
                  onChange={(e) => setSearchQuery(e.target.value)}
                />
              </div>
            </div>
            <div className="col-12 col-md-2">
              <button className="btn btn-primary btn-sm w-100 fw-bold shadow-sm">
                <i className="bi bi-funnel me-2"></i>Filter
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* 3. Data View */}
      
      {/* DESKTOP TABLE: Hidden on mobile */}
      <div className="d-none d-md-block card border-0 shadow-sm rounded-4 overflow-hidden">
        <table className="table table-hover align-middle mb-0">
          <thead className="bg-light">
            <tr className="small text-secondary">
              <th className="ps-4">S.NO</th>
              <th>DATE</th>
              <th>ITEM DETAILS</th>
              <th>CUSTOMER</th>
              <th className="text-center">QTY</th>
              <th>STATUS</th>
              <th className="pe-4 text-end">AMOUNT</th>
            </tr>
          </thead>
          <tbody>
            {currentRecords.map((item) => (
              <tr key={item.id}>
                <td className="ps-4 text-muted">{item.id}</td>
                <td className="text-nowrap small">{item.date}</td>
                <td>
                  <div className="fw-bold small">{item.item}</div>
                  <div className="text-muted" style={{fontSize: '11px'}}>{item.ref}</div>
                </td>
                <td className="small">{item.customer}</td>
                <td className="text-center fw-bold">{item.qty}</td>
                <td>
                  <span className={`badge rounded-pill ${item.status === 'Completed' ? 'bg-success-subtle text-success' : 'bg-warning-subtle text-warning'}`}>
                    {item.status}
                  </span>
                </td>
                <td className="pe-4 text-end fw-bold text-primary">${item.amount}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* MOBILE LIST: Visible only on mobile */}
      <div className="d-md-none">
        {currentRecords.map((item) => (
          <div key={item.id} className="card border-0 shadow-sm mb-3 rounded-4">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-start mb-2">
                <span className="badge bg-light text-dark border">#{item.id}</span>
                <span className={`badge rounded-pill ${item.status === 'Completed' ? 'bg-success-subtle text-success' : 'bg-warning-subtle text-warning'}`}>
                    {item.status}
                </span>
              </div>
              <h6 className="fw-bold mb-1">{item.item}</h6>
              <p className="text-muted small mb-2">{item.ref} | {item.date}</p>
              
              <div className="d-flex justify-content-between align-items-center bg-light p-2 rounded-3">
                <div className="small">
                    <div className="text-secondary" style={{fontSize: '10px'}}>CUSTOMER</div>
                    <div className="fw-medium">{item.customer}</div>
                </div>
                <div className="text-end">
                    <div className="text-secondary" style={{fontSize: '10px'}}>AMOUNT</div>
                    <div className="fw-bold text-primary">${item.amount}</div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* 4. Footer Pagination: Responsive alignment */}
      <div className="d-flex flex-column flex-md-row justify-content-between align-items-center mt-4 gap-3">
        <span className="text-muted small">
            Showing <b>{indexOfFirstRecord + 1}</b> to <b>{Math.min(indexOfLastRecord, totalRecords)}</b> of {totalRecords}
        </span>
        <nav>
          <ul className="pagination pagination-sm mb-0 shadow-sm">
            <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
              <button className="page-link" onClick={() => handlePageChange(currentPage - 1)}>Prev</button>
            </li>
            <li className="page-item active">
              <span className="page-link">{currentPage} / {totalPages || 1}</span>
            </li>
            <li className={`page-item ${currentPage === totalPages ? 'disabled' : ''}`}>
              <button className="page-link" onClick={() => handlePageChange(currentPage + 1)}>Next</button>
            </li>
          </ul>
        </nav>
      </div>
    </div>
  );
}