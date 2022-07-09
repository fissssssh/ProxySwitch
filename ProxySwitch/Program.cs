using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace ProxySwitch
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var mutex = new Mutex(false, "Global\\" + Process.GetCurrentProcess().ProcessName, out var isFirst))
            {
                if (!isFirst)
                {
                    MessageBox.Show($"{Application.ProductName} is running!");
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ProxySwitchApplicationContext());
            }
        }
    }
}
