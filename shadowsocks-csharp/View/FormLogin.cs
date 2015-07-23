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
using System.IO;

namespace Shadowsocks.View
{
   
    public partial class FormLogin : Form
    {
        bool rememberPasswd;
        string password;
        config newConfig = new config();
        string configJson;
        string path = Application.StartupPath + "\\config.json";
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
        private string[] login(string username, string passwd ,bool passwdStatus)
        {
            byte[] usernameData;
            usernameData = Encoding.ASCII.GetBytes(username);
            username = System.Convert.ToBase64String(usernameData);
            byte[] passwdData;
            MD5 md5Hash = MD5.Create();
            string hash;
            if (passwdStatus == false)
            {
                passwdData = Encoding.ASCII.GetBytes(passwd);
                passwd = System.Convert.ToBase64String(passwdData);
                hash = GetMd5Hash(md5Hash, passwd);
            }
            else
            {
                hash = passwd;
            }
            byte[] tempData;
            tempData = Encoding.ASCII.GetBytes(hash);
            hash = System.Convert.ToBase64String(tempData);
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
                try
                {
                    string authJson = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(response));
                    AuthInfo auth = JsonConvert.DeserializeObject<AuthInfo>(authJson);
                    string[] result = { auth.status, auth.username };
                    //MessageBox.Show("breakpoint1");
                    return result;
                }
                catch
                {
                    string[] result = { "2", "NULL" };
                    //MessageBox.Show("breakpoint2");
                    return result;
                }
                
                

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
            //config newConfig = new config();
            string configJson;
            string path = Application.StartupPath + "\\config.json";
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            if (!File.Exists(path))
            {
                newConfig.rememberUsername = "0";
                newConfig.rememberPasswd = "0";
                newConfig.passwd = "";
                newConfig.username = "";
                newConfig.noticed = "0";
                configJson = JsonConvert.SerializeObject(newConfig);
                //MessageBox.Show(configJson);
                using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\config.json"))
                {
                    sw.WriteLine(configJson);
                    sw.Close();
                }
             }
            FileStream fs = File.Open(path, FileMode.Open);
            byte[] configData = new byte[1024];
            fs.Read(configData,0,configData.Length);
            UTF8Encoding temp = new UTF8Encoding(true);
            configJson = temp.GetString(configData);
            newConfig = JsonConvert.DeserializeObject<config>(configJson);
            string user = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(newConfig.username));
            password = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(newConfig.passwd));
            fs.Close();
            if (newConfig.noticed == "0")
            {
                Form notice = new Notice();
                notice.ShowDialog();
            }
            if(newConfig.rememberUsername == "0")
            {
                checkBox1.Checked = false;
            }
            else { username.Text = user;
            checkBox1.Checked = true;
            }
            if (newConfig.rememberPasswd == "0")
            {
                checkBox2.Checked = false;
                rememberPasswd = false;
            }
            else
            {
                checkBox2.Checked = true;
                passwd.Text = "########";
            }
            


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (username.Text == "")
            {
                MessageBox.Show("请输入用户名！");
            }
            else
            {
                if (passwd.Text == "")
                {
                    MessageBox.Show("请输入密码！");
                }
                else
                {
                    if (rememberPasswd == false)
                    {
                        password = passwd.Text;
                    }
                    string[] auth = login(username.Text, passwd.Text, rememberPasswd);
                    switch (auth[0])
                    {
                        case "0":
                            this.Text = "登录成功！";
                            Program prog = new Program();
                            int a = prog.start();

                            if (!File.Exists(path))
                            {
                                newConfig.rememberUsername = "0";
                                newConfig.rememberPasswd = "0";
                                newConfig.passwd = "";
                                newConfig.username = "";
                            }
                            configJson = JsonConvert.SerializeObject(newConfig);
                            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\config.json"))
                            {
                                sw.WriteLine(configJson);
                            }



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
            
            
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("test!");
            if (!File.Exists(path))
            {
                newConfig.rememberUsername = "0";
                newConfig.rememberPasswd = "0";
                newConfig.passwd = "";
                newConfig.username = "";
            }
            configJson = JsonConvert.SerializeObject(newConfig);
            //MessageBox.Show(configJson);
            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\config.json"))
            {
                sw.WriteLine(configJson);
            }
            System.Environment.Exit(0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                newConfig.rememberUsername = "0";
                newConfig.username ="";
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    newConfig.rememberUsername = "1";
                    byte[] usernameData;
                    usernameData = Encoding.ASCII.GetBytes(username.Text);
                    newConfig.username = System.Convert.ToBase64String(usernameData);
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
            {
                newConfig.rememberPasswd = "0";
                newConfig.passwd = "";
            }
            else
            {
                if (checkBox2.Checked == true)
                {
                    byte[] passwdData;
                    newConfig.rememberPasswd = "1";
                    MD5 md5Hash = MD5.Create();
                    newConfig.rememberUsername = "1";
                    passwdData = Encoding.ASCII.GetBytes(passwd.Text);
                    newConfig.passwd = GetMd5Hash(md5Hash, System.Convert.ToBase64String(passwdData));
                }
            }
        }
    }
    public class AuthInfo
    {
        public string status;
        public string username;
    }
    public class config
    {
        public string rememberUsername;
        public string rememberPasswd;
        public string username;
        public string passwd;
        public string noticed;

    }
   
}
