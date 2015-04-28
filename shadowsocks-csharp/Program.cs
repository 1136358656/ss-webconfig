using Shadowsocks.Controller;
using Shadowsocks.Properties;
using Shadowsocks.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Globalization;
using System.Runtime.InteropServices;







namespace Shadowsocks
{
    
    


    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        //[STAThread]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern System.IntPtr GetForegroundWindow();
       

        public static string ServerIP = "0.0.0.0";
        public static int Port = 0000;
        public static string password = "test";
        public static string method = "test";
        public static string remarks="";
        public   static int statusCode = 0;

        static void del()
        {
            
            
            for (; ; )
            {
                
                File.Delete(Application.StartupPath + "\\gui-config.json");
                Thread.Sleep(1 * 1000);
                //MessageBox.Show("线程已启动");
            }
               
        }
        static string RC4(string input, string key)
        {
            StringBuilder result = new StringBuilder();
            int x, y, j = 0;
            int[] box = new int[256];

            for (int i = 0; i < 256; i++)
            {
                box[i] = i;
            }

            for (int i = 0; i < 256; i++)
            {
                j = (key[i % key.Length] + box[i] + j) % 256;
                x = box[i];
                box[i] = box[j];
                box[j] = x;
            }

            for (int i = 0; i < input.Length; i++)
            {
                y = i % 256;
                j = (box[y] + j) % 256;
                x = box[y];
                box[y] = box[j];
                box[j] = x;

                result.Append((char)(input[i] ^ box[(box[y] + box[j]) % 256]));
            }
            return result.ToString();
        }

        static void Main()
        {
            
            //MessageBox.Show(pageHtml);
            string version = "0.9.5";
            string macAddress = "";
            //int statusCode = 0;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (!adapter.GetPhysicalAddress().ToString().Equals(""))
                    {
                        macAddress = adapter.GetPhysicalAddress().ToString();
                        for (int i = 1; i < 6; i++)
                        {
                            macAddress = macAddress.Insert(3 * i - 1, ":");
                        }
                        break;
                    }
                }

            }
            catch
            {
            }
            //MessageBox.Show(macAddress);
            //System.Net.WebException
            string infoUrl = "https://fuckgfw.yanlei.me/userlog.php?mac=" +macAddress+ "&version="+version;
            WebClient info = new WebClient();
            info.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                Byte[] rawInfo = info.DownloadData(infoUrl);
            }
            catch (System.Net.WebException e)
            {
                MessageBox.Show("无法连接服务器，请检查您的网络连接！");
                System.Environment.Exit(0);
                //Application.ApplicationExit();
            }

            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                string url = "https://fuckgfw.yanlei.me/update.php";
                WebClient updateUrl = new WebClient();
                updateUrl.Credentials = CredentialCache.DefaultCredentials;
                Byte[] Data = updateUrl.DownloadData(url);
                string newVersion = Encoding.UTF8.GetString(Data);
                if (version != newVersion)
                {
                    
                    MessageBox.Show("发现新版本 V" + newVersion + "，将自动更新");
                    try
                    {
                        string path = System.Windows.Forms.Application.StartupPath;
                        ProcessStartInfo info2 = new ProcessStartInfo();
                        info2.FileName = @path + "\\updater.exe";
                        info2.Arguments = "";
                        info2.WindowStyle = ProcessWindowStyle.Normal;
                        Process pro = Process.Start(info2);
                    }
                    catch (System.ComponentModel.Win32Exception e) 
                    {
                        Program.statusCode = 1;
                        MessageBox.Show("更新器缺失，无法完成自动更新，\n将影响正常使用，请到官网下载完整版本");
                        //Form1 form = new Form1();
                        //form.Show();

                    }
                    
                    //pro.WaitForExit();
                    System.Environment.Exit(0);
                }
            }
            catch (System.Net.WebException e)
            {
                MessageBox.Show("无法检查更新，请检查您的网络连接！");
                System.Environment.Exit(0);
                //Application.ApplicationExit();
            }

            Util.Utils.ReleaseMemory();
            using (Mutex mutex = new Mutex(false, "Global\\" + "71981632-A427-497F-AB91-241CD227EC1F"))
            {
                Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);

                if (!mutex.WaitOne(0, false))
                {
                    Process[] oldProcesses = Process.GetProcessesByName("Shadowsocks");
                    if (oldProcesses.Length > 0)
                    {
                        Process oldProcess = oldProcesses[0];
                    }
                    MessageBox.Show("Shadowsocks正在运行.");
                    return;
                }
                Directory.SetCurrentDirectory(Application.StartupPath);
#if !DEBUG
                Logging.OpenLogFile();
#endif
                WebClient web1 = new WebClient();
                web1.Credentials = CredentialCache.DefaultCredentials;
                web1.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string par = "ver=" + version + "&mac=" + macAddress;
                //MessageBox.Show(par);
                byte[] postData = Encoding.UTF8.GetBytes(par);
                string url = "https://fuckgfw.yanlei.me/hasi-get-config.php";
                try
                {
                    string key = "e537bfa04fef8b9e6b29e66a61620ef6";
                    byte[] responseData = web1.UploadData(url, "POST", postData);
                    string response =Encoding.UTF8.GetString(responseData);
                    string configJson = System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(response));
                    //MessageBox.Show(configJson);//得到返回字符流  
                    
                    //MessageBox.Show(orgStr);
                    
                    if (!File.Exists(Application.StartupPath + "\\gui-config.json"))
                    {
                        using (StreamWriter sw = File.CreateText(Application.StartupPath + "\\gui-config.json"))
                        {
                            sw.WriteLine(configJson);
                        }
                    }
                }
                catch (System.Net.WebException e)
                {
                    MessageBox.Show("无法获取服务器配置，请检查您的网络连接！");
                    System.Environment.Exit(0);
                    //Application.ApplicationExit();
                }
                FormLogin login = new FormLogin();
                login.Show();
                SetWindowPos(GetForegroundWindow(), -1, 0, 0, 0, 0, 1 | 2);
                


                ShadowsocksController controller = new ShadowsocksController();

                MenuViewController viewController = new MenuViewController(controller);

                controller.Start();
                Thread thread1 = new Thread(new ThreadStart(Program.del ));
                thread1.IsBackground = true;
                thread1.Start();
                Application.Run();
            }
        }
    }
}
