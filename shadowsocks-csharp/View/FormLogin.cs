using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Shadowsocks.View
{
   
    public partial class FormLogin : Form
    {
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        private string[] login(string username, string passwd)
        {
            byte[] usernameData;
            usernameData = Encoding.ASCII.GetBytes(username);
            username = System.Convert.ToBase64String(usernameData);
            byte[] passwdData;
            passwdData = Encoding.ASCII.GetBytes(passwd);
            passwd = System.Convert.ToBase64String(passwdData);
            MD5 md5Hash = MD5.Create();
            string hash = GetMd5Hash(md5Hash,passwd);
            WebClient authority = new WebClient();
            authority.Credentials = CredentialCache.DefaultCredentials;
            authority.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            string par = "username=" + username+"&passwd="+hash;
            byte[] postData = Encoding.UTF8.GetBytes(par);
            string url = "https://fuckgfw.yanlei.me/api/userverify.php";
            try
            {
                byte[] responseData = authority.UploadData(url, "POST", postData);
                string response = Encoding.UTF8.GetString(responseData);
                string authJson = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(response));
                AuthInfo auth = JsonConvert.DeserializeObject<AuthInfo>(authJson);
                string[] result = {auth.status,auth.username};
                return result;

            }catch (System.Net.WebException e)
                {
                    MessageBox.Show("登录失败，因为无法连接到服务器，请检查您的网络连接");
                    string[] result = {"-1","NULL"};
                    return result;
                }

            
            
        }

   

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] auth = login(username.Text,passwd.Text);
            switch (auth[0])
                {
                case "0":
                    this.Text = "登录成功！";
                    Program prog = new Program();
                    int a = prog.start();
                    this.Hide();
                        break;
                case "1":
                        MessageBox.Show("登录失败，请检查您的用户名和密码！");
                        break;
                case "-1":
                        
                        break;
                default:
                        MessageBox.Show("登录失败，未知错误");
                        break;
                }
        }
    }
    public class AuthInfo
    {
        public string status;
        public string username;
    }
   
}
