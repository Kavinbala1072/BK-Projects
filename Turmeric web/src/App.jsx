import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import Navbar from './components/Navbar';
import Footer from './components/Footer';

import Home from './pages/Home';
import About from './pages/About';
import Product from './pages/Product';
import Service from './pages/Service';
import Contact from './pages/Contact';

import './App.css';

function App() {
  const [theme, setTheme] = useState(localStorage.getItem('theme') || 'dark');

  useEffect(() => {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
  }, [theme]);

  const toggleTheme = () => {
    setTheme((prevTheme) => (prevTheme === 'light' ? 'dark' : 'light'));
  };

  return (
    <Router>
      <div className="app-wrapper">
        <div className="bg-blobs">
          <div className="blob b1"></div>
          <div className="blob b2"></div>
          <div className="blob b3"></div>
        </div>
        <Navbar theme={theme} toggleTheme={toggleTheme} />

        <main className="main-viewport">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/about" element={<About theme={theme} />} />
            <Route path="/product" element={<Product />} />
            <Route path="/service" element={<Service />} />
            <Route path="/contact" element={<Contact />} />
          </Routes>
        </main>
        <Footer />
        <a 
          href="https://wa.me/919876543210?text=Hi, I am interested in Turmeric Bulk Export Inquiry." 
          className="whatsapp-btn-container" 
          target="_blank" 
          rel="noreferrer"
          title="Chat with Trade Desk"
        >
          <div className="whatsapp-tooltip">Chat with Trade Desk</div>
          <div className="whatsapp-pulse"></div>
          <div className="whatsapp-icon-box">
            <img 
              src="https://upload.wikimedia.org/wikipedia/commons/6/6b/WhatsApp.svg" 
              alt="WhatsApp" 
            />
          </div>
        </a>

      </div>
    </Router>
  );
}

export default App;