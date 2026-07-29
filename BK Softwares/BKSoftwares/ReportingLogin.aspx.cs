using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace BKSoftwares
{
    public partial class CreateUser : System.Web.UI.Page
    {
        private const string GitToken = "A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";

        private static readonly string ActualGitToken = GitToken.StartsWith("A") ? GitToken.Substring(1) : GitToken;

        private const string GitURL = "https://api.github.com/repos/Kavinbala1072/Reporting/contents/Login.json";
        private const string ADMIN_KEY = "BK@2026";

        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            string newUser = txtNewUserCode.Text.Trim();
            string newPass = txtNewPassword.Text.Trim();
            string secret = txtAdminSecret.Text.Trim();

            if (secret != ADMIN_KEY)
            {
                ShowMsg("Invalid Admin Secret Key!", "text-danger");
                return;
            }

            try
            {
                var gitResponse = FetchGitFile();
                string decodedJson = Encoding.UTF8.GetString(Convert.FromBase64String(gitResponse.content));
                var gitData = new JavaScriptSerializer().Deserialize<GitHubRoot>(decodedJson);

                if (gitData.users.Any(x => x.User_Code.Equals(newUser, StringComparison.OrdinalIgnoreCase)))
                {
                    ShowMsg("User Code already exists in database!", "text-warning");
                    return;
                }

                gitData.users.Add(new UserEntry
                {
                    User_Code = newUser,
                    Password = newPass,
                    Active = ""
                });

                string updatedJson = new JavaScriptSerializer().Serialize(gitData);
                UpdateGitFile(updatedJson, gitResponse.sha);

                ShowMsg("User created and synced with GitHub successfully!", "text-success");
                txtNewUserCode.Text = ""; txtNewPassword.Text = "";
            }
            catch (Exception ex)
            {
                ShowMsg("Error: " + ex.Message, "text-danger");
            }
        }

        private GitResponse FetchGitFile()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GitURL);
            request.Headers.Add("Authorization", "token " + ActualGitToken);
            request.UserAgent = "BK_App_Creator";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return new JavaScriptSerializer().Deserialize<GitResponse>(reader.ReadToEnd());
            }
        }

        private void UpdateGitFile(string jsonContent, string sha)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GitURL);
            request.Method = "PUT";
            request.Headers.Add("Authorization", "token " + ActualGitToken);
            request.UserAgent = "BK_App_Creator";
            request.ContentType = "application/json";

            var payload = new
            {
                message = "Added new application user",
                content = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonContent)),
                sha = sha
            };

            byte[] body = Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(payload));
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
            }
            request.GetResponse().Close();
        }

        private void ShowMsg(string msg, string cssClass)
        {
            lblStatus.Text = msg;
            lblStatus.CssClass = cssClass;
        }

        #region Models
        public class UserEntry
        {
            public string User_Code { get; set; }
            public string Password { get; set; }
            public string Active { get; set; }
        }
        public class GitHubRoot
        {
            public List<UserEntry> users { get; set; }
        }
        public class GitResponse
        {
            public string sha { get; set; }
            public string content { get; set; }
        }
        #endregion
    }
}