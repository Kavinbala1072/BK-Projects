using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Web.Services;
using Newtonsoft.Json;

namespace BKSoftwares
{
    public partial class CustomerList : System.Web.UI.Page
    {
        [WebMethod]
        public static string GetUsageReport()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                string jsonUrl = "https://raw.githubusercontent.com/Kavinbala1072/Reporting/refs/heads/main/BK%20Reporting.json";
                string rawJson = "";

                using (WebClient client = new WebClient())
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0;)");
                    rawJson = client.DownloadString(jsonUrl);
                }

                var jsonList = JsonConvert.DeserializeObject<List<UsageInfo>>(rawJson);

                DataTable dtCust = new DataTable();
                string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = @"SELECT CustCode, UPPER(REPLACE(Application, ' ', '')) as MatchApp, SystemCount FROM Customers";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    da.Fill(dtCust);
                }

                foreach (var item in jsonList)
                {
                    string jsonID = (item.CompNo ?? "").Trim();
                    string jsonApp = (item.Application ?? "").Replace(" ", "").ToUpper();

                    if (!string.IsNullOrEmpty(jsonID))
                    {
                        DataRow[] foundRows = dtCust.Select("CustCode = '" + jsonID + "' AND MatchApp = '" + jsonApp + "'");

                        if (foundRows.Length > 0)
                        {
                            item.SystemCount = foundRows[0]["SystemCount"].ToString();
                            item.MatchStatus = "Verified";
                        }
                        else
                        {
                            item.SystemCount = "0";
                            item.MatchStatus = "Unregistered";
                        }
                    }
                    else
                    {
                        item.MatchStatus = "No ID";
                    }

                    item.DisplayID = string.IsNullOrEmpty(jsonID) ? "N/A" : jsonID;
                }

                return JsonConvert.SerializeObject(jsonList);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }

        public class UsageInfo
        {
            public string server { get; set; }
            public string CompNo { get; set; }
            public string Application { get; set; }
            public string Version { get; set; }
            public string NewVersion { get; set; }

            [JsonProperty("Company name")]
            public string CompanyName { get; set; }

            [JsonProperty("AMC expiry")]
            public string AMCExpiry { get; set; }

            public string secretPassword { get; set; }
            public string lastlogin { get; set; }
            public string DisplayID { get; set; }
            public string SystemCount { get; set; }
            public string MatchStatus { get; set; }
        }
    }
}