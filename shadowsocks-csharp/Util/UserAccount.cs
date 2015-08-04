using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Shadowsocks.Util
{
    class UserAccount
    {
        public Config UserConfig = new Config();
        public AuthInfo UserStatus = new AuthInfo();
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
        public void login(string username, string hash)//username为用户名base64,hash为密码base64以后再md5
        {
            byte[] tempData;
            tempData = Encoding.ASCII.GetBytes(hash);
            hash = System.Convert.ToBase64String(tempData);
            WebClient authority = new WebClient();
            authority.Credentials = CredentialCache.DefaultCredentials;
            authority.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            string par = "username=" + username + "&passwd=" + hash;
            byte[] postData = Encoding.UTF8.GetBytes(par);
            string url = "https://fuckgfw.yanlei.me/api/userverify.php";
            try
            {
                byte[] responseData = authority.UploadData(url, "POST", postData);
                string response = Encoding.UTF8.GetString(responseData);
                try
                {
                    string authJson = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(response));
                    UserStatus = JsonConvert.DeserializeObject<AuthInfo>(authJson);
                    
                }
                catch
                {
                    throw;
                }



            }
            catch (System.Net.WebException e)
            {
                throw;
            }



        }
        public void LoadConfig(string ConfigFile)
        {
            UserConfig.Load(ConfigFile);
        }
    }
    class AuthInfo
    {
        public string status = "";
        public string username = "";
    }
    class Config
    {
        public string rememberUsername = "";
        public string rememberPasswd = "";
        public string username = "";
        public string passwd = "";
        public string noticed = "";
        private string ConfigFile = "";
        public void Load(string ConfigFile)
        {
            this.ConfigFile = ConfigFile;
            if (!File.Exists(ConfigFile))
            {
                this.rememberUsername = "0";
                this.rememberPasswd = "0";
                this.passwd = "";
                this.username = "";
                this.noticed = "0";
                string configJson = JsonConvert.SerializeObject(this);
                using (StreamWriter sw = File.CreateText(ConfigFile))
                {
                    sw.WriteLine(configJson);
                    sw.Close();
                }
            }
            FileStream fs = File.Open(ConfigFile, FileMode.Open);
            byte[] configData = new byte[1024];
            fs.Read(configData, 0, configData.Length);
            UTF8Encoding temp = new UTF8Encoding(true);
            string configJson2 = temp.GetString(configData);
            TempConfig temp1 = JsonConvert.DeserializeObject<TempConfig>(configJson2);
            this.passwd = temp1.passwd;
            this.noticed = temp1.noticed;
            this.rememberPasswd = temp1.rememberPasswd;
            this.rememberUsername = temp1.rememberUsername;
            this.username = temp1.username;

        }
        public Config()
        {
            this.rememberUsername = "0";
            this.rememberPasswd = "0";
            this.passwd = "";
            this.username = "";
            this.noticed = "0";
        }
        public void SaveConfig()
        {
            TempConfig temp1 = new TempConfig();
            temp1.passwd = this.passwd;
            temp1.noticed = this.noticed;
            temp1.rememberPasswd = this.rememberPasswd;
            temp1.rememberUsername = this.rememberUsername;
            temp1.username = this.username;
            string configJson = JsonConvert.SerializeObject(temp1);
            using (StreamWriter sw = File.CreateText(this.ConfigFile))
            {
                sw.WriteLine(configJson);
                sw.Close();
            }
        }
        ~Config()
        {
            //保存配置文件等等善后操作
            TempConfig temp1 = new TempConfig();
            temp1.passwd = this.passwd;
            temp1.noticed = this.noticed;
            temp1.rememberPasswd = this.rememberPasswd;
            temp1.rememberUsername = this.rememberUsername;
            temp1.username = this.username;
            string configJson = JsonConvert.SerializeObject(temp1);
            using (StreamWriter sw = File.CreateText(this.ConfigFile))
            {
                sw.WriteLine(configJson);
                sw.Close();
            }
        }

    }
    class TempConfig
    {
        public string rememberUsername = "";
        public string rememberPasswd = "";
        public string username = "";
        public string passwd = "";
        public string noticed = "";
    }
}
