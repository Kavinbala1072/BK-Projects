import React from 'react';
import './About.css';

const About = () => {
  return (
    <div className="page-container fade-in">
      {/* Header Area */}
      <div className="text-center about-header">
        <span className="pill">Our Story</span>
        <h1 className="title">Born from the Fields of <span className="text-gold">Erode</span></h1>
      </div>

      {/* Main Story & Stats Section */}
      <div className="glass-card about-main-card">
        <div className="about-grid">
          <div className="about-text">
            <h3>Purity in every grain.</h3>
            <p>Golden Turmeric was started with one clear belief — India grows the world's best turmeric, and Erode grows the best in India. But too often, the quality that leaves the farm never reaches the final buyer. We set out to fix that.</p>
            <p>Our founders grew up watching turmeric farmers in Erode work hard through every season — planting, nurturing, and harvesting — only to be at the mercy of multiple middlemen who diluted both quality and profit. Golden Turmeric was built to close that gap.</p>
            <p>"We don't just sell turmeric. We carry the trust of every farmer who grew it and every buyer who depends on its purity."</p>
            <p>As a startup, we move fast but never compromise. We personally visit farms, test every batch, and build long-term relationships with both our grower network and our B2B buyers across India. Global export is our next horizon — and we're building the infrastructure to reach it responsibly.</p>
          </div>
          
          <div className="about-stats">
            <div className="stat-pill">
              <span>10+</span> 
              <p>Years of Excellence</p>
            </div>
            <div className="stat-pill">
              <span>500+</span> 
              <p>Farming Partners</p>
            </div>
            <div className="stat-pill">
              <span>100%</span> 
              <p>Traceable Source</p>
            </div>
          </div>
        </div>
      </div>

      {/* Vision & Mission Cards - Added for more "Interest" */}
      <div className="vision-mission-grid">
        <div className="glass-card mission-box">
          <div className="icon-wrap">🎯</div>
          <h4>Our Mission</h4>
          <p>To empower local Indian farmers while providing global industries with pharmaceutical-grade turmeric processed under zero-contamination standards.</p>
        </div>
        <div className="glass-card mission-box">
          <div className="icon-wrap">👁️</div>
          <h4>Our Vision</h4>
          <p>To become the world's most trusted digital-first spice exporter, setting the benchmark for transparency and high-curcumin quality.</p>
        </div>
      </div>
    </div>
  );
};

export default About;