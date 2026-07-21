<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Key.aspx.cs" Inherits="BKSoftwares.Key" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>License Key Generator | BK Softwares</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />

    <style>
        :root {
            --primary: #6366f1; --accent: #4f46e5;
            --bg: #f8fafc; --dark: #0f172a; --text-light: #64748b;
        }
        body { background-color: var(--bg); font-family: 'Inter', sans-serif; color: var(--dark); padding: 20px; }

        /* Premium Header Style */
        .page-header { margin-bottom: 2rem; }
        
        /* Main Key Card */
        .main-container { 
            background: white; border-radius: 16px; border: 1px solid #e2e8f0; 
            box-shadow: 0 10px 15px -3px rgba(0,0,0,0.05); 
            max-width: 600px; margin: 0 auto; overflow: hidden;
        }

        .card-body-custom { padding: 30px; }

        /* Form Styling */
        .form-label { font-size: 0.75rem; font-weight: 700; text-transform: uppercase; color: var(--text-light); letter-spacing: 0.05em; margin-bottom: 8px; }
        .form-control-lg { border-radius: 12px; border: 1px solid #e2e8f0; padding: 14px; font-size: 1.1rem; transition: 0.3s; }
        .form-control-lg:focus { box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.1); border-color: var(--primary); }

        /* Button Styling */
        .btn-generate { 
            background: var(--primary); color: white; border-radius: 12px; 
            padding: 14px; font-weight: 700; border: none; transition: 0.3s; width: 100%;
            margin-top: 10px;
        }
        .btn-generate:hover { background: var(--accent); transform: translateY(-2px); box-shadow: 0 10px 15px rgba(99, 102, 241, 0.2); color: white; }

        /* Result Section */
        .result-box { 
            margin-top: 2rem; background: #f1f5f9; border-radius: 16px; 
            padding: 24px; border: 2px dashed #cbd5e1; text-align: center;
            animation: slideUp 0.4s ease-out;
        }
        @keyframes slideUp { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }

        .key-display { font-size: 2rem; font-weight: 800; color: var(--primary); letter-spacing: 2px; line-height: 1.2; margin: 10px 0; }
        .copy-wrapper { 
            display: inline-flex; align-items: center; gap: 8px; 
            background: white; padding: 8px 16px; border-radius: 50px; 
            box-shadow: 0 4px 6px rgba(0,0,0,0.05); cursor: pointer; transition: 0.2s;
        }
        .copy-wrapper:hover { transform: scale(1.05); background: var(--dark); color: white; }
        .copy-wrapper:hover i { color: #fff; }

        /* Error Label */
        .error-label { font-size: 0.85rem; font-weight: 600; color: #f43f5e; margin-top: 15px; text-align: center; display: block; }

        @media (max-width: 768px) {
            body { padding: 15px; }
            .key-display { font-size: 1.5rem; }
            .card-body-custom { padding: 20px; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            
            <!-- Header matching Premium Modules -->
            <div class="page-header text-center">
                <h2 class="fw-bold m-0 text-dark">License Key Generator</h2>
                <%--<p class="text-muted m-0 small">Secure activation utility for system installations</p>--%>
            </div>

            <!-- Central Generator Card -->
            <div class="main-container">
                <div class="card-body-custom">
                    
                    <div class="mb-4">
                        <label class="form-label">Client Hardware Key</label>
                        <asp:TextBox ID="txtHardwareKey" runat="server" CssClass="form-control form-control-lg text-center fw-bold" placeholder="00000 00000 00000" autocomplete="off"></asp:TextBox>
                        <div class="small text-muted mt-2">Paste the 3-part code displayed on the client's screen.</div>
                    </div>

                    <div class="mb-4">
                        <label class="form-label">Activation Date</label>
                        <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-control form-control-lg"></asp:TextBox>
                        <div class="small text-muted mt-2">The key will only work if the client PC is set to this date.</div>
                    </div>

                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Activation Code" CssClass="btn-generate shadow-sm" OnClick="btnGenerate_Click" />

                    <asp:Label ID="lblError" runat="server" CssClass="error-label"></asp:Label>

                    <!-- Result Section (Shows only after generation) -->
                    <asp:Panel ID="pnlResult" runat="server" Visible="false" CssClass="result-box">
                        <div class="small fw-bold text-uppercase text-muted mb-2">Activation Code Generated</div>
                        <div class="key-display" id="keyArea">
                            <asp:Label ID="lblActivationKey" runat="server"></asp:Label>
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 small fw-bold text-success"></asp:Label>
                        
                        <div class="copy-wrapper shadow-sm" onclick="copyKey()">
                            <i class="fas fa-copy text-primary me-1"></i>
                            <span class="small fw-bold">Copy to Clipboard</span>
                        </div>
                    </asp:Panel>

                </div>
            </div>
            
            <%--<p class="text-center text-muted small mt-4">BK Softwares &copy; <%= DateTime.Now.Year %> | Enterprise Security</p>--%>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        // Set default date logic
        document.addEventListener("DOMContentLoaded", function () {
            var dateInput = document.getElementById('<%= txtDate.ClientID %>');
            if (dateInput && !dateInput.value) {
                dateInput.valueAsDate = new Date();
            }
        });

        // Copy logic with visual feedback
        function copyKey() {
            var keyText = document.getElementById('<%= lblActivationKey.ClientID %>').innerText;
            navigator.clipboard.writeText(keyText).then(function () {
                // Change UI for feedback
                const wrapper = document.querySelector('.copy-wrapper');
                const originalContent = wrapper.innerHTML;

                wrapper.innerHTML = '<i class="fas fa-check text-success"></i> <span class="small fw-bold text-success">Copied!</span>';
                wrapper.style.borderColor = "#10b981";

                setTimeout(() => {
                    wrapper.innerHTML = originalContent;
                    wrapper.style.borderColor = "";
                }, 2000);
            }).catch(err => {
                alert("Please copy manually: " + keyText);
            });
        }
    </script>
</body>
</html>