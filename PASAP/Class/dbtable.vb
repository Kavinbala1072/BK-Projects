Imports System.Data.SqlClient

Public Class dbtable
    Public Shared Sub InitializeDatabase()
        Try
            Tools.LoadConfiguration()
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Dim SqlCommand As SqlCommand = sqlconnect.CreateCommand()
                SqlCommand.CommandType = CommandType.Text

                ' Create table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'User_table') " &
                                         "BEGIN " &
                                         "CREATE TABLE User_table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, User_Name VARCHAR(255) NOT NULL, User_Password VARCHAR(255));" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AutoBackup_table') " &
                                         "BEGIN " &
                                         "CREATE TABLE AutoBackup_table (BackupDate DATE PRIMARY KEY, LastRunTime DATETIME NOT NULL, BackupStatus VARCHAR(20) NOT NULL, BackupFile VARCHAR(300) NULL, ErrorMessage VARCHAR(500) NULL );" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserRight_Table')" &
                                            "BEGIN " &
                                            "CREATE TABLE UserRight_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, User_ID INT NOT NULL, Menu_ID VARCHAR(10) NOT NULL ,Menu_Name VARCHAR(500) NOT NULL, IsAllowed BIT NOT NULL DEFAULT 0,FOREIGN KEY (User_ID) REFERENCES User_table(ID));" &
                                            "END"
                SqlCommand.ExecuteNonQuery()

                ' Insert default Admin record if not exists
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM User_table WHERE User_Name = 'Admin') " &
                                         "BEGIN " &
                                         "INSERT INTO User_table (User_Name, User_Password) VALUES ('Admin', 'a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s=');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()


                ' Create V_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'V_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE V_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Vt_Name VARCHAR(255) NOT NULL, Vt_Prefix VARCHAR(10), Vt_Suffix VARCHAR(10), Vt_Billno INT);" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Insert default records into V_Table if they don't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM V_Table WHERE Vt_Name = 'Receipt') " &
                                     "BEGIN " &
                                     "INSERT INTO V_Table (Vt_Name, Vt_Prefix, Vt_Suffix, Vt_Billno) VALUES ('Receipt', 'R/', '/26-27', 1);" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM V_Table WHERE Vt_Name = 'Payment') " &
                                     "BEGIN " &
                                     "INSERT
                                     INTO V_Table (Vt_Name, Vt_Prefix, Vt_Suffix, Vt_Billno) VALUES ('Payment', 'P/', '/26-27', 1);" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Create Company_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Company_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE Company_Table (ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), Comp_Name VARCHAR(255) NOT NULL, Comp_Address1 VARCHAR(255) NOT NULL, Comp_Address2 VARCHAR(255) NOT NULL, Comp_Address3 VARCHAR(255) NOT NULL, Mobile VARCHAR(255) NOT NULL, Comp_No VARCHAR(255) NOT NULL DEFAULT 'BK0002');" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Insert default row if table is empty
                SqlCommand.CommandText = "UPDATE Company_Table SET Comp_No = 'BK0002' WHERE Comp_No = 'KR1';
                                            IF NOT EXISTS (SELECT 1 FROM Company_Table)
                                            BEGIN
                                                INSERT INTO Company_Table (Comp_No, Comp_Name, Comp_Address1, Comp_Address2, Comp_Address3, Mobile) 
                                                VALUES ('BK0002', 'BK SOFTWARE', '', '', '', '')
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add 'Version' column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Company_Table' AND COLUMN_NAME = 'Version')
                                            BEGIN
                                                ALTER TABLE Company_Table ADD Version VARCHAR(50) DEFAULT '1.0'
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Create Control_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Control_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE Control_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Ctl_Desc VARCHAR(255) NOT NULL, Ctl_Value VARCHAR(15));" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'UserName') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('UserName', '');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'LastBackupDate') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('LastBackupDate', '01-01-2000');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'LastCheckDate') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('LastCheckDate', '01-01-2000');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'fromDate') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('fromDate', '01-04-2026');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'toDate') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('toDate', '31-03-2027');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'UserRight') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('UserRight', 0);" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'EnableBackup') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('EnableBackup', 1);" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Member_Table') 
                                         BEGIN 
                                                CREATE TABLE Member_Table (ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),   M_No VARCHAR(50), Member_Name NVARCHAR(255), Mobile_No VARCHAR(15), Address_Text NVARCHAR(MAX),  
                                                Aadhar_No VARCHAR(12),  Remarks NVARCHAR(MAX),   Member_Photo VARBINARY(MAX), Is_Active BIT DEFAULT 1,  Joining_Date DATE,  Created_Date DATETIME DEFAULT GETDATE(),  Modified_Date DATETIME DEFAULT GETDATE(),
                                                User_ID INT, 
                                                CONSTRAINT FK_Member_User FOREIGN KEY (User_ID) REFERENCES User_table(ID) ON DELETE SET NULL);
                                         END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Voucher_Table') 
                                         BEGIN 
                                            CREATE TABLE Voucher_Table ( ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), Bill_No NVARCHAR(50) NOT NULL,  V_Date DATE NOT NULL,  V_Type NVARCHAR(20) NOT NULL, 
                                            Member_ID UNIQUEIDENTIFIER NULL, Amount DECIMAL(18, 2) DEFAULT 0, Purpose NVARCHAR(100), Payment_Method NVARCHAR(50), Remarks NVARCHAR(MAX), User_ID INT, 
                                            Created_Date DATETIME DEFAULT GETDATE(), Modified_Date DATETIME DEFAULT GETDATE(), Is_Cancelled INT DEFAULT 0,
                                                CONSTRAINT FK_Voucher_Member FOREIGN KEY (Member_ID) REFERENCES Member_Table(ID) ON DELETE SET NULL,
                                                CONSTRAINT FK_Voucher_User FOREIGN KEY (User_ID) REFERENCES User_table(ID) ON DELETE SET NULL);
                                         END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Voucher_Table' AND COLUMN_NAME = 'Ledger_ID')
                                        BEGIN
                                            ALTER TABLE Voucher_Table ADD Ledger_ID INT;
                                            ALTER TABLE Voucher_Table 
                                            ADD CONSTRAINT FK_Voucher_Ledger 
                                            FOREIGN KEY (Ledger_ID) REFERENCES Ledger_Table(ID) ON DELETE SET NULL;
                                        END"
                SqlCommand.ExecuteNonQuery()

                ' Create LedgerGroup_Table if it doesn't exist
                SqlCommand.CommandText = "If Not EXISTS (Select * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LedgerGroup_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE LedgerGroup_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, LedgerGroup_Name VARCHAR(255) NOT NULL, Under VARCHAR(255) DEFAULT 'Primary');" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Insert default records into LedgerGroup_Table if they don't exist
                SqlCommand.CommandText = "IF NOT EXISTS (Select * from LedgerGroup_Table where LedgerGroup_Name = 'Bank Account') " &
                                         "BEGIN " &
                                         "INSERT INTO LedgerGroup_Table (LedgerGroup_Name, under) VALUES ('Bank Account', 'Standard Group');" &
                                         "END " &
                                         "IF NOT EXISTS (Select * from LedgerGroup_Table where LedgerGroup_Name = 'Cash In Hand') " &
                                         "BEGIN " &
                                         "INSERT INTO LedgerGroup_Table (LedgerGroup_Name, under) VALUES ('Cash In Hand', 'Standard Group');" &
                                         "END "
                SqlCommand.ExecuteNonQuery()

                ' Create Ledger_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Ledger_Table') " &
                                         "BEGIN " &
                                         "CREATE TABLE Ledger_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Partyname VARCHAR(255) NOT NULL, Under VARCHAR(255),  Opening DECIMAL(18, 2) DEFAULT 0.00); " &
                                         "END; " &
                                         "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Ledger_Table' AND COLUMN_NAME = 'Active') " &
                                         "BEGIN " &
                                         "ALTER TABLE Ledger_Table ADD Active BIT DEFAULT 0; " &
                                         "END; " &
                                        "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Ledger_Table' AND COLUMN_NAME = 'UserID') " &
                                        "BEGIN " &
                                        "    ALTER TABLE Ledger_Table ADD UserID INT DEFAULT 1 NOT NULL; " &
                                        "END; " &
                                        "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_User') " &
                                        "BEGIN " &
                                        "    ALTER TABLE Ledger_Table ADD CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES user_table(ID); " &
                                        "END;"
                SqlCommand.ExecuteNonQuery()


                SqlCommand.CommandText = "If Not EXISTS (Select * from Ledger_Table where Partyname = 'Cash In Hand') " &
                                         "BEGIN " &
                                         "INSERT INTO Ledger_Table (Partyname, under) VALUES ('Cash In Hand', 'Cash In Hand');" &
                                         "END "
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Member_Table WHERE Member_Name = 'UnMember') " +
                                         "BEGIN " +
                                         "INSERT INTO Member_Table (M_No, Member_Name, Is_Active, Joining_Date) " +
                                         "VALUES ('0', 'UnMember', 1, GETDATE()); " +
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Voucher_Table' AND COLUMN_NAME = 'Member_Name')
                                             BEGIN
                                                 ALTER TABLE Voucher_Table ADD Member_Name NVARCHAR(255);
                                             END"
                SqlCommand.ExecuteNonQuery()

                MsgBox("created successfully.", MsgBoxStyle.Information)
            End Using
        Catch ex As SqlException
            MsgBox("SQL Error: " & ex.Message)
        Catch ex As Exception
            MsgBox("General Error: " & ex.Message)
        End Try
    End Sub
End Class
