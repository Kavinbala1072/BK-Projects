using System;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace WinFormsInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBoxApps.Items.Add("Godown Stock");
            comboBoxApps.Items.Add("Attma");
            comboBoxApps.SelectedIndex = 0;
        }

        private void btnInstall_Click_1(object sender, EventArgs e)
        {
            string selectedApp = comboBoxApps.SelectedItem.ToString();
            string url = "";
            string internalFolderName = "";

            if (selectedApp == "Godown Stock")
            {
                url = "https://github.com/Kavinbala1072/GodownStock/archive/refs/heads/main.zip";
                internalFolderName = "GodownStock-main";
            }
            else if (selectedApp == "Attma")
            {
                url = "https://github.com/Kavinbala1072/Attma/archive/refs/heads/main.zip";
                internalFolderName = "Attma-main";
            }

            string targetDir = Path.Combine(@"C:\MyInstalledFiles", selectedApp.Replace(" ", ""));
            string zipPath = Path.Combine(targetDir, "download.zip");
            string extractPath = Path.Combine(targetDir, "Extracted");

            try
            {
                if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

                using (WebClient client = new WebClient())
                {
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    btnInstall.Enabled = false;
                    lblStatus.Text = $"Downloading {selectedApp}...";

                    client.DownloadFile(url, zipPath);
                }

                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }
                Directory.CreateDirectory(extractPath);

                ZipFile.ExtractToDirectory(zipPath, extractPath);

                string appFolder = Path.Combine(extractPath, internalFolderName);

                string[] exeFiles = Directory.GetFiles(appFolder, "*.exe", SearchOption.AllDirectories);

                if (exeFiles.Length > 0)
                {
                    string appExe = exeFiles[0];

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string shortcutLocation = Path.Combine(desktopPath, selectedApp + ".lnk");

                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
                    shortcut.Description = $"Launch {selectedApp}";
                    shortcut.TargetPath = appExe;
                    shortcut.WorkingDirectory = Path.GetDirectoryName(appExe);
                    shortcut.Save();

                    MessageBox.Show($"{selectedApp} installation complete! Shortcut created on Desktop.", "Success");
                }
                else
                {
                    MessageBox.Show("No executable found in extracted folder. Please check the repository structure.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                progressBar1.Style = ProgressBarStyle.Blocks;
                btnInstall.Enabled = true;
            }
        }
    }
}