using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;


namespace updater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void DownloadFile(string url, string savefile, Action<int> downloadProgressChanged, Action downloadFileCompleted)
        {
            WebClient client = new WebClient();
            if (downloadProgressChanged != null)
            {
                client.DownloadProgressChanged += delegate(object sender, DownloadProgressChangedEventArgs e)
                {
                    this.Invoke(downloadProgressChanged, e.ProgressPercentage);
                };
            }
            if (downloadFileCompleted != null)
            {
                client.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e)
                {
                    this.Invoke(downloadFileCompleted);
                };
            }
            client.DownloadFileAsync(new Uri(url), savefile);
        }
        delegate void Action();
        private void ProgressBar_Value(int val)
        {
            if (progressBar1.Value == 100)
            {
                MessageBox.Show("更新已完成");
                string path = System.Windows.Forms.Application.StartupPath;
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = @path + "\\Shadowsocks.exe";
                info.Arguments = "";
                info.WindowStyle = ProcessWindowStyle.Normal;
                Process pro = Process.Start(info);
                //pro.WaitForExit();
                System.Environment.Exit(0);
            }
            progressBar1.Value = val;
            label1.Text = val.ToString() + "%";

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = System.Windows.Forms.Application.StartupPath;
            //MessageBox.Show(path + "\\Shadowsocks.exe");
            string url = "https://fuckgfw.yanlei.me/update.php";
            WebClient updateUrl = new WebClient();
            updateUrl.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = updateUrl.DownloadData(url);
            string version = Encoding.UTF8.GetString(pageData);
            //MessageBox.Show(version);
            label4.Text = version;
            try
            {
                if (File.Exists(path + "\\Shadowsocks.exe"))
                {
                    File.Delete(path + "\\Shadowsocks.exe");
                }
            }
            catch(System.UnauthorizedAccessException ) 
            {
                MessageBox.Show("请关闭正在运行的客户端后再进行更新");
                System.Environment.Exit(0);
            }
            
            string fileUrl = "https://fuckgfw.yanlei.me/download/Update/" + version + "/Shadowsocks.exe";
            DownloadFile(fileUrl, @path + "\\Shadowsocks.exe",ProgressBar_Value, null);

            

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
