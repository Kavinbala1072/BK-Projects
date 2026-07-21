import React, { useState, useEffect } from 'react';
import { featuredProducts } from '../data/products';
import './Product.css';

const Product = () => {
  const [currency, setCurrency] = useState('INR');
  const [rates, setRates] = useState({ INR: 1, USD: 0.012, EUR: 0.011, AED: 0.044 }); // Default Fallback
  const [isLoading, setIsLoading] = useState(true);

  // --- FETCH REAL-TIME RATES ---
  useEffect(() => {
    const fetchRates = async () => {
      try {
        // We fetch rates based on 1 INR
        const response = await fetch('https://open.er-api.com/v6/latest/INR');
        const data = await response.json();
        
        if (data && data.rates) {
          setRates({
            INR: 1,
            USD: data.rates.USD,
            EUR: data.rates.EUR,
            AED: data.rates.AED,
          });
        }
        setIsLoading(false);
      } catch (error) {
        console.error("Currency API failed, using fallback rates.", error);
        setIsLoading(false);
      }
    };

    fetchRates();
  }, []);

  const symbols = {
    INR: '₹',
    USD: '$',
    EUR: '€',
    AED: 'د.إ'
  };

  const formatPrice = (price) => {
    if (isLoading) return "...";
    const converted = (price * rates[currency]).toFixed(2);
    return `${symbols[currency]} ${converted}`;
  };

  return (
    <div className="page-container fade-in">
      <div className="catalog-header-wrap">
        <div className="text-left">
          <span className="pill">Global Export Catalog</span>
          <h1 className="title">Premium <span className="text-gold">Varieties</span></h1>
        </div>
        
        {/* Currency Switcher */}
        <div className="currency-selector">
            <span>Market Price in: </span>
            <select value={currency} onChange={(e) => setCurrency(e.target.value)}>
                <option value="INR">India (INR)</option>
                <option value="USD">USA (USD)</option>
                <option value="EUR">Europe (EUR)</option>
                <option value="AED">Dubai (AED)</option>
            </select>
            {isLoading && <div className="loader-mini"></div>}
        </div>
      </div>

      <div className="product-grid-refined">
        {featuredProducts.map((p) => (
          <div key={p.id} className="glass-card b2b-product-card">
            <div className="card-visual">
              <img src={p.img} alt={p.title} className="main-prod-img" />
              <div className="cur-floating-tag">{p.cur} Curcumin</div>
            </div>
            
            <div className="card-body-refined">
              <div className="card-top-row">
                <h3>{p.title}</h3>
                <div className="price-tag">
                    {formatPrice(p.basePrice)} 
                    <small>{currency === 'INR' ? '/ kg' : ' (Est. FOB)'}</small>
                </div>
              </div>
              
              <p className="description-text">{p.desc}</p>
              
              <div className="specs-table">
                <div className="spec-row"><span>Origin</span> <strong>{p.specs.origin}</strong></div>
                <div className="spec-row"><span>Packaging</span> <strong>{p.specs.packaging}</strong></div>
              </div>

              <div className="card-action-btns">
                <button className="btn-main-gold small-padding">Get Technical Data</button>
                <button className="btn-icon-blur">✉️</button>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div className="bulk-order-notice">
         <p>
           {isLoading 
            ? "🔄 Syncing with global markets..." 
            : `✅ Live Rates applied. 1 INR = ${rates[currency]} ${currency}`}
         </p>
         <p className="sub-note">Prices are for bulk orders above 500kg. Contact for LCL/FCL shipping quotes.</p>
      </div>
    </div>
  );
};

export default Product;