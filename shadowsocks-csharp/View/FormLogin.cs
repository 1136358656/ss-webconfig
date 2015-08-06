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
        
        UserAccount User = new UserAccount();
        string path = Application.StartupPath + "\\config.json";

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            User.LoadConfig(path);
            
            if (User.UserConfig.noticed == "0")
            {
                
            }
            if (User.UserConfig.rememberUsername == "0")
            {
                checkBox1.Checked = false;
            }
            else
            {
                username.Text =Encoding.ASCII.GetString(System.Convert.FromBase64String(User.UserConfig.username));
                checkBox1.Checked = true;
            }
            
            if (User.UserConfig.rememberPasswd == "0")
            {
                checkBox2.Checked = false;
            }
            else
            {
                
                passwd.Text = "UseSavedPasswd!";
                checkBox2.Checked = true;
                
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormInputDlg InputDlg = new FormInputDlg();
            InputDlg.ShowDialog();
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
                    byte[] tempData;
                    tempData = Encoding.ASCII.GetBytes(username.Text);
                    
                    User.UserConfig.username = System.Convert.ToBase64String(tempData);
                    if (passwd.Text != "UseSavedPasswd!")
                    {
                        MD5 md5Hash = MD5.Create();
                        tempData = Encoding.ASCII.GetBytes(passwd.Text);
                        User.UserConfig.passwd = User.GetMd5Hash(md5Hash, System.Convert.ToBase64String(tempData));
                    }
                    
                    User.login(User.UserConfig.username, User.UserConfig.passwd);
                    ServerConfig ServerCfg = new ServerConfig();
                    switch (User.UserStatus.status)
                    {
                        case "0":
                            this.Text = "登录成功！";
                            Program prog = new Program();
                            bool ConfigResult = ServerCfg.Get(Application.StartupPath + "\\gui-config.json", Program.version, Program.macAddress);
                            if (ConfigResult == false)
                            {
                                MessageBox.Show("无法连接服务器获取配置，请检查您的网络连接！\r\n1、网络连接是否正常\r\n2、DNS设置是否正常\r\n3、IE代理是否正常(请取消一切代理选项)");
                                Environment.Exit(0);
                            }
                            int a = prog.start();
                            User.UserConfig.SaveConfig();
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
            
            System.Environment.Exit(0);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                User.UserConfig.rememberUsername = "0";
                User.UserConfig.username = "";
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    User.UserConfig.rememberUsername = "1";
                    byte[] usernameData;
                    usernameData = Encoding.ASCII.GetBytes(username.Text);
                    User.UserConfig.username = System.Convert.ToBase64String(usernameData);
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
            {
                User.UserConfig.rememberPasswd = "0";
                User.UserConfig.passwd = "";
            }
            else
            {
                if (checkBox2.Checked == true)
                {
                    checkBox1.Checked = true;
                    byte[] passwdData;
                    User.UserConfig.rememberPasswd = "1";
                    MD5 md5Hash = MD5.Create();
                    if (passwd.Text == "UseSavedPasswd!")
                    {

                    }
                    else
                    {
                        passwdData = Encoding.ASCII.GetBytes(passwd.Text);
                        User.UserConfig.passwd = User.GetMd5Hash(md5Hash, System.Convert.ToBase64String(passwdData));
                    } 
                }
            }
        }

        private void passwd_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormChangePasswdDlg ChangePasswdDlg = new FormChangePasswdDlg();
            ChangePasswdDlg.ShowDialog();
        }
    }
    
   
}
