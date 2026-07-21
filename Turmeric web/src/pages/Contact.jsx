import React from 'react';
import './Contact.css';

const Contact = () => {
  return (
    <div className="page-container fade-in">
      {/* Header */}
      <div className="text-center contact-header">
        <span className="pill">Global Trade Desk</span>
        <h1 className="title">Start Your <span className="text-gold">Bulk Order</span></h1>
        <p className="subtitle">Connect with our export division for CIF/FOB pricing and documentation.</p>
      </div>

      <div className="contact-grid-refined">
        {/* Inquiry Form */}
        <div className="glass-card form-container-premium">
          <form className="trade-form-refined" onSubmit={(e) => e.preventDefault()}>
            <div className="input-row">
                <input type="text" placeholder="Full Name / Company" className="premium-input" required />
                <input type="email" placeholder="Business Email" className="premium-input" required />
            </div>
            
            <div className="input-row">
                <select className="premium-input" defaultValue="">
                    <option value="" disabled>Select Variety</option>
                    <option value="lakadong">Lakadong (9% Curcumin)</option>
                    <option value="salem">Salem Polished</option>
                    <option value="erode">Erode Grade</option>
                    <option value="powder">Organic Powder</option>
                </select>
                <input type="text" placeholder="Estimated Quantity (MT)" className="premium-input" />
            </div>
            
            <textarea placeholder="Port of Destination & Special Requirements (e.g. OEM Packaging, Lab Reports)" rows="5" className="premium-input"></textarea>
            
            <button type="submit" className="btn-solid glow-effect">
                Submit Export Inquiry
            </button>
          </form>
        </div>

        {/* Office Details Stack */}
        <div className="office-stack">
          <div className="glass-card mini-info">
             <div className="info-icon">📍</div>
             <div>
                <h4>India Office (HQ)</h4>
                <p>Erode, <br/>Tamil Nadu 638001</p>
             </div>
          </div>

          <div className="glass-card mini-info">
             <div className="info-icon">📞</div>
             <div>
                <h4>Direct Support</h4>
                <p>+91 90928 51477 <br/>export@goldencurcumin.com</p>
             </div>
          </div>
          
          <div className="bk-software-contact">
             <p>Technical Partner: <span>BK Software @ 2026</span></p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Contact;