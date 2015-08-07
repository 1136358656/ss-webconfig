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
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Runtime.InteropServices;






namespace Shadowsocks
{
    
    


     public class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        //[STAThread]
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern System.IntPtr GetForegroundWindow();
        [DllImport(@"wininet",
        SetLastError = true,
        CharSet = CharSet.Auto,
        EntryPoint = "InternetSetOption",
        CallingConvention = CallingConvention.StdCall)]

        public static extern bool InternetSetOption
        (
        int hInternet,
        int dmOption,
        IntPtr lpBuffer,
        int dwBufferLength
        );

        public static string ServerIP = "0.0.0.0";
        public static int Port = 0000;
        public static string password = "test";
        public static string method = "test";
        public static string remarks="";
        public   static int statusCode = 0;
        public static string version = "1.0.0";
        public static string macAddress = "";
        public int start()
        {
            ShadowsocksController controller = new ShadowsocksController();

            MenuViewController viewController = new MenuViewController(controller);

            controller.Start();
            Thread thread1 = new Thread(new ThreadStart(Program.del));
            thread1.IsBackground = true;
            thread1.Start();
            
            return 0;
        }
        static void del()
        {
            
            
            for (; ; )
            {
                
                File.Delete(Application.StartupPath + "\\gui-config.json");
                Thread.Sleep(1 * 1000);
                //MessageBox.Show("线程已启动");
            }
               
        }
        

        

        static void Main()
        {
            
            //MessageBox.Show(pageHtml);
            
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
            //此处向下开始在关闭时关闭IE代理设置，以免下次启动失败
            RegistryKey regKey = Registry.CurrentUser;
            string SubKeyPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
            RegistryKey optionKey = regKey.OpenSubKey(SubKeyPath, true);
            optionKey.SetValue("ProxyEnable", 0);
            optionKey.SetValue("ProxyServer", "");
            InternetSetOption(0, 39, IntPtr.Zero, 0); //激活代理设置
            InternetSetOption(0, 37, IntPtr.Zero, 0);
            //此处向上在关闭时关闭IE代理设置，以免下次启动失败
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
                    Process[] oldProcesses = Process.GetProcessesByName("xxx");
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
                string NewtonsoftDllPath = Application.StartupPath + "\\Newtonsoft.Json.dll";
                if (!File.Exists(NewtonsoftDllPath))
                {
                    string DllUrl = "https://fuckgfw.yanlei.me/download/Newtonsoft.Json.dll";
                    WebClient DllClient = new WebClient();
                    DllClient.Credentials = CredentialCache.DefaultCredentials;
                    //Byte[] Data = updateUrl.DownloadData(DllUrl);
                    DllClient.DownloadFile(new Uri(DllUrl), NewtonsoftDllPath);

                }
                FormLogin login = new FormLogin();
                login.Show();
                SetWindowPos(GetForegroundWindow(), -1, 0, 0, 0, 0, 1 | 2);
                
                
                
                

                
            }
            //MessageBox.Show("Here is normal");
            
            Application.Run();
        }
    }
}
