import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';

const Navbar = ({ theme, toggleTheme }) => {
  const [isOpen, setIsOpen] = useState(false);

  // Close menu and scroll to top smoothly
  const handleLinkClick = () => {
    setIsOpen(false);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  return (
    <nav className="glass-nav">
      <div className="nav-container">
        <Link to="/" className="nav-logo" onClick={handleLinkClick}>
          GOLDEN<span>TURMERIC</span>
        </Link>

        {/* Navigation Links */}
        <div className={`nav-menu ${isOpen ? 'active' : ''}`}>
          <Link to="/" onClick={handleLinkClick}>Home</Link>
          <Link to="/about" onClick={handleLinkClick}>About</Link>
          <Link to="/product" onClick={handleLinkClick}>Products</Link>
          <Link to="/service" onClick={handleLinkClick}>Services</Link>
          
          {/* Mobile Only: Theme Toggle */}
          <div className="mobile-only-controls">
             <button className="theme-toggle" onClick={toggleTheme}>
                {theme === 'light' ? '🌙 Dark Mode' : '☀️ Light Mode'}
             </button>
          </div>
          
          <Link to="/contact" className="nav-cta" onClick={handleLinkClick}>Inquiry</Link>
        </div>

        {/* Desktop Theme Toggle Icon */}
        <button className="theme-toggle desktop-only" onClick={toggleTheme} title="Toggle Theme">
          {theme === 'light' ? '🌙' : '☀️'}
        </button>

        {/* Hamburger Icon */}
        <div className={`hamburger ${isOpen ? 'active' : ''}`} onClick={() => setIsOpen(!isOpen)}>
          <span className="line top"></span>
          <span className="line mid"></span>
          <span className="line bot"></span>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;