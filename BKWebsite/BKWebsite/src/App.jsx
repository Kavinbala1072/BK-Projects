import { useState, useEffect, useRef } from 'react'
import './App.css'
import reviewsData from './data/reviews.json' 

function App() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  
  // --- AI CHATBOT LOGIC ---
  const [isChatOpen, setIsChatOpen] = useState(false);
  const [chatStep, setChatStep] = useState(0);
  const [userResponse, setUserResponse] = useState("");
  const [messages, setMessages] = useState([
    { sender: 'bot', text: 'Hello! Welcome to BK Softwares. I am your AI assistant. What is your name?' }
  ]);
  const [leadData, setLeadData] = useState({});
  const chatEndRef = useRef(null);

  useEffect(() => {
    chatEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const handleChatSubmit = (e) => {
    e.preventDefault();
    if (!userResponse.trim()) return;

    const newMessages = [...messages, { sender: 'user', text: userResponse }];
    setMessages(newMessages);
    
    setTimeout(() => {
      let botText = "";
      if (chatStep === 0) {
        setLeadData({ ...leadData, name: userResponse });
        botText = `Nice to meet you, ${userResponse}! Which solution do you need? (ERP, Website, or Billing?)`;
        setChatStep(1);
      } else if (chatStep === 1) {
        setLeadData({ ...leadData, service: userResponse });
        botText = `Great! Please briefly describe your business requirements.`;
        setChatStep(2);
      } else if (chatStep === 2) {
        setLeadData({ ...leadData, goals: userResponse });
        botText = `Perfect. Please provide your Phone number so our technical team can reach out.`;
        setChatStep(3);
      } else {
        botText = `Thank you! I have recorded your requirements. Our experts will contact you shortly.`;
        setChatStep(4);
      }
      setMessages([...newMessages, { sender: 'bot', text: botText }]);
    }, 800);
    setUserResponse("");
  };

  return (
    <div className="site-wrapper">
      {/* FLOATING WHATSAPP (Bottom Left) */}
      <a href="https://wa.me/919597720998" className="whatsapp-float" target="_blank" rel="noreferrer">
        <img src="https://upload.wikimedia.org/wikipedia/commons/6/6b/WhatsApp.svg" alt="WhatsApp" />
        <span className="wa-label">Inquiry Now</span>
      </a>

      {/* AI CHATBOT (Bottom Right) */}
      <div className={`ai-chat-wrapper ${isChatOpen ? 'open' : ''}`}>
        {!isChatOpen ? (
          <button className="ai-trigger" onClick={() => setIsChatOpen(true)}>
            <div className="ai-icon">🤖</div>
            <span>Talk to AI</span>
          </button>
        ) : (
          <div className="ai-window">
            <div className="ai-header">
              <div className="ai-brand">BK AI ASSISTANT</div>
              <button className="ai-close" onClick={() => setIsChatOpen(false)}>✕</button>
            </div>
            <div className="ai-body">
              {messages.map((m, i) => (
                <div key={i} className={`msg-bubble ${m.sender}`}>{m.text}</div>
              ))}
              <div ref={chatEndRef} />
            </div>
            {chatStep <= 3 && (
              <form className="ai-footer" onSubmit={handleChatSubmit}>
                <input type="text" placeholder="Type here..." value={userResponse} onChange={(e) => setUserResponse(e.target.value)} />
                <button type="submit">➤</button>
              </form>
            )}
          </div>
        )}
      </div>

      {/* NAVBAR */}
      <nav className="navbar">
        <div className="container nav-container">
          <div className="logo">BK<span>SOFTWARES</span></div>
          <ul className={`nav-links ${isMenuOpen ? 'active' : ''}`}>
            <li><a href="#home" onClick={() => setIsMenuOpen(false)}>Home</a></li>
            <li><a href="#about" onClick={() => setIsMenuOpen(false)}>Our Story</a></li>
            <li><a href="#products" onClick={() => setIsMenuOpen(false)}>Solutions</a></li>
            <li><a href="#reviews" onClick={() => setIsMenuOpen(false)}>Reviews</a></li>
            <li><a href="#contact" className="nav-cta" onClick={() => setIsMenuOpen(false)}>Start Project</a></li>
          </ul>
          <div className="mobile-toggle" onClick={() => setIsMenuOpen(!isMenuOpen)}>
            <div className="bar"></div><div className="bar"></div><div className="bar"></div>
          </div>
        </div>
      </nav>

      {/* HERO SECTION - 100vh */}
      <section id="home" className="hero-fullscreen">
        <div className="container hero-grid">
          <div className="hero-text">
            <span className="badge">Established 2025 • IT Professional Led</span>
            <h1>Modern Software <br/><span className="blue-text">For Local Excellence.</span></h1>
            <p>Industrial-grade ERPs and high-performance web applications built with corporate standards and startup agility.</p>
            <div className="hero-btns">
              <a href="#contact" className="btn-primary">Get Started</a>
              <a href="#products" className="btn-secondary">Explore Products</a>
            </div>
          </div>
          <div className="hero-image-wrap">
            <img src="https://images.unsplash.com/photo-1551434678-e076c223a692?auto=format&fit=crop&w=1000" alt="Software" />
            <div className="experience-box-floating"><strong>1+ Year</strong><span>Startup Agility</span></div>
          </div>
        </div>
      </section>

      {/* ABOUT SECTION - 100vh */}
      <section id="about" className="about-fullscreen">
        <div className="container about-grid">
          <div className="about-image-side">
            <img src="https://images.unsplash.com/photo-1498050108023-c5249f4df085?auto=format&fit=crop&w=800" alt="Coder" />
          </div>
          <div className="about-content-side">
            <span className="section-label">OUR STORY</span>
            <h2>Expert Code. <br/>Personal Dedication.</h2>
            <p className="lead-text">BK Softwares is founded by an <strong>Active IT Professional</strong>. We apply global corporate standards to every business solution we build.</p>
            <p>We bridge the gap between complex engineering and user-friendly software. Every project follows international standards to ensure 100% data accuracy.</p>
            <div className="about-features">
              <div className="feat"><h4>✓ Corporate Logic</h4><p>Industry-standard architecture.</p></div>
              <div className="feat"><h4>✓ Startup Support</h4><p>Fast delivery & 24/7 care.</p></div>
            </div>
          </div>
        </div>
      </section>

      {/* PRODUCTS SECTION */}
      <section id="products" className="products-section">
        <div className="container">
          <div className="section-header">
            <span className="section-label">SOLUTIONS</span>
            <h2>Our Core Product Suites</h2>
          </div>
          <div className="products-grid">
            <div className="product-card">
              <div className="p-img"><img src="https://images.unsplash.com/photo-1586528116311-ad8dd3c8310d?auto=format&fit=crop&w=500" alt="ERP" /></div>
              <div className="p-body">
                <span className="p-tag">VB.NET ERP</span>
                <h3>Godown & Inventory</h3>
                <p>Advanced stock maintenance and Atma Donation management tools built for robust performance.</p>
              </div>
            </div>
            <div className="product-card featured-card">
              <div className="p-img"><img src="https://images.unsplash.com/photo-1460925895917-afdab827c52f?auto=format&fit=crop&w=500" alt="Billing" /></div>
              <div className="p-body">
                <span className="p-tag">ASP.NET C#</span>
                <h3>BK Billing Software</h3>
                <p>Cloud-based Sales, Purchase, and Inventory with automated GST Reports accessible anywhere.</p>
              </div>
            </div>
            <div className="product-card">
              <div className="p-img"><img src="https://images.unsplash.com/photo-1555066931-4365d14bab8c?auto=format&fit=crop&w=500" alt="Web" /></div>
              <div className="p-body">
                <span className="p-tag">React JS</span>
                <h3>Custom Web Design</h3>
                <p>High-performance, responsive websites built with the latest React 18+ standards.</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* REVIEWS SECTION */}
      <section id="reviews" className="reviews-section">
        <div className="container">
          <div className="section-header">
            <span className="section-label">FEEDBACK</span>
            <h2>What Our Clients Say</h2>
          </div>
          <div className="reviews-container">
            {reviewsData.map((review) => (
              <div key={review.id} className="review-bubble">
                <div className="stars">{"★".repeat(review.rating)}</div>
                <p>"{review.comment}"</p>
                <div className="reviewer-meta">
                  <strong>{review.name}</strong>
                  <span>{review.position}</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CONTACT SECTION */}
      <section id="contact" className="contact-section">
        <div className="container">
          <div className="contact-card">
            <div className="contact-text">
              <h2>Let's build <br/>your vision.</h2>
              <p>BK Softwares | Erode, Tamil Nadu</p>
              <div className="contact-info-footer">
                <p>📧 kavinbala82@gmail.com</p>
                <p>📞 +91 95977 20998</p>
              </div>
            </div>
            <form className="contact-form" onSubmit={(e) => e.preventDefault()}>
              <div className="input-group">
                <input type="text" placeholder="Full Name" required />
                <input type="email" placeholder="Email Address" required />
              </div>
              <select required>
                <option value="">Select Interest</option>
                <option value="erp">ERP Solution (VB.NET)</option>
                <option value="billing">Cloud Billing (ASP.NET)</option>
                <option value="web">Website Design (React)</option>
              </select>
              <textarea placeholder="Describe your project goals..." rows="4" required></textarea>
              <button type="submit" className="btn-primary-full">Submit Inquiry</button>
            </form>
          </div>
        </div>
      </section>

      {/* FOOTER */}
      <footer className="footer">
        <div className="container footer-grid">
          <div className="footer-col">
            <div className="footer-logo">BK<span>SOFTWARES</span></div>
            <p className="footer-desc">Engineering industrial-grade ERPs with 100% accuracy. Founded by IT professionals for reliable growth.</p>
          </div>
          <div className="footer-col">
            <h4>Quick Links</h4>
            <ul><li><a href="#home">Home</a></li><li><a href="#about">Our Story</a></li><li><a href="#products">Solutions</a></li></ul>
          </div>
          <div className="footer-col">
            <h4>Solutions</h4>
            <ul><li>Godown ERP</li><li>BK Billing</li><li>Custom Web</li></ul>
          </div>
          <div className="footer-col">
            <h4>Contact</h4>
            <p>📍 Erode, Tamil Nadu</p>
            <p>✉️ kavinbala82@gmail.com</p>
          </div>
        </div>
        <div className="footer-bottom">
          <div className="container bottom-flex">
            <p>© 2025 BK Softwares • Built with Accuracy & Integrity</p>
          </div>
        </div>
      </footer>
    </div>
  )
}

export default App