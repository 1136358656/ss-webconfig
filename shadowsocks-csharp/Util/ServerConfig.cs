using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Shadowsocks.Util
{
    class ServerConfig
    {
        public bool Get(string ConfigPath,string Version,string MacAddress)
        {
            WebClient web1 = new WebClient();
            web1.Credentials = CredentialCache.DefaultCredentials;
            web1.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            string par = "ver=" + Version + "&mac=" + MacAddress;
            //MessageBox.Show(par);
            byte[] postData = Encoding.UTF8.GetBytes(par);
            string url = "https://fuckgfw.yanlei.me/hasi-get-config.php";
            try
            {
                //string key = "e537bfa04fef8b9e6b29e66a61620ef6";
                byte[] responseData = web1.UploadData(url, "POST", postData);
                string response = Encoding.UTF8.GetString(responseData);
                string configJson = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(response));
                File.Delete(ConfigPath);
                    using (StreamWriter sw = File.CreateText(ConfigPath))
                    {
                        sw.WriteLine(configJson);
                    }
                    return true;
            }
            catch (System.Net.WebException e)
            {
                return false;
            }
            return true;
        }
    }
}
