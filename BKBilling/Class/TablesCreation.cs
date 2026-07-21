using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BKBilling.Class
{
    public class TablesCreation
    {
        public static void CreateBusinessSchema(long cid)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LedgerGroup_Table')
                    BEGIN
                        CREATE TABLE LedgerGroup_Table (
                            LedgerGroup_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                            Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                            LedgerGroup_Name NVARCHAR(100), 
                            Ledgergroup_Under INT,
                            Nature NVARCHAR(50),
                            IsActive BIT DEFAULT 1, 
                            Created_Date DATETIME DEFAULT GETDATE()
                        );
                    END");

                string seedSql = @"IF (SELECT COUNT(*) FROM LedgerGroup_Table WHERE Company_No = @cid) = 0
                BEGIN
                    SET IDENTITY_INSERT LedgerGroup_Table ON;
                    INSERT INTO LedgerGroup_Table (LedgerGroup_Sno, Company_No, LedgerGroup_Name, Ledgergroup_Under, Nature, IsActive) VALUES 
                    (1000000000, @cid, 'PRIMARY', 0, '0', 1),
                    (1000000001, @cid, 'Branches / Divisions', 1000000000, '0', 1),
                    (1000000002, @cid, 'Current Liabilities', 1000000000, '0', 1),
                    (1000000003, @cid, 'Fixed Assets', 1000000000, '0', 1),
                    (1000000004, @cid, 'Investments', 1000000000, '0', 1),
                    (1000000005, @cid, 'Loans(Liability)', 1000000000, '0', 1),
                    (1000000006, @cid, 'Capital Accounts', 1000000000, '0', 1),
                    (1000000007, @cid, 'Preliminary Expenses(Asset)', 1000000000, '0', 1),
                    (1000000008, @cid, 'Profit And Loss Account', 1000000000, '0', 1),
                    (1000000009, @cid, 'Revenue Accounts', 1000000000, '0', 1),
                    (1000000010, @cid, 'Suspense Account', 1000000000, '0', 1),
                    (1000000011, @cid, 'Current Assets', 1000000000, '0', 1),
                    (1000000012, @cid, 'Duties And Taxes', 1000000002, '1', 1),
                    (1000000013, @cid, 'Provisions', 1000000002, '1', 1),
                    (1000000014, @cid, 'Sundry Creditors', 1000000002, '1', 1),
                    (1000000015, @cid, 'Bank OD Account', 1000000005, '1', 1),
                    (1000000016, @cid, 'Secured Loans', 1000000005, '1', 1),
                    (1000000017, @cid, 'Unsecured Loans', 1000000005, '1', 1),
                    (1000000018, @cid, 'Reserves & Surplus', 1000000006, '1', 1),
                    (1000000019, @cid, 'Expenditure Account', 1000000009, '1', 1),
                    (1000000020, @cid, 'Income', 1000000009, '1', 1),
                    (1000000021, @cid, 'Purchase Account', 1000000009, '1', 1),
                    (1000000022, @cid, 'Sales Account', 1000000009, '1', 1),
                    (1000000023, @cid, 'Sundry Debtors', 1000000011, '1', 1),
                    (1000000024, @cid, 'Bank Accounts', 1000000011, '1', 1),
                    (1000000025, @cid, 'Cash In Hand', 1000000011, '1', 1),
                    (1000000026, @cid, 'Stock In Hand', 1000000011, '1', 1),
                    (1000000027, @cid, 'Expenses(Mfg)', 1000000019, '2', 1),
                    (1000000028, @cid, 'Expenses(Admin)', 1000000019, '2', 1),
                    (1000000029, @cid, 'Customers', 1000000023, '1', 1),
                    (1000000030, @cid, 'Supplier', 1000000014, '1', 1),
                    (1000000031, @cid, 'Job Worker', 1000000014, '1', 1);
    
                    SET IDENTITY_INSERT LedgerGroup_Table OFF;
                END";
                using (SqlCommand cmd = new SqlCommand(seedSql, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", cid);
                    cmd.ExecuteNonQuery();
                }

                // 3. Other Tables Creation
                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Area_table')
                    BEGIN
                        CREATE TABLE Area_table (
                            Area_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                            Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                            User_No INT REFERENCES User_Table(User_Sno),
                            Area_Name NVARCHAR(100), Area_Under INT, IsActive BIT DEFAULT 1,
                            Created_Date DATETIME DEFAULT GETDATE()
                        );
                    END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Ledger_Table')
                BEGIN
                    CREATE TABLE Ledger_Table (Ledger_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                        Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                        User_No INT REFERENCES User_Table(User_Sno),
                        LedgerGroup_no INT REFERENCES LedgerGroup_Table(LedgerGroup_Sno),
                        Area_no INT REFERENCES Area_table(Area_Sno),
                        ledger_Active BIT DEFAULT 1, 
                        ledger_code NVARCHAR(50), 
                        ledger_name NVARCHAR(150),
                        ledger_Add1 NVARCHAR(200),
                        ledger_Add2 NVARCHAR(200),
                        ledger_Add3 NVARCHAR(200),
                        Ledger_Email NVARCHAR(100),
                        Ledger_Phone NVARCHAR(20),
                        Ledger_ContactPerson NVARCHAR(100),
                        ledger_bank NVARCHAR(100), 
                        Ledger_Branch NVARCHAR(100),
                        ledger_AcNo NVARCHAR(50),
                        ledger_Ifscode NVARCHAR(20),
                        Ledger_PAN NVARCHAR(10),
                        Ledger_open DECIMAL(18,2) DEFAULT 0,
                        Balance_Type NVARCHAR(10) DEFAULT 'Debit',
                        Credit_Limit DECIMAL(18,2) DEFAULT 0,
                        Credit_Days INT DEFAULT 0,
                        Is_TDS_Applicable BIT DEFAULT 0,
                        Use_GST BIT DEFAULT 1,
                        Ledger_GST NVARCHAR(15),
                        GST_DealerType NVARCHAR(20),
                        GST_StateCode NVARCHAR(5),
                        Ledger_remarks NVARCHAR(MAX),
                        Created_Date DATETIME DEFAULT GETDATE(),
                        Modified_Date DATETIME NULL
                    );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Color_Table')
                    BEGIN
                        CREATE TABLE Color_Table (
                        Color_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                        Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                        User_No INT,
                        Color_Name NVARCHAR(100),
                        Color_HexCode NVARCHAR(20),
                        IsActive BIT DEFAULT 1,
                        Created_Date DATETIME DEFAULT GETDATE(),
                        Modified_Date DATETIME DEFAULT GETDATE()
                    );
                    END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ItemGroup_Table')
                    BEGIN
                        CREATE TABLE ItemGroup_Table (
                            ItemGroup_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                            Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                            User_No INT REFERENCES User_Table(User_Sno),
                            ItemGroup_Name NVARCHAR(100), ItemGroup_Under INT, IsActive BIT DEFAULT 1,
                            Created_Date DATETIME DEFAULT GETDATE()
                        );
                    END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Unit_Table')
                    BEGIN
                        CREATE TABLE Unit_Table (
                            Unit_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                            Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                            Unit_Name NVARCHAR(50), Unit_Sname NVARCHAR(20), Decimal_Places INT DEFAULT 0, IsActive BIT DEFAULT 1,
                            Created_Date DATETIME DEFAULT GETDATE()
                        );
                    END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinYear_Table')
                BEGIN
                    CREATE TABLE FinYear_Table (
                    FY_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                    Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                    FY_Name NVARCHAR(20),
                    StartDate DATE,
                    EndDate DATE,
                    Created_Date DATETIME DEFAULT GETDATE()
                );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Control_Table')
                BEGIN
                    CREATE TABLE Control_Table (
                    Control_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                    Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                    Ctl_MtDesc NVARCHAR(100),
                    Ctl_Value NVARCHAR(500),
                    Modified_Date DATETIME DEFAULT GETDATE(),
                    CONSTRAINT UQ_Company_Setting UNIQUE (Company_No, Ctl_MtDesc) 
                );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WeaveType_Table')
                BEGIN
                CREATE TABLE WeaveType_Table (
                    Weave_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                    Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                    User_No INT,
                    Weave_Name NVARCHAR(100),
                    IsActive BIT DEFAULT 1,
                    Created_Date DATETIME DEFAULT GETDATE(),
                    Modified_Date DATETIME DEFAULT GETDATE()
                );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ClosingStk_table')
                BEGIN
                CREATE TABLE ClosingStk_table (
                    Stk_Sno INT PRIMARY KEY IDENTITY(1,1),
                    Item_no INT,
                    Company_No BIGINT,
                    Stk_Value DECIMAL(18,2),
                    Stk_date DATE
                );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Item_Table')
                BEGIN
                    CREATE TABLE Item_Table (
                    Item_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                    Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                    User_No INT,
                    Item_Name NVARCHAR(200),
                    Item_Code NVARCHAR(50),
                    HSN_Code NVARCHAR(20),
                    ItemGroup_No INT,
                    SubCategory_Sno INT,
                    Color_Sno INT,
                    Weave_Sno INT, 
                    GST_Sno INT,
                    Barcode NVARCHAR(50), 
                    Item_Image NVARCHAR(MAX),
                    Min_Stock DECIMAL(18,2) DEFAULT 0,
                    Max_Stock DECIMAL(18,2) DEFAULT 0,
                    Batch_Enabled BIT DEFAULT 0,
                    Serial_Enabled BIT DEFAULT 0;
                    ItemUnit_No INT,
                    AltUnit_No INT,
                    Conv_Factor DECIMAL(18, 4) DEFAULT 1,
                    GST_Rate DECIMAL(5, 2) DEFAULT 0,
                    Purchase_Rate DECIMAL(18, 2) DEFAULT 0,
                    Selling_Price DECIMAL(18, 2) DEFAULT 0,
                    IsActive BIT DEFAULT 1,
                    Created_Date DATETIME DEFAULT GETDATE(),
                    Modified_Date DATETIME DEFAULT GETDATE()
                );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Item_Table') AND name = 'Item_Type')
                BEGIN
                 ALTER TABLE Item_Table ADD Item_Type NVARCHAR(20);
                END");          

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ItemSubCategory_Table')
                BEGIN
                CREATE TABLE ItemSubCategory_Table (
                    SubCat_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                    Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                    User_No INT,
                    Category_No INT REFERENCES ItemGroup_Table(ItemGroup_Sno),
                    SubCat_Name NVARCHAR(100),
                    IsActive BIT DEFAULT 1,
                    Created_Date DATETIME DEFAULT GETDATE(),
                    Modified_Date DATETIME DEFAULT GETDATE()
                );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UnitConversion_Table')
                BEGIN
                    CREATE TABLE UnitConversion_Table (
                        Conv_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                        Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                        MainUnit_Sno INT REFERENCES Unit_Table(Unit_Sno),
                        SubUnit_Sno INT REFERENCES Unit_Table(Unit_Sno),
                        Multiplier DECIMAL(18, 4),
                        IsActive BIT DEFAULT 1,
                        Created_Date DATETIME DEFAULT GETDATE()
                    );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'VoucherType_Table')
                BEGIN
                    CREATE TABLE VoucherType_Table (
                        VoucherType_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                        Company_No BIGINT REFERENCES Company_Table(Company_Sno),
                        Voucher_Name NVARCHAR(50),
                        Prefix NVARCHAR(20),
                        Suffix NVARCHAR(20),
                        Start_No INT DEFAULT 1,
                        Current_No INT DEFAULT 0,
                        Padding_Width INT DEFAULT 5,
                        Main_Ledger_Sno BIGINT NULL,
                        Discount_Ledger_Sno BIGINT NULL,
                        RoundOff_Ledger_Sno BIGINT NULL,
                        Print_Title NVARCHAR(100) NULL,
                        Is_Tax_Inclusive BIT DEFAULT 0,
                        IsActive BIT DEFAULT 1,
                        Created_Date DATETIME DEFAULT GETDATE()
                    );
                END");

                Execute(conn, @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'GST_Table')
                BEGIN
                    CREATE TABLE GST_Table (
                    GST_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                    Company_No BIGINT,
                    Tax_Name NVARCHAR(100),
                    Print_Name NVARCHAR(100),
                    SGST_Rate DECIMAL(5,2),
                    CGST_Rate DECIMAL(5,2),
                    IGST_Rate DECIMAL(5,2),
                    CESS_Rate DECIMAL(5,2),
                    SGST_LocSales_Acount BIGINT,
                    SGST_SalesTax_Ledger BIGINT,
                    SGST_LocPur_Account  BIGINT,
                    SGST_PurTax_Ledger   BIGINT,    
                    CGST_SalesTax_Ledger BIGINT,
                    CGST_PurTax_Ledger   BIGINT,
                    IGST_IntSales_Acount BIGINT,
                    IGST_SalesTax_Ledger BIGINT,
                    IGST_IntPur_Account  BIGINT,
                    IGST_PurTax_Ledger   BIGINT,    
                    CESS_SalesTax_Ledger BIGINT,
                    CESS_PurTax_Ledger   BIGINT,    
                    IsActive BIT DEFAULT 1,
                    Created_Date DATETIME DEFAULT GETDATE()
                );
                END");

                string VTSql = @"IF (SELECT COUNT(*) FROM VoucherType_Table WHERE Company_No = @cid) = 0
                BEGIN
                    INSERT INTO VoucherType_Table (Company_No, Voucher_Name, Prefix, Suffix, Start_No, Current_No, Padding_Width)
                    VALUES 
                    -- Purchase Transactions
                    (@cid, 'Purchase Order', 'PO/', '', 1, 0, 5),
                    (@cid, 'GRN Creation', 'GRN/', '', 1, 0, 5),
                    (@cid, 'Purchase Invoice', 'PUR/', '', 1, 0, 5),
                    (@cid, 'Purchase Return', 'PR/', '', 1, 0, 5),

                    -- Production Transactions
                    (@cid, 'Production Order', 'PRO/', '', 1, 0, 5),
                    (@cid, 'Job Work Issue', 'JWI/', '', 1, 0, 5),
                    (@cid, 'Job Work Receipt', 'JWR/', '', 1, 0, 5),
                    (@cid, 'Production Entry', 'PE/', '', 1, 0, 5),
                    (@cid, 'Quality Check', 'QC/', '', 1, 0, 5),
                    (@cid, 'Wastage Entry', 'WST/', '', 1, 0, 5),

                    -- Boutique - Customization
                    (@cid, 'Measurement Chart', 'MC/', '', 1, 0, 5),
                    (@cid, 'Order Slip', 'OS/', '', 1, 0, 5),
                    (@cid, 'Stitching Status', 'ST/', '', 1, 0, 5),
                    (@cid, 'Trial Appointment', 'TA/', '', 1, 0, 5),
                    (@cid, 'Final Payment', 'FP/', '', 1, 0, 5),

                    -- Sales Transactions
                    (@cid, 'Sales Order', 'SO/', '', 1, 0, 5),
                    (@cid, 'Sales Invoice', 'SAL/', '', 1, 0, 5),
                    (@cid, 'Sales Return', 'SR/', '', 1, 0, 5),
                    (@cid, 'Quotation', 'QUO/', '', 1, 0, 5),
                    (@cid, 'Delivery Note', 'DN/', '', 1, 0, 5),

                    -- Stock Management
                    (@cid, 'Stock Adjustment', 'ADJ/', '', 1, 0, 5),
                    (@cid, 'Godown Transfer', 'GT/', '', 1, 0, 5);
                    -- REMOVED IDENTITY_INSERT OFF
                END";

                using (SqlCommand cmd = new SqlCommand(VTSql, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", cid);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void Execute(SqlConnection conn, string sql)
        {
            try { using (SqlCommand cmd = new SqlCommand(sql, conn)) { cmd.ExecuteNonQuery(); } }
            catch (Exception ex) { throw new Exception("SQL Setup Error: " + ex.Message); }
        }
    }
}