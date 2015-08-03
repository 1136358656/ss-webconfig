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
        Config UserConfig = new Config();
        AuthInfo UserStatus = new AuthInfo();
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
        bool login(string username, string hash)
        {
            byte[] usernameData;
            usernameData = Encoding.ASCII.GetBytes(username);
            username = System.Convert.ToBase64String(usernameData);
            byte[] passwdData;
            MD5 md5Hash = MD5.Create();
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
                    return true;
                }
                catch
                {
                    return false;
                }



            }
            catch (System.Net.WebException e)
            {
                throw;
            }



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
        public Config(string ConfigFile)
        {
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
            this = JsonConvert.DeserializeObject<Config>(configJson2);
        }

    }
}
