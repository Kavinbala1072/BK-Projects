using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace BKSoftwares
{
    public partial class SetupDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSetup_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["AdminDbConn"].ConnectionString;
            string databaseName = "BKSoftware";
            string dbUser = databaseName + "_user";

            var steps = new List<(string Name, string Sql)>();

            steps.Add(("Create Login", $@"
                USE [master];
                IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'{dbUser}')
                BEGIN
                    CREATE LOGIN [{dbUser}] WITH PASSWORD = N'K@vin2000',
                    DEFAULT_DATABASE = [{databaseName}],
                    CHECK_EXPIRATION = OFF,
                    CHECK_POLICY = OFF;
                END"));

            steps.Add(("Create Database", $@"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{databaseName}')
                BEGIN
                    CREATE DATABASE [{databaseName}];
                END"));

            steps.Add(("Create Database User", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'{dbUser}')
                BEGIN
                    CREATE USER [{dbUser}] FOR LOGIN [{dbUser}];
                END"));

            steps.Add(("Grant db_owner Role", $@"
                USE [{databaseName}];
                EXEC sp_addrolemember 'db_owner', '{dbUser}';"));

            steps.Add(("Create Users Table", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                CREATE TABLE Users (
                    UserID INT PRIMARY KEY IDENTITY(1,1),
                    Username NVARCHAR(50) UNIQUE,
                    Password NVARCHAR(MAX),
                    FullName NVARCHAR(100)
                );"));

            steps.Add(("Create Customers Table", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
                BEGIN
                    CREATE TABLE Customers
                    (
                        CustomerID INT PRIMARY KEY IDENTITY(1,1),
                        CustCode AS 
                        ('BK' + 
                            CASE 
                                WHEN CustomerID < 10 THEN '000' + CAST(CustomerID AS VARCHAR(10))
                                WHEN CustomerID < 100 THEN '00' + CAST(CustomerID AS VARCHAR(10))
                                WHEN CustomerID < 1000 THEN '0' + CAST(CustomerID AS VARCHAR(10))
                                ELSE CAST(CustomerID AS VARCHAR(10))
                            END),
                        CustomerName NVARCHAR(150) NOT NULL,
                        Phone NVARCHAR(20),
                        Email NVARCHAR(100),
                        City NVARCHAR(100),
                        Address NVARCHAR(MAX),
                        Application NVARCHAR(100),
                        CompanyName NVARCHAR(250),
                        SystemCount INT DEFAULT 0,
                        OpeningBalance DECIMAL(18,2) DEFAULT 0
                    )
                END"));

            steps.Add(("Create Vouchers Table", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Vouchers')
                CREATE TABLE Vouchers (
                    VoucherID INT PRIMARY KEY IDENTITY(1,1),
                    VoucherNo AS ('VCH-' + CAST(VoucherID AS NVARCHAR(10))),
                    VoucherDate DATETIME DEFAULT GETDATE(),
                    VoucherType NVARCHAR(10),
                    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
                    Amount DECIMAL(18,2),
                    PaymentMode NVARCHAR(50),
                    Narration NVARCHAR(MAX)
                );"));

            steps.Add(("Insert Default Admin User", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
                INSERT INTO Users (Username, Password, FullName) VALUES ('admin', 'a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s=', 'System Admin');"));

            steps.Add(("Add Customers.Address Column", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Customers') AND name = 'Address')
                    ALTER TABLE Customers ADD Address NVARCHAR(MAX);"));

            steps.Add(("Add Customers.City Column", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Customers') AND name = 'City')
                    ALTER TABLE Customers ADD City NVARCHAR(100);"));

            steps.Add(("Add Customers.Email Column", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Customers') AND name = 'Email')
                    ALTER TABLE Customers ADD Email NVARCHAR(100);"));

            steps.Add(("Add Customers.Application Column", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Customers') AND name = 'Application')
                    ALTER TABLE Customers ADD Application NVARCHAR(100);"));

            steps.Add(("Add Customers.CompanyName Column", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Customers') AND name = 'CompanyName')
                    ALTER TABLE Customers ADD CompanyName NVARCHAR(250);"));

            steps.Add(("Add Customers.SystemCount Column", $@"
                USE [{databaseName}];
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Customers') AND name = 'SystemCount')
                    ALTER TABLE Customers ADD SystemCount INT DEFAULT 0;"));

            string currentStep = "Opening Connection";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    foreach (var step in steps)
                    {
                        currentStep = step.Name;
                        using (SqlCommand cmd = new SqlCommand(step.Sql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                lblStatus.Text = $@"<b>Success!</b> Database is ready and User '{dbUser}' is configured. 
                                  <br/><a href='AppLogin.aspx' class='btn btn-success mt-2'>Go to Login</a>";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error during step '{currentStep}': {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}