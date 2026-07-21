using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BKBilling.Class
{
    public class CompanyTable
    {
        public static void CreateAllSchema()
        {
            string appConnStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(appConnStr);

            string serverName = builder.DataSource;
            string databaseName = builder.InitialCatalog;
            string dbUser = builder.UserID;
            string dbPass = builder.Password;

            string masterConnStr = $"Data Source={serverName};Initial Catalog=master;Integrated Security=True;";

            using (SqlConnection masterConn = new SqlConnection(masterConnStr))
            {
                masterConn.Open();

                Execute(masterConn, $@"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{databaseName}')
                    BEGIN
                        CREATE DATABASE [{databaseName}];
                    END");

                if (!string.IsNullOrEmpty(dbUser))
                {
                    Execute(masterConn, $@"
                        IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'{dbUser}')
                        BEGIN
                            CREATE LOGIN [{dbUser}] WITH PASSWORD = N'{dbPass}', 
                            DEFAULT_DATABASE = [{databaseName}], CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF;
                        END
                        ELSE
                        BEGIN
                            ALTER LOGIN [{dbUser}] WITH PASSWORD = N'{dbPass}';
                        END");
                }
            }

            //string dbLevelConnStr = $"Data Source={serverName};Initial Catalog={databaseName};User ID={dbUser};Password={dbPass};";
            string dbLevelConnStr = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(dbLevelConnStr))
            {
                conn.Open();

                if (!string.IsNullOrEmpty(dbUser))
                {
                    Execute(conn, $@"
                        IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'{dbUser}')
                        BEGIN
                            CREATE USER [{dbUser}] FOR LOGIN [{dbUser}];
                        END
                        ALTER ROLE db_owner ADD MEMBER [{dbUser}];");
                }

                // Company Table
                Execute(conn, @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Company_Table')
                    BEGIN
                        CREATE TABLE Company_Table (
                            Company_Sno BIGINT PRIMARY KEY IDENTITY(1000000000, 1),
                            Company_Name NVARCHAR(150) NOT NULL,
                            Address_1 NVARCHAR(255), Address_2 NVARCHAR(255),
                            State_Name NVARCHAR(100), GSTIN NVARCHAR(15), PAN NVARCHAR(10),
                            Country NVARCHAR(100), Phone NVARCHAR(20), Email NVARCHAR(100),
                            Website NVARCHAR(100), Currency_Symbol NVARCHAR(10),
                            Currency_Format NVARCHAR(50), Financial_Year DATE,
                            Created_Date DATETIME DEFAULT GETDATE(), Modified_Date DATETIME
                        );
                    END");

                Execute(conn, @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Active_Sessions ')
                    BEGIN
                        CREATE TABLE Active_Sessions (
                            Username    NVARCHAR(100) NOT NULL PRIMARY KEY,
                            CompanyID   NVARCHAR(50)  NOT NULL,
                            AuthToken   NVARCHAR(100) NOT NULL,
                            LoginTime   DATETIME      NOT NULL DEFAULT GETDATE(),
                            ExpiryTime  DATETIME      NOT NULL
                        );
                    END");

                // User Table
                Execute(conn, @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User_Table')
                    BEGIN
                        CREATE TABLE User_Table (
                            User_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                            Company_No BIGINT NOT NULL REFERENCES Company_Table(Company_Sno),
                            Username NVARCHAR(50) UNIQUE, Password NVARCHAR(100),
                            FullName NVARCHAR(100), Role NVARCHAR(20), IsActive BIT DEFAULT 1,
                            Address_1 NVARCHAR(255), Address_2 NVARCHAR(255),
                            Phone NVARCHAR(20), Email NVARCHAR(100), Join_Date DATE,
                            Created_Date DATETIME DEFAULT GETDATE(), Modified_Date DATETIME
                        );
                    END");

                // Form Settings
                Execute(conn, @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Form_Settings')
                    BEGIN
                        CREATE TABLE Form_Settings (
                            Setting_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),
                            Company_ID BIGINT NOT NULL REFERENCES Company_Table(Company_Sno),
                            Form_Name NVARCHAR(100),
                            Control_ID NVARCHAR(100),
                            Is_Enabled BIT DEFAULT 1
                        );
                    END");

            }

            SqlConnection.ClearAllPools();
        }

        private static void Execute(SqlConnection conn, string sql)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Setup Error: {ex.Message} \nQuery: {sql.Trim().Substring(0, Math.Min(sql.Trim().Length, 100))}");
            }
        }
    }
}