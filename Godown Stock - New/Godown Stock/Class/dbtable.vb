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
                                         "INSERT INTO User_table (User_Name, User_Password) VALUES ('Admin', 'Admin');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                ' Create ItemGroup_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ItemGroup_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE ItemGroup_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, ItemGroup_Name VARCHAR(255) NOT NULL, Under VARCHAR(255) DEFAULT 'Primary');" &
                                     "END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (Select * from ItemGroup_Table where ItemGroup_Name = 'Primary') " &
                                         "BEGIN " &
                                         "INSERT INTO ItemGroup_Table (ItemGroup_Name, under) VALUES ('Primary', 'Primary');" &
                                         "END "
                SqlCommand.ExecuteNonQuery()

                ' Create ItemModel_Table if it doesn't exist
                SqlCommand.CommandText = "If Not EXISTS (Select * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ItemModel_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE ItemModel_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, ItemModel_Name VARCHAR(255) NOT NULL, Under VARCHAR(255) DEFAULT 'Primary');" &
                                     "END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (Select * from ItemModel_Table where ItemModel_Name = 'Primary') " &
                                         "BEGIN " &
                                         "INSERT INTO ItemModel_Table (ItemModel_Name, under) VALUES ('Primary', 'Primary');" &
                                         "END "
                SqlCommand.ExecuteNonQuery()

                ' Create ItemBrand_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ItemBrand_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE ItemBrand_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, ItemBrand_Name VARCHAR(255) NOT NULL, Under VARCHAR(255) DEFAULT 'Primary');" &
                                     "END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (Select * from ItemBrand_Table where ItemBrand_Name = 'Primary') " &
                                         "BEGIN " &
                                         "INSERT INTO ItemBrand_Table (ItemBrand_Name, under) VALUES ('Primary', 'Primary');" &
                                         "END "
                SqlCommand.ExecuteNonQuery()

                ' Create ItemUnit_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ItemUnit_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE ItemUnit_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, ItemUnit_Name VARCHAR(255) NOT NULL, Under VARCHAR(255) DEFAULT 'Primary');" &
                                     "END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (Select * from ItemUnit_Table where ItemUnit_Name = 'Nos') " &
                                         "BEGIN " &
                                         "INSERT INTO ItemUnit_Table (ItemUnit_Name, under) VALUES ('Nos', 'Primary');" &
                                         "END "
                SqlCommand.ExecuteNonQuery()

                ' Create Item_Table if it doesn't exist and add a new integer column (e.g., 'Quantity', 'MinStock') if needed
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Item_Table') 
                                        BEGIN
                                            CREATE TABLE Item_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Itemname VARCHAR(255) NOT NULL, Unit VARCHAR(255), Itemgroup VARCHAR(255),Itembrand VARCHAR(255),Itemmodel VARCHAR(255));
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Item_Table' AND COLUMN_NAME = 'Quantity' AND TABLE_SCHEMA = 'dbo')
                                        BEGIN
                                            ALTER TABLE dbo.Item_Table ADD Quantity INT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Item_Table' AND COLUMN_NAME = 'Active' AND TABLE_SCHEMA = 'dbo')
                                        BEGIN
                                            ALTER TABLE dbo.Item_Table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Item_Table' AND COLUMN_NAME = 'MinStock' AND TABLE_SCHEMA = 'dbo')
                                        BEGIN
                                            ALTER TABLE dbo.Item_Table ADD MinStock INT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Item_Table' AND COLUMN_NAME = 'ItemGroup_ID')
                                        BEGIN
                                            ALTER TABLE Item_Table ADD ItemGroup_ID INT NULL;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Item_Table' AND COLUMN_NAME = 'ItemBrand_ID')
                                        BEGIN
                                            ALTER TABLE Item_Table ADD ItemBrand_ID INT NULL;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Item_Table' AND COLUMN_NAME = 'ItemModel_ID')
                                        BEGIN
                                            ALTER TABLE Item_Table ADD ItemModel_ID INT NULL;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Item_Table' AND COLUMN_NAME = 'ItemUnit_ID')
                                        BEGIN
                                            ALTER TABLE Item_Table ADD ItemUnit_ID INT NULL;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Item_Table' AND COLUMN_NAME = 'UserID')
                                        BEGIN
                                            ALTER TABLE Item_Table ADD UserID INT NOT NULL Default 1;
                                        END;

                                        -- Step 3: Add foreign key constraints if not already present
                                        IF NOT EXISTS (
                                            SELECT * FROM sys.foreign_keys WHERE name = 'FK_ItemGroup'
                                        )
                                        BEGIN
                                            ALTER TABLE Item_Table
                                            ADD CONSTRAINT FK_ItemGroup FOREIGN KEY (ItemGroup_ID) REFERENCES ItemGroup_Table(ID);
                                        END;

                                        IF NOT EXISTS (
                                            SELECT * FROM sys.foreign_keys WHERE name = 'FK_ItemBrand'
                                        )
                                        BEGIN
                                            ALTER TABLE Item_Table
                                            ADD CONSTRAINT FK_ItemBrand FOREIGN KEY (ItemBrand_ID) REFERENCES ItemBrand_Table(ID);
                                        END;

                                        IF NOT EXISTS (
                                            SELECT * FROM sys.foreign_keys WHERE name = 'FK_ItemModel'
                                        )
                                        BEGIN
                                            ALTER TABLE Item_Table
                                            ADD CONSTRAINT FK_ItemModel FOREIGN KEY (ItemModel_ID) REFERENCES ItemModel_Table(ID);
                                        END;

                                        IF NOT EXISTS (
                                            SELECT * FROM sys.foreign_keys WHERE name = 'FK_ItemUnit'
                                        )
                                        BEGIN
                                            ALTER TABLE Item_Table
                                            ADD CONSTRAINT FK_ItemUnit FOREIGN KEY (ItemUnit_ID) REFERENCES ItemUnit_Table(ID);
                                        END

                                        IF NOT EXISTS (
                                            SELECT * FROM sys.foreign_keys WHERE name = 'FK_User'
                                        )
                                        BEGIN
                                            ALTER TABLE Item_Table
                                            ADD CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES user_table(ID);
                                        END;;"


                SqlCommand.ExecuteNonQuery()

                ' Create LedgerGroup_Table if it doesn't exist
                SqlCommand.CommandText = "If Not EXISTS (Select * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LedgerGroup_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE LedgerGroup_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, LedgerGroup_Name VARCHAR(255) NOT NULL, Under VARCHAR(255) DEFAULT 'Primary');" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Insert default records into LedgerGroup_Table if they don't exist
                SqlCommand.CommandText = "IF NOT EXISTS (Select * from LedgerGroup_Table where LedgerGroup_Name = 'Supplier') " &
                                         "BEGIN " &
                                         "INSERT INTO LedgerGroup_Table (LedgerGroup_Name, under) VALUES ('Supplier', 'Standard Group');" &
                                         "END " &
                                         "IF NOT EXISTS (Select * from LedgerGroup_Table where LedgerGroup_Name = 'Customer') " &
                                         "BEGIN " &
                                         "INSERT INTO LedgerGroup_Table (LedgerGroup_Name, under) VALUES ('Customer', 'Standard Group');" &
                                         "END "
                SqlCommand.ExecuteNonQuery()

                ' Create Ledger_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Ledger_Table') " &
                                         "BEGIN " &
                                         "CREATE TABLE Ledger_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Partyname VARCHAR(255) NOT NULL, Under VARCHAR(255), Mobile VARCHAR(255)); " &
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


                SqlCommand.CommandText = "If Not EXISTS (Select * from Ledger_Table where Partyname = 'Cash Purchase') " &
                                         "BEGIN " &
                                         "INSERT INTO Ledger_Table (Partyname, under) VALUES ('Cash Purchase', 'Supplier');" &
                                         "END " &
                                         "IF NOT EXISTS (Select * from Ledger_Table where Partyname = 'Cash Sales') " &
                                         "BEGIN " &
                                         "INSERT INTO Ledger_Table (Partyname, under) VALUES ('Cash Sales', 'Customer');" &
                                         "END "
                SqlCommand.ExecuteNonQuery()

                ' Create V_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'V_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE V_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Vt_Name VARCHAR(255) NOT NULL, Vt_Prefix VARCHAR(10), Vt_Suffix VARCHAR(10), Vt_Billno INT);" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Insert default records into V_Table if they don't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM V_Table WHERE Vt_Name = 'Purchase') " &
                                     "BEGIN " &
                                     "INSERT INTO V_Table (Vt_Name, Vt_Prefix, Vt_Suffix, Vt_Billno) VALUES ('Purchase', 'P/', '/25-26', 1);" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM V_Table WHERE Vt_Name = 'Sales') " &
                                     "BEGIN " &
                                     "INSERT
                                     INTO V_Table (Vt_Name, Vt_Prefix, Vt_Suffix, Vt_Billno) VALUES ('Sales', 'S/', '/25-26', 1);" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM V_Table WHERE Vt_Name = 'JobCard') " &
                                     "BEGIN " &
                                     "INSERT
                                     INTO V_Table (Vt_Name, Vt_Prefix, Vt_Suffix, Vt_Billno) VALUES ('JobCard', 'JC/', '/25-26', 1);" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM V_Table WHERE Vt_Name = 'Printing') " &
                                     "BEGIN " &
                                     "INSERT
                                     INTO V_Table (Vt_Name, Vt_Prefix, Vt_Suffix, Vt_Billno) VALUES ('Printing', 'P/', '/25-26', 1);" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Create Company_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Company_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE Company_Table (ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), Comp_Name VARCHAR(255) NOT NULL, Comp_Address1 VARCHAR(255) NOT NULL, Comp_Address2 VARCHAR(255) NOT NULL, Comp_Address3 VARCHAR(255) NOT NULL, Mobile VARCHAR(255) NOT NULL, Comp_No VARCHAR(255) NOT NULL DEFAULT 'KR1');" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Insert default row if table is empty
                SqlCommand.CommandText = "UPDATE Company_Table SET Comp_No = 'BK0001' WHERE Comp_No = 'K1-001';
                                            IF NOT EXISTS (SELECT 1 FROM Company_Table)
                                            BEGIN
                                                INSERT INTO Company_Table (Comp_No, Comp_Name, Comp_Address1, Comp_Address2, Comp_Address3, Mobile) 
                                                VALUES ('KR1', 'BK SOFTWARE', 'Address1', 'Address2', 'Address3', '9876543210')
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add 'Version' column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Company_Table' AND COLUMN_NAME = 'Version')
                                            BEGIN
                                                ALTER TABLE Company_Table ADD Version VARCHAR(50) DEFAULT '1.0'
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Create Addons_Master table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Addons_Master')
                                          BEGIN
                                              CREATE TABLE Addons_Master (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Processing_Method VARCHAR(255) NOT NULL);
                                          END;"
                SqlCommand.ExecuteNonQuery()

                ' Create Addons_Table table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Addons_Table')
                                            BEGIN
                                                CREATE TABLE Addons_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Master_ID INT NOT NULL,Processing_Method_Name VARCHAR(255) NOT NULL,
                                                    Value_Name VARCHAR(255) NOT NULL,JC_BillNo VARCHAR(255) NOT NULL,FOREIGN KEY (Master_ID) REFERENCES Addons_Master(ID));
                                            END;"
                SqlCommand.ExecuteNonQuery()

                ' Create Purchase_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Purchase_table') " &
                                     "BEGIN " &
                                     "CREATE TABLE Purchase_table (ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),ledger_id INT NOT NULL,Partyname VARCHAR(100),Bill_No VARCHAR(50),Manual_BillNo VARCHAR(50),Purchase_date DATE,item_id INT NOT NULL,Itemname VARCHAR(100),quantity INT,Rate DECIMAL(10, 2),Total_Amount DECIMAL(15, 2),Cancel BIT DEFAULT 0,EntryType VARCHAR(50),Remarks VARCHAR(100), FOREIGN KEY (ledger_id) REFERENCES Ledger_Table(ID),FOREIGN KEY (item_id) REFERENCES Item_Table(ID));" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                'Add UserID column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Purchase_table' AND COLUMN_NAME = 'UserID')
                                            BEGIN
                                                ALTER TABLE Purchase_table ADD UserID INT DEFAULT 1 NOT NULL;
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add FK for UserID
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_User')
                                        BEGIN
                                            ALTER TABLE Purchase_table
                                            ADD CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES user_table(ID)
                                        END"
                SqlCommand.ExecuteNonQuery()

                ' Create NoteProcessing_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NoteProcessing_table')
                                          BEGIN
                                              CREATE TABLE NoteProcessing_table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(255), TamilName NVARCHAR(255));
                                          END;"
                SqlCommand.ExecuteNonQuery()

                ' Create NoteSize_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NoteSize_table')
                                          BEGIN
                                              CREATE TABLE NoteSize_table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(255), TamilName NVARCHAR(255));
                                          END;"
                SqlCommand.ExecuteNonQuery()

                ' Create NoteType_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NoteType_table')
                                        BEGIN
                                            CREATE TABLE NoteType_table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(255),TamilName NVARCHAR(255))
                                        END"
                SqlCommand.ExecuteNonQuery()

                ' Create JobCard_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'JobCard_table')
                                         BEGIN
                                            CREATE TABLE JobCard_table (ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),Bill_No VARCHAR(50),JobCard_date DATE,ledger_id INT NOT NULL,
                                            Partyname VARCHAR(50),NoteProcessing_Id INT NOT NULL,Note_Processing VARCHAR(100),NoteSize_Id INT NOT NULL,Note_Size VARCHAR(100),
                                            Paper_Size_GSM VARCHAR(100),Sheet VARCHAR(50),Pages VARCHAR(50),Note VARCHAR(50),Reem VARCHAR(50),Finishing VARCHAR(50),Cancel BIT DEFAULT 0,
                                            WorkingStatus VARCHAR(50),Manual_BillNo VARCHAR(50),Finish_Date DATE NOT NULL DEFAULT ('19990101'),
                                            FOREIGN KEY (ledger_id) REFERENCES Ledger_Table(ID),
                                            FOREIGN KEY (NoteProcessing_Id) REFERENCES NoteProcessing_Table(ID),
                                            FOREIGN KEY (NoteSize_Id) REFERENCES NoteSize_Table(ID));
                                            END;"
                SqlCommand.ExecuteNonQuery()

                'Add UserID column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'UserID')
                                            BEGIN
                                                ALTER TABLE JobCard_table ADD UserID INT DEFAULT 1 NOT NULL;
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add FK for UserID
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_User')
                                        BEGIN
                                            ALTER TABLE JobCard_table
                                            ADD CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES user_table(ID)
                                        END"
                SqlCommand.ExecuteNonQuery()

                ' Rename Paper_Size_GSM to Paper_Size
                SqlCommand.CommandText = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'Paper_Size_GSM')
                                            BEGIN
                                                EXEC sp_rename 'JobCard_table.Paper_Size_GSM', 'Paper_Size', 'COLUMN'
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add NoteType_Id column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'NoteType_Id')
                                            BEGIN
                                                ALTER TABLE JobCard_table ADD NoteType_Id INT
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add FK for NoteType_Id
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_NoteType_Id')
                                        BEGIN
                                            ALTER TABLE JobCard_table
                                            ADD CONSTRAINT FK_NoteType_Id FOREIGN KEY (NoteType_Id) REFERENCES NoteType_table(ID)
                                        END"

                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "If Not EXISTS (Select * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'Paper_Brand')
                                        BEGIN
                                            ALTER TABLE JobCard_table ADD Paper_Brand VARCHAR(50)
                                        END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'Paper_GSM')
                                        BEGIN
                                            ALTER TABLE JobCard_table ADD Paper_GSM VARCHAR(50)
                                        END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'Paper_Weight')
                                        BEGIN
                                            ALTER TABLE JobCard_table ADD Paper_Weight VARCHAR(50)
                                        END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'No_Index')
                                        BEGIN
                                            ALTER TABLE JobCard_table ADD No_Index VARCHAR(50)
                                        END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'Wrapper')
                                        BEGIN
                                            ALTER TABLE JobCard_table ADD Wrapper VARCHAR(50)
                                        END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'JobCard_table' AND COLUMN_NAME = 'Remarks')
                                        BEGIN
                                            ALTER TABLE JobCard_table ADD Remarks VARCHAR(255)
                                        END"
                SqlCommand.ExecuteNonQuery()

                ' Create Sales_table if it doesn't exist and add Manual_BillNo column if missing
                SqlCommand.CommandText = "If Not EXISTS ( Select * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Sales_table')
                                          BEGIN
                                          CREATE TABLE Sales_table (ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),ledger_id INT NOT NULL,Partyname VARCHAR(100),Bill_No VARCHAR(50),sale_date DATE,
                                          item_id INT NOT NULL,Itemname VARCHAR(100),quantity INT,Rate DECIMAL(10, 2),Total_Amount DECIMAL(15, 2),Cancel BIT DEFAULT 0,EntryType VARCHAR(50),Remarks VARCHAR(100),
                                          FOREIGN KEY (ledger_id) REFERENCES Ledger_Table(ID),FOREIGN KEY (item_id) REFERENCES Item_Table(ID));
                                          END;
                                          IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sales_table' AND COLUMN_NAME = 'Manual_BillNo')
                                          BEGIN
                                                ALTER TABLE Sales_table ADD Manual_BillNo VARCHAR(50);
                                           END;"
                SqlCommand.ExecuteNonQuery()

                'Add UserID column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Sales_table' AND COLUMN_NAME = 'UserID')
                                            BEGIN
                                                ALTER TABLE Sales_table ADD UserID INT DEFAULT 1 NOT NULL;
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add FK for UserID
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_User')
                                        BEGIN
                                            ALTER TABLE Sales_table
                                            ADD CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES user_table(ID)
                                        END"
                SqlCommand.ExecuteNonQuery()

                ' Create Stock_table if it doesn't exist
                SqlCommand.CommandText = "If Not EXISTS (Select * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stock_table') " &
                                     "BEGIN " &
                                     "CREATE TABLE Stock_table (ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),Bill_No VARCHAR(50),Stock_date DATE,ledger_id INT NOT NULL,item_id INT NOT NULL,Itemname VARCHAR(100),quantity INT,Rate DECIMAL(10, 2),Total_Amount DECIMAL(15, 2),EntryType VARCHAR(50),FOREIGN KEY (ledger_id) REFERENCES Ledger_Table(ID),FOREIGN KEY (item_id) REFERENCES Item_Table(ID));" &
                                     "END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'Stock_table' AND COLUMN_NAME = 'Cancel')
                                        BEGIN
                                            ALTER TABLE Stock_table ADD Cancel BIT DEFAULT 0
                                        END"
                SqlCommand.ExecuteNonQuery()

                'Add UserID column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Stock_table' AND COLUMN_NAME = 'UserID')
                                            BEGIN
                                                ALTER TABLE Stock_table ADD UserID INT DEFAULT 1 NOT NULL;
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add FK for UserID
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_User')
                                        BEGIN
                                            ALTER TABLE Stock_table
                                            ADD CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES user_table(ID)
                                        END"
                SqlCommand.ExecuteNonQuery()

                ' Create PrintingMethod_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrintingMethod_table')
                                          BEGIN
                                              CREATE TABLE PrintingMethod_table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(255), TamilName NVARCHAR(255));
                                          END;"
                SqlCommand.ExecuteNonQuery()

                ' Create PrintingType_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrintingType_table')
                                          BEGIN
                                              CREATE TABLE PrintingType_table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(255), TamilName NVARCHAR(255));
                                          END;"
                SqlCommand.ExecuteNonQuery()

                ' Create PrintingMachine_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrintingMachine_table')
                                          BEGIN
                                              CREATE TABLE PrintingMachine_table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(255), TamilName NVARCHAR(255));
                                          END;"
                SqlCommand.ExecuteNonQuery()

                ' Create PrintingItem_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PrintingItem_table')
                                          BEGIN
                                              CREATE TABLE PrintingItem_table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(255), TamilName NVARCHAR(255));
                                          END;"
                SqlCommand.ExecuteNonQuery()

                ' Create Printing_table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Printing_table')
                                        BEGIN
                                            CREATE TABLE Printing_table ( ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), Bill_No VARCHAR(50),
                                                Printing_date DATE,ledger_id INT NOT NULL,Partyname VARCHAR(50), PrintMethod_Id INT NOT NULL, Paper_Size_GSM VARCHAR(100),
                                                PrintingType_Id INT NOT NULL,PrintingMachine_Id INT NOT NULL,
                                                Printing_Colour VARCHAR(50), Quantity INT,Printing_Details VARCHAR(255),Cancel BIT DEFAULT 0,
                                                WorkingStatus VARCHAR(50),
                                                FOREIGN KEY (ledger_id) REFERENCES Ledger_Table(ID),
                                                FOREIGN KEY (PrintMethod_Id) REFERENCES PrintingMethod_table(ID),
                                                FOREIGN KEY (PrintingType_Id) REFERENCES PrintingType_table(ID),
                                                FOREIGN KEY (PrintingMachine_Id) REFERENCES PrintingMachine_table(ID));
                                        END;"
                SqlCommand.ExecuteNonQuery()

                'Add UserID column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printing_table' AND COLUMN_NAME = 'UserID')
                                            BEGIN
                                                ALTER TABLE Printing_table ADD UserID INT DEFAULT 1 NOT NULL;
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add FK for UserID
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_User')
                                        BEGIN
                                            ALTER TABLE Printing_table
                                            ADD CONSTRAINT FK_User FOREIGN KEY (UserID) REFERENCES user_table(ID)
                                        END"
                SqlCommand.ExecuteNonQuery()

                'Add PrintingItem_Id column if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printing_table' AND COLUMN_NAME = 'PrintingItem_Id')
                                            BEGIN
                                                ALTER TABLE Printing_table ADD PrintingItem_Id INT
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Add FK for PrintingItem_Id
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_PrintingItem_Id')
                                        BEGIN
                                            ALTER TABLE Printing_table
                                            ADD CONSTRAINT FK_PrintingItem_Id FOREIGN KEY (PrintingItem_Id) REFERENCES PrintingItem_table(ID)
                                        END"
                SqlCommand.ExecuteNonQuery()

                ''Add NoteType_Id column if it doesn't exist
                'SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printing_table' AND COLUMN_NAME = 'NoteType_Id')
                '                            BEGIN
                '                                ALTER TABLE Printing_table ADD NoteType_Id INT
                '                            END"
                'SqlCommand.ExecuteNonQuery()

                '' Add FK for NoteType_Id
                'SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_NoteType_Id')
                '                        BEGIN
                '                            ALTER TABLE Printing_table
                '                            ADD CONSTRAINT FK_NoteType_Id FOREIGN KEY (NoteType_Id) REFERENCES NoteType_table(ID)
                '                        END"
                'SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF EXISTS (SELECT * FROM sys.foreign_keys  WHERE name = 'FK_Printing_table_NoteType_Id')
                                            BEGIN
                                                ALTER TABLE Printing_table DROP CONSTRAINT FK_Printing_table_NoteType_Id
                                            END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'Printing_table'  AND COLUMN_NAME = 'NoteType_Id')
                                            BEGIN
                                                ALTER TABLE Printing_table DROP COLUMN NoteType_Id
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Create Item_Table if it doesn't exist and add a new integer column (e.g., 'Quantity', 'MinStock') if needed
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Addons_Master' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.Addons_Master ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ItemBrand_Table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.ItemBrand_Table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ItemGroup_Table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.ItemGroup_Table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ItemModel_Table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.ItemModel_Table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ItemUnit_Table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.ItemUnit_Table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'NoteProcessing_table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.NoteProcessing_table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Notesize_table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.Notesize_table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'NoteType_table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.NoteType_table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintingMachine_table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.PrintingMachine_table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintingMethod_table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.PrintingMethod_table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintingItem_table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.PrintingItem_table ADD Active BIT DEFAULT 0;
                                        END;

                                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PrintingType_table' AND COLUMN_NAME = 'Active')
                                        BEGIN
                                            ALTER TABLE dbo.PrintingType_table ADD Active BIT DEFAULT 0;
                                        END;"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printing_table' AND COLUMN_NAME = 'Finish_Date')
                                            BEGIN
                                                ALTER TABLE Printing_table ADD Finish_Date DATE NOT NULL DEFAULT ('19990101')
                                            END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printing_table' AND COLUMN_NAME = 'Finish')
                                            BEGIN
                                                ALTER TABLE Printing_table ADD Finish VARCHAR(50)
                                            END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printing_table' AND COLUMN_NAME = 'Paper_Brand')
                                            BEGIN
                                                ALTER TABLE Printing_table ADD Paper_Brand VARCHAR(50)
                                            END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Printing_table' AND COLUMN_NAME = 'Paper_Weight')
                                            BEGIN
                                                ALTER TABLE Printing_table ADD Paper_Weight VARCHAR(15) NOT NULL DEFAULT (0)
                                            END"
                SqlCommand.ExecuteNonQuery()

                ' Create Control_Table if it doesn't exist
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Control_Table') " &
                                     "BEGIN " &
                                     "CREATE TABLE Control_Table (ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Ctl_Desc VARCHAR(255) NOT NULL, Ctl_Value VARCHAR(15));" &
                                     "END"
                SqlCommand.ExecuteNonQuery()

                ' Insert default Control_Table record if not exists
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'Print_BoxWidth') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('Print_BoxWidth', 500);" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'Print_PageWidth') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('Print_PageWidth', 800);" &
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

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'UserRight') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('UserRight', 1);" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'EnableBackup') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('EnableBackup', 1);" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'EnableTheme') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('EnableTheme', 0);" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'HeaderColor') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('headerColor', '');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'ScreenColor') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('ScreenColor', '');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'LastNoUpdated') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('LastNoUpdated', '01-04-2025');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'fromDate') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('fromDate', '01-04-2025');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()

                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'toDate') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('toDate', '31-03-2026');" &
                                         "END"
                SqlCommand.ExecuteNonQuery()
                SqlCommand.CommandText = "IF NOT EXISTS (SELECT * FROM Control_Table WHERE Ctl_Desc = 'JC_rowSpacing') " &
                                         "BEGIN " &
                                         "INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('JC_rowSpacing', 0);" &
                                         "END"
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
