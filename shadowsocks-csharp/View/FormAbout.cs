using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
namespace Shadowsocks.View
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            string s = key.GetValue("").ToString();
            String browserpath = "https://fuckgfw.yanlei.me";
            if (s.StartsWith("\""))
            {
                browserpath = s.Substring(1, s.IndexOf('\"', 1) - 1);
            }
            else
            {
                browserpath = s.Substring(0, s.IndexOf(" "));
            }
            System.Diagnostics.Process.Start(browserpath, "https://fuckgfw.yanlei.me");
        }
    }
}
