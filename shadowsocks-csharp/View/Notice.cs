using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using Shadowsocks.Util;

namespace Shadowsocks.View
{
    public partial class Notice : Form
    {
        Config newConfig = new Config();
        string configJson;
        string path = Application.StartupPath + "\\config.json";
        public Notice()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fs = File.Open(path, FileMode.Open);
            byte[] configData = new byte[1024];
            fs.Read(configData, 0, configData.Length);
            fs.Close();
            UTF8Encoding temp = new UTF8Encoding(true);
            configJson = temp.GetString(configData);
            newConfig = JsonConvert.DeserializeObject<Config>(configJson);
            newConfig.noticed = "1";
            configJson = JsonConvert.SerializeObject(newConfig);
            //MessageBox.Show(configJson);
            using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\config.json"))
            {
                sw.WriteLine(configJson);
            }
            this.Close();
        }
    }
}
