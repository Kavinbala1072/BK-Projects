using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using Newtonsoft.Json;

namespace BKSoftwares
{
    public partial class Customers : System.Web.UI.Page
    {
        [WebMethod]
        public static string GetCustomers()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT CustomerID, CustCode, CustomerName, CompanyName, Application, SystemCount, City FROM Customers ORDER BY CustomerID Asc";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn); da.Fill(dt);
            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod]
        public static string GetCustomerByID(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Customers WHERE CustomerID = @ID";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn); da.SelectCommand.Parameters.AddWithValue("@ID", id); da.Fill(dt);
            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod]
        public static string SaveCustomer(CustomerObj obj)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = (obj.ID == "0") ?
                    @"INSERT INTO Customers (CustomerName, CompanyName, Application, SystemCount, Phone, Email, City, Address, OpeningBalance) 
                      VALUES (@Name, @Comp, @App, @Sys, @Phone, @Email, @City, @Addr, @Bal)" :
                    @"UPDATE Customers SET CustomerName=@Name, CompanyName=@Comp, Application=@App, SystemCount=@Sys, 
                      Phone=@Phone, Email=@Email, City=@City, Address=@Addr, OpeningBalance=@Bal WHERE CustomerID=@ID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", obj.ID);
                cmd.Parameters.AddWithValue("@Name", obj.Name);
                cmd.Parameters.AddWithValue("@Comp", obj.Company);
                cmd.Parameters.AddWithValue("@App", obj.App);
                cmd.Parameters.AddWithValue("@Sys", obj.SysCount);
                cmd.Parameters.AddWithValue("@Phone", obj.Phone);
                cmd.Parameters.AddWithValue("@Email", obj.Email);
                cmd.Parameters.AddWithValue("@City", obj.City);
                cmd.Parameters.AddWithValue("@Addr", obj.Address);
                cmd.Parameters.AddWithValue("@Bal", obj.Bal);
                conn.Open(); cmd.ExecuteNonQuery();
            }
            return "Success";
        }

        public class CustomerObj
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string App { get; set; }
            public int SysCount { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string City { get; set; }
            public string Address { get; set; }
            public decimal Bal { get; set; }
        }
    }
}