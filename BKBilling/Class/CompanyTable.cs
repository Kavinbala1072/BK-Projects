using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BKBilling.Class
{
    public class CompanyTable
    {
        public static void CreateAllSchema()
        {
            string appConnStr =
                ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

            if (string.IsNullOrWhiteSpace(appConnStr))
                throw new Exception("MyDbConn connection string is missing.");

            SqlConnectionStringBuilder builder =
                new SqlConnectionStringBuilder(appConnStr);

            string serverName = builder.DataSource;
            string databaseName = builder.InitialCatalog;
            string dbUser = builder.UserID;
            string dbPass = builder.Password;

            if (string.IsNullOrWhiteSpace(serverName))
                throw new Exception("SQL Server name is missing in MyDbConn.");

            if (string.IsNullOrWhiteSpace(databaseName))
                throw new Exception("Database name is missing in MyDbConn.");

            if (string.IsNullOrWhiteSpace(dbUser))
                throw new Exception("SQL Login/User ID is missing in MyDbConn.");

            if (string.IsNullOrWhiteSpace(dbPass))
                throw new Exception("SQL Password is missing in MyDbConn.");

            // ---------------------------------------------------------
            // 1. Connect to MASTER using Windows Authentication
            // ---------------------------------------------------------
            string masterConnStr =
                $"Data Source={serverName};" +
                $"Initial Catalog=master;" +
                $"Integrated Security=True;" +
                $"TrustServerCertificate=True;";

            using (SqlConnection masterConn =
                   new SqlConnection(masterConnStr))
            {
                masterConn.Open();

                // -----------------------------------------------------
                // Create Database
                // -----------------------------------------------------
                string createDatabaseSql = $@"
IF DB_ID(N'{EscapeSqlString(databaseName)}') IS NULL
BEGIN
    CREATE DATABASE {QuoteIdentifier(databaseName)};
END";

                Execute(masterConn, createDatabaseSql);

                // -----------------------------------------------------
                // Create SQL Login
                // -----------------------------------------------------
                string createLoginSql = $@"
IF NOT EXISTS
(
    SELECT 1
    FROM sys.server_principals
    WHERE name = N'{EscapeSqlString(dbUser)}'
)
BEGIN
    CREATE LOGIN {QuoteIdentifier(dbUser)}
    WITH PASSWORD = N'{EscapeSqlString(dbPass)}',
         DEFAULT_DATABASE = {QuoteIdentifier(databaseName)},
         CHECK_EXPIRATION = OFF,
         CHECK_POLICY = OFF;
END";

                Execute(masterConn, createLoginSql);

                // -----------------------------------------------------
                // Ensure Login Default Database
                // -----------------------------------------------------
                string updateLoginSql = $@"
IF EXISTS
(
    SELECT 1
    FROM sys.server_principals
    WHERE name = N'{EscapeSqlString(dbUser)}'
)
BEGIN
    ALTER LOGIN {QuoteIdentifier(dbUser)}
    WITH DEFAULT_DATABASE = {QuoteIdentifier(databaseName)};
END";

                Execute(masterConn, updateLoginSql);
            }

            // ---------------------------------------------------------
            // 2. Connect to Application Database using SQL Login
            // ---------------------------------------------------------
            string dbLevelConnStr =
                $"Data Source={serverName};" +
                $"Initial Catalog={databaseName};" +
                $"User ID={dbUser};" +
                $"Password={dbPass};" +
                $"TrustServerCertificate=True;";

            using (SqlConnection conn =
                   new SqlConnection(dbLevelConnStr))
            {
                conn.Open();

                // -----------------------------------------------------
                // 3. Create Database User for Login
                // -----------------------------------------------------
                string createUserSql = $@"
IF NOT EXISTS
(
    SELECT 1
    FROM sys.database_principals
    WHERE name = N'{EscapeSqlString(dbUser)}'
)
BEGIN
    CREATE USER {QuoteIdentifier(dbUser)}
    FOR LOGIN {QuoteIdentifier(dbUser)};
END";

                Execute(conn, createUserSql);

                // -----------------------------------------------------
                // 4. Add User to db_owner
                // -----------------------------------------------------
                string addRoleSql = $@"
IF NOT EXISTS
(
    SELECT 1
    FROM sys.database_role_members drm
    INNER JOIN sys.database_principals rolePrincipal
        ON drm.role_principal_id = rolePrincipal.principal_id
    INNER JOIN sys.database_principals userPrincipal
        ON drm.member_principal_id = userPrincipal.principal_id
    WHERE rolePrincipal.name = N'db_owner'
      AND userPrincipal.name = N'{EscapeSqlString(dbUser)}'
)
BEGIN
    ALTER ROLE [db_owner]
    ADD MEMBER {QuoteIdentifier(dbUser)};
END";

                Execute(conn, addRoleSql);

                // -----------------------------------------------------
                // 5. Company Table
                // -----------------------------------------------------
                string companyTableSql = @"
IF OBJECT_ID(N'dbo.Company_Table', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Company_Table
    (
        Company_Sno BIGINT PRIMARY KEY IDENTITY(1000000000, 1),
        Company_Name NVARCHAR(150) NOT NULL,
        Address_1 NVARCHAR(255),
        Address_2 NVARCHAR(255),
        State_Name NVARCHAR(100),
        GSTIN NVARCHAR(15),
        PAN NVARCHAR(10),
        Country NVARCHAR(100),
        Phone NVARCHAR(20),
        Email NVARCHAR(100),
        Website NVARCHAR(100),
        Currency_Symbol NVARCHAR(10),
        Currency_Format NVARCHAR(50),
        Financial_Year DATE,
        Created_Date DATETIME DEFAULT GETDATE(),
        Modified_Date DATETIME
    );
END";

                Execute(conn, companyTableSql);

                // -----------------------------------------------------
                // 6. Active Sessions Table
                // -----------------------------------------------------
                string activeSessionsSql = @"
IF OBJECT_ID(N'dbo.Active_Sessions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Active_Sessions
    (
        Username NVARCHAR(100) NOT NULL PRIMARY KEY,
        CompanyID NVARCHAR(50) NOT NULL,
        AuthToken NVARCHAR(100) NOT NULL,
        LoginTime DATETIME NOT NULL DEFAULT GETDATE(),
        ExpiryTime DATETIME NOT NULL
    );
END";

                Execute(conn, activeSessionsSql);

                // -----------------------------------------------------
                // 7. User Table
                // -----------------------------------------------------
                string userTableSql = @"
IF OBJECT_ID(N'dbo.User_Table', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.User_Table
    (
        User_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),

        Company_No BIGINT NOT NULL
            REFERENCES dbo.Company_Table(Company_Sno),

        Username NVARCHAR(50) UNIQUE,
        Password NVARCHAR(100),
        FullName NVARCHAR(100),
        Role NVARCHAR(20),
        IsActive BIT DEFAULT 1,

        Address_1 NVARCHAR(255),
        Address_2 NVARCHAR(255),
        Phone NVARCHAR(20),
        Email NVARCHAR(100),
        Join_Date DATE,

        Created_Date DATETIME DEFAULT GETDATE(),
        Modified_Date DATETIME
    );
END";

                Execute(conn, userTableSql);

                // -----------------------------------------------------
                // 8. Form Settings Table
                // -----------------------------------------------------
                string formSettingsSql = @"
IF OBJECT_ID(N'dbo.Form_Settings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Form_Settings
    (
        Setting_Sno INT PRIMARY KEY IDENTITY(1000000000, 1),

        Company_ID BIGINT NOT NULL
            REFERENCES dbo.Company_Table(Company_Sno),

        Form_Name NVARCHAR(100),
        Control_ID NVARCHAR(100),
        Is_Enabled BIT DEFAULT 1
    );
END";

                Execute(conn, formSettingsSql);
            }

            // ---------------------------------------------------------
            // Clear connection pools
            // ---------------------------------------------------------
            SqlConnection.ClearAllPools();
        }

        // =============================================================
        // Execute SQL
        // =============================================================
        private static void Execute(SqlConnection conn, string sql)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string queryPreview = sql.Trim();

                if (queryPreview.Length > 300)
                    queryPreview = queryPreview.Substring(0, 300);

                throw new Exception(
                    $"SQL Setup Error: {ex.Message}\r\n" +
                    $"SQL Error Number: {ex.Number}\r\n" +
                    $"Query: {queryPreview}",
                    ex);
            }
            catch (Exception ex)
            {
                string queryPreview = sql.Trim();

                if (queryPreview.Length > 300)
                    queryPreview = queryPreview.Substring(0, 300);

                throw new Exception(
                    $"SQL Setup Error: {ex.Message}\r\n" +
                    $"Query: {queryPreview}",
                    ex);
            }
        }

        // =============================================================
        // Escape SQL string values
        // =============================================================
        private static string EscapeSqlString(string value)
        {
            if (value == null)
                return string.Empty;

            return value.Replace("'", "''");
        }

        // =============================================================
        // Safely quote SQL identifiers
        // =============================================================
        private static string QuoteIdentifier(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("SQL identifier cannot be empty.");

            return "[" + value.Replace("]", "]]") + "]";
        }
    }
}