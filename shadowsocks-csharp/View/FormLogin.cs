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
using Shadowsocks.Util;

namespace Shadowsocks.View
{
   
    public partial class FormLogin : Form
    {
        
        Config newConfig = new Config();
        string configJson;
        bool passwordTyped = false;
        bool usernameTyped = false;
        string path = Application.StartupPath + "\\config.json";

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            //config newConfig = new config();
            string configJson;
            
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            
            string user = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(newConfig.username));
            password = newConfig.passwd;
            fs.Close();
            if (newConfig.noticed == "0")
            {
                Form notice = new Notice();
                notice.ShowDialog();
                newConfig.noticed = "1";
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
                passwordTyped = false;
                rememberPasswd = true;
            }
            


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] auth = {"NULL","NULL"};
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
                        auth = login(username.Text, password, false);
                    }
                    else
                    {
                        if (passwordTyped == true)
                        {
                            password = passwd.Text;
                            auth = login(username.Text, password, false);
                        }
                        else
                        {
                            password = newConfig.passwd;
                            MessageBox.Show(password);
                            auth = login(username.Text, password, true);
                        }
                        
                    }
                    //MessageBox.Show(password);
                    
                    
                    switch (auth[0])
                    {
                        case "0":
                            this.Text = "登录成功！";
                            Program prog = new Program();
                            WebClient web1 = new WebClient();
                            web1.Credentials = CredentialCache.DefaultCredentials;
                            web1.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                            string par = "ver=" + Program.version + "&mac=" + Program.macAddress;
                            //MessageBox.Show(par);
                            byte[] postData = Encoding.UTF8.GetBytes(par);
                            string url = "https://fuckgfw.yanlei.me/hasi-get-config.php";
                            try
                            {
                                //string key = "e537bfa04fef8b9e6b29e66a61620ef6";
                                byte[] responseData = web1.UploadData(url, "POST", postData);
                                string response =Encoding.UTF8.GetString(responseData);
                                string configJson = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(response));
                    
                                if (!File.Exists(Application.StartupPath + "\\gui-config.json"))
                                {
                                    using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\gui-config.json"))
                                        {
                                            sw.WriteLine(configJson);
                                            }
                                }
                            }
                            catch (System.Net.WebException e2)
                            {
                                MessageBox.Show("无法获取服务器配置，请检查您的网络连接！");
                                System.Environment.Exit(0);
                    
                             }
                            int a = prog.start();

                            //MessageBox.Show("test!");
                            if (!File.Exists(path))
                            {
                                newConfig.rememberUsername = "0";
                                newConfig.rememberPasswd = "0";
                                newConfig.passwd = "";
                                newConfig.username = "";
                            }
                            byte[] usernameData2;
                            usernameData2 = Encoding.ASCII.GetBytes(username.Text);
                            newConfig.username = System.Convert.ToBase64String(usernameData2);
                            if (passwordTyped == true && rememberPasswd == true)
                            {
                                byte[] passwdData;
                                MD5 md5Hash = MD5.Create();
                                passwdData = Encoding.ASCII.GetBytes(passwd.Text);
                                newConfig.passwd = GetMd5Hash(md5Hash, System.Convert.ToBase64String(passwdData));
                            }
                            string configJson2 = JsonConvert.SerializeObject(newConfig);
                            //MessageBox.Show(configJson);
                            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\config.json"))
                            {
                                sw.WriteLine(configJson2);
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
            byte[] usernameData2;
            usernameData2 = Encoding.ASCII.GetBytes(username.Text);
            newConfig.username = System.Convert.ToBase64String(usernameData2);
            if (passwordTyped == true && rememberPasswd == true)
            {
                byte[] passwdData;
                MD5 md5Hash = MD5.Create();
                passwdData = Encoding.ASCII.GetBytes(passwd.Text);
                newConfig.passwd = GetMd5Hash(md5Hash, System.Convert.ToBase64String(passwdData));
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
            MessageBox.Show(newConfig.passwd);
            if (checkBox2.Checked == false)
            {
                newConfig.rememberPasswd = "0";
                newConfig.passwd = "";
                rememberPasswd = false;
            }
            else
            {
                if (checkBox2.Checked == true)
                {
                    if (passwordTyped == true)
                    {
                        rememberPasswd = false;
                    }
                    else
                    {
                        rememberPasswd = true;
                    }
                    byte[] passwdData;
                    newConfig.rememberPasswd = "1";
                    MD5 md5Hash = MD5.Create();
                    newConfig.rememberUsername = "1";
                    passwdData = Encoding.ASCII.GetBytes(passwd.Text);
                    newConfig.passwd = GetMd5Hash(md5Hash, System.Convert.ToBase64String(passwdData));
                    
                }
            }
        }

        private void passwd_TextChanged(object sender, EventArgs e)
        {
            passwordTyped = true;
        }
    }
    
   
}
