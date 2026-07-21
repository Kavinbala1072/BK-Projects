import React from 'react';
import { Link } from 'react-router-dom';
import { featuredProducts } from '../data/products';
import './Home.css';

const Home = () => {
  const whyChooseUs = [
    { title: "Premium Erode Turmeric", desc: "Sourced from the world's turmeric capital with superior curcumin levels.", icon: "🎖️" },
    { title: "Quality Assurance", desc: "Strict quality checks to ensure 100% purity and consistency in every batch.", icon: "✅" },
    { title: "Direct Farm Sourcing", desc: "Working directly with farmers to ensure fair pricing and farm-fresh quality.", icon: "🚜" },
    { title: "Export Packaging", desc: "Food-grade materials designed to preserve aroma and freshness during transit.", icon: "📦" },
    { title: "Bulk Supply", desc: "Fulfilling small, medium, and large bulk orders for global industrial needs.", icon: "🏗️" },
    { title: "Timely Delivery", desc: "Efficient logistics network ensuring your gold reaches you on schedule.", icon: "🚚" },
    { title: "Competitive Pricing", desc: "Premium export quality offered at the most competitive market rates.", icon: "💰" },
    { title: "Customer Satisfaction", desc: "Building long-term global relationships through trust and transparency.", icon: "🤝" },
  ];

  const processSteps = [
    { step: "01", title: "Ethical Harvest", desc: "Hand-picked by Erode farmers at peak maturity." },
    { step: "02", title: "Scientific Processing", desc: "ETO-free steam sterilization & Sortex cleaning." },
    { step: "03", title: "Traceable Packaging", desc: "QR-coded batches for 100% digital transparency." },
    { step: "04", title: "Global Export", desc: "Fast-track logistics to 40+ international ports." }
  ];

  return (
    <div className="home-wrapper fade-in">
      
      {/* --- 1. HERO SECTION --- */}
      <section className="home-hero">
        <div className="hero-inner">
          <div className="hero-text-area">
            <span className="pill-premium">Welcome to Golden Turmeric</span>
            <h1 className="hero-title">
              Exporters of <span className="gold-gradient">Gold Standard</span> <br/>
              Indian Turmeric.
            </h1>
            <p className="tagline-text">From the Heart of Erode to the World. Premium Quality You Can Trust.</p>
            <p className="hero-subtitle">
              We proudly bring the finest turmeric from the fertile lands of Erode, Tamil Nadu. 
              We specialize in sourcing, processing, and exporting products that meet 
              stringent international standards.
            </p>
            <div className="hero-button-group">
              <Link to="/product" className="btn-main-gold">Explore Products</Link>
              <Link to="/contact" className="btn-blur-glass">Request Bulk Quote</Link>
            </div>
          </div>
          
          <div className="hero-visual-area">
            <div className="image-blob-decoration"></div>
            <img 
              src="https://static.vecteezy.com/system/resources/previews/053/409/558/non_2x/turmeric-with-slices-on-transparent-background-free-png.png" 
              // src="https://www.pngplay.com/wp-content/uploads/9/Turmeric-Transparent-Image.png"
              alt="Premium Turmeric" 
              className="hero-floating-img"
            />
          </div>
        </div>
      </section>

      {/* --- 2. IMPACT STATS (NEW) --- */}
      <section className="impact-stats-bar">
         <div className="stat-item"><h3>28+</h3><p>Indian States</p></div>
         <div className="stat-item"><h3>40+</h3><p>Export Countries</p></div>
         <div className="stat-item"><h3>9.2%</h3><p>Max Curcumin</p></div>
         <div className="stat-item"><h3>100%</h3><p>Farm Fresh</p></div>
      </section>

      {/* --- 3. ABOUT US PREVIEW --- */}
      <section className="home-about-preview">
        <div className="glass-card about-preview-card">
          <div className="about-preview-content">
            <span className="pill-premium">Who We Are</span>
            <h2>Established for Purity</h2>
            <p>
              <strong>GOLDEN TURMERIC</strong> is a startup established with the vision of delivering the authentic richness of Erode turmeric to customers worldwide. 
            </p>
            <p>
              By combining modern processing techniques with traditional farming values, we provide products that retain their natural aroma, vibrant color, and nutritional value.
            </p>
            <Link to="/about" className="simple-gold-link">Read Our Full Story →</Link>
          </div>
          <div className="about-preview-image">
             <img src="https://navilnatural.com/wp-content/uploads/2019/01/turmeric-1.jpg" alt="Erode Farms" />
          </div>
        </div>
      </section>

      {/* --- 4. THE PROCESS JOURNEY (NEW) --- */}
      <section className="process-journey">
        <div className="text-center">
            <span className="pill-premium">Our Workflow</span>
            <h2 className="section-title">Seed to Shelf Journey</h2>
        </div>
        <div className="process-grid">
          {processSteps.map((s, i) => (
            <div key={i} className="process-node">
                <div className="step-number">{s.step}</div>
                <h4>{s.title}</h4>
                <p>{s.desc}</p>
                {i < 3 && <div className="process-arrow">→</div>}
            </div>
          ))}
        </div>
      </section>

      {/* --- 5. WHY CHOOSE US GRID --- */}
      <section className="why-choose-section">
        <div className="text-center">
            <span className="pill-premium">Our Value</span>
            <h2 className="section-title">Why GOLDEN TURMERIC?</h2>
        </div>
        <div className="why-grid">
          {whyChooseUs.map((item, index) => (
            <div key={index} className="glass-card why-card">
              <div className="why-icon">{item.icon}</div>
              <h3>{item.title}</h3>
              <p>{item.desc}</p>
            </div>
          ))}
        </div>
      </section>

    </div>
  );
};

export default Home;