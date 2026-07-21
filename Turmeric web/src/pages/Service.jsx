import React from 'react';
import './Service.css';

const Service = () => {
  const services = [
    { 
      title: "Steam Sterilization", 
      desc: "Industry-leading ETO-free pathogen control. We maintain 100% natural essential oils and aroma during the process.",
      icon: "🔥" 
    },
    { 
      title: "Private Labeling", 
      desc: "Custom OEM packaging solutions. From retail-ready 50g pouches to 50kg bulk industrial bags with your branding.",
      icon: "🏷️" 
    },
    { 
      title: "Curcumin Standardizing", 
      desc: "Precise blending and extraction services to meet specific curcumin requirements for pharmaceutical clients.",
      icon: "🧪" 
    },
    { 
      title: "Global Logistics", 
      desc: "End-to-end supply chain management. Seamless door-to-port delivery with full international customs compliance.",
      icon: "🌍" 
    },
    { 
      title: "Digital Traceability", 
      desc: "Powered by BK Software @ 2026, we offer QR-based batch tracking from the farm level to the final destination.",
      icon: "📱" 
    },
    { 
      title: "Quality Lab Testing", 
      desc: "Third-party lab reports (SGS/Eurofins) for every batch, covering lead content, pesticides, and microbial counts.",
      icon: "🛡️" 
    }
  ];

  return (
    <div className="page-container fade-in">
      {/* Header Area */}
      <div className="text-center service-header">
        <span className="pill">Technical Excellence</span>
        <h1 className="title">Our Export <span className="text-gold">Expertise</span></h1>
        <p className="subtitle">High-end processing and logistics tailored for global turmeric demand.</p>
      </div>

      {/* Services Grid */}
      <div className="services-grid-layout">
        {services.map((s, i) => (
          <div key={i} className="glass-card service-card-premium">
            <div className="service-icon-box">{s.icon}</div>
            <div className="service-text">
                <h3>{s.title}</h3>
                <p>{s.desc}</p>
            </div>
            <div className="service-hover-line"></div>
          </div>
        ))}
      </div>

      {/* Bottom CTA for Services */}
      <div className="service-footer-note">
          <div className="glass-card tech-partnership-card">
              <p>Looking for a technical trade partner? <strong>BK Software</strong> & <strong>Golden Turmeric</strong> provide integrated digital trade solutions.</p>
              <button className="btn-small-gold">Download Brochure</button>
          </div>
      </div>
    </div>
  );
};

export default Service;