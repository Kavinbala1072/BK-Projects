import React from 'react';
import { Link } from 'react-router-dom';
import './Footer.css';

const Footer = () => {
  const scrollToTop = () => {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  return (
    <footer className="footer-premium">
      {/* Organic Wave Divider to transition from page content */}
      <div className="footer-wave">
        <svg data-name="Layer 1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 120" preserveAspectRatio="none">
          <path d="M321.39,56.44c58-10.79,114.16-30.13,172-41.86,82.39-16.72,168.19-17.73,250.45-.39C823.78,31,906.67,72,985.66,92.83c70.05,18.48,146.53,26.09,214.34,3V0H0V27.35A600.21,600.21,0,0,0,321.39,56.44Z" className="shape-fill"></path>
        </svg>
      </div>

      <div className="footer-container">
        <div className="footer-main-grid">
          
          {/* Brand Identity & Vision */}
          <div className="footer-brand-section">
            <Link to="/" className="footer-logo" onClick={scrollToTop}>
              GOLDEN<span>TURMERIC</span>
            </Link>
            <p className="brand-statement">
              We proudly bring the finest turmeric from the fertile lands of Erode, Tamil Nadu. We specialize in sourcing, processing, and exporting products that meet stringent international standards.
            </p>
            <div className="export-badges">
              <div className="badge-item"><span>APEDA</span></div>
              <div className="badge-item"><span>FSSAI</span></div>
              <div className="badge-item"><span>ISO 22000</span></div>
              <div className="badge-item"><span>GMP</span></div>
            </div>
          </div>

          {/* Quick Navigation */}
          <div className="footer-nav-section">
            <h4 className="footer-title">Marketplace</h4>
            <ul className="footer-links-list">
              <li><Link to="/" onClick={scrollToTop}>Home</Link></li>
              <li><Link to="/about" onClick={scrollToTop}>Our Heritage</Link></li>
              <li><Link to="/product" onClick={scrollToTop}>Export Varieties</Link></li>
              <li><Link to="/service" onClick={scrollToTop}>Processing Services</Link></li>
              <li><Link to="/contact" onClick={scrollToTop}>Bulk Quote</Link></li>
            </ul>
          </div>

          {/* Export Reach & Contact */}
          <div className="footer-contact-section">
            <h4 className="footer-title">Global Reach</h4>
            <p className="reach-text">Supplying to all over India.</p>
            <div className="contact-info-stack">
              <a href="tel:+919876543210" className="contact-link">
                <i className="icon">📞</i> +91 98765 43210
              </a>
              <a href="mailto:export@goldencurcumin.com" className="contact-link">
                <i className="icon">✉️</i> export@goldencurcumin.com
              </a>
              <div className="location-tag">
                <i className="icon">📍</i> HQ: Erode, TamilNadu | India
              </div>
            </div>
          </div>

        </div>

        {/* Branding & Bottom Strip */}
        <div className="footer-bottom-bar">
          <div className="copyright-area">
            <p>© 2026 Golden Turmeric Exports. All Rights Reserved.</p>
          </div>
          <div className="dev-partnership">
            <div className="bk-software-tag">
              Technical Trade Partner <a href="https://bksoftware.netlify.app/" target="_blank" rel="noopener noreferrer" className="dev-highlight link-button">
              <span className="dev-highlight">BK Software @ 2026</span></a>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;