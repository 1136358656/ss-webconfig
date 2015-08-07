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

namespace Shadowsocks.View
{
    public partial class FormChangePasswdDlg : Form
    {
        public FormChangePasswdDlg()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public string GetMd5Hash(MD5 md5Hash, string input)
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
        private string ChangePasswd(string Username, string OldPasswd, string NewPasswd)
        {
            MD5 Hash = MD5.Create();
            Username = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(Username));
            OldPasswd = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(GetMd5Hash(Hash,System.Convert.ToBase64String(Encoding.ASCII.GetBytes(oldpasswd.Text)))));
            NewPasswd = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(GetMd5Hash(Hash, System.Convert.ToBase64String(Encoding.ASCII.GetBytes(newpasswd.Text)))));
            WebClient Change = new WebClient();
            string Url = "https://dev.unwall.org/api/Client-Change-Passwd.php";
            Change.Credentials = CredentialCache.DefaultCredentials;
            Change.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            Change.Headers.Add("User-Agent", "Shadowsocks-Kaguya");
            string Par = "username="+Username+"&oldpassword="+OldPasswd+"&newpassword="+NewPasswd;
            string Response="";
            try
            {
                Response = Encoding.ASCII.GetString(Change.UploadData(Url, Encoding.ASCII.GetBytes(Par)));
                
            }
            catch
            {
                throw;
            }
            return Response;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (username.Text == "")
            {
                MessageBox.Show("用户名不能为空");
            }
            else
            {
                if (oldpasswd.Text == "")
                {
                    MessageBox.Show("原密码不能为空");
                }
                else
                {
                    if (newpasswd.Text == "")
                    {
                        MessageBox.Show("新密码不能为空");
                    }
                    else
                    {
                        if (newpasswd.Text != confirmnewpasswd.Text)
                        {
                            MessageBox.Show("两次输入的新密码不一致");
                        }
                        else
                        {
                            string Result = ChangePasswd(username.Text, oldpasswd.Text, newpasswd.Text);
                            switch (Result)
                            {
                                case "SUCCESS":
                                    MessageBox.Show("修改密码成功");
                                    this.Hide();
                                    break;
                                case "FAIL_DATABASE_ERROR":
                                    MessageBox.Show("数据库错误");
                                    break;
                                case "FAIL_WRONG_PASSWORD":
                                    MessageBox.Show("原密码错误，请重试");
                                    break;
                                case "FAIL_WRONG_USERNAME":
                                    MessageBox.Show("该用户名不存在");
                                    break;
                                default:
                                    MessageBox.Show("未知错误");
                                    break;
                            }

                        }
                    }
                }
            }
        }
    }
}
