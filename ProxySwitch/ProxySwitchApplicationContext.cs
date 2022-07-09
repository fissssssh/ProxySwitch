using Microsoft.Win32;
using ProxySwitch.Properties;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProxySwitch
{
    internal class ProxySwitchApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly CheckBox _startWithWindowsCore;

        public ProxySwitchApplicationContext()
        {
            _startWithWindowsCore = new CheckBox()
            {
                Text = "Start with windows"
            };
            _startWithWindowsCore.CheckedChanged += (s, e) =>
            {
                SetStartWithWindows(_startWithWindowsCore.Checked);
            };
            var startWithWindows = new ToolStripControlHost(_startWithWindowsCore);
            var exit = new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit());
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip(new Container());
            contextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                startWithWindows,
                exit
            });
            _notifyIcon = new NotifyIcon()
            {
                ContextMenuStrip = contextMenuStrip,
                Visible = true
            };
            _notifyIcon.DoubleClick += (s, e) => SwitchProxyEnable();
            SetBehaviors();
        }

        private bool GetStartWithWindows()
        {
            // use LocalMachine when program need administrator permission
            var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            return key.GetValue(Application.ProductName) != null;
        }

        private void SetBehaviors()
        {
            if (ProxyHelper.IsEnable)
            {
                _notifyIcon.Icon = Resources.on;
                _notifyIcon.Text = "proxy on";
            }
            else
            {
                _notifyIcon.Icon = Resources.off;
                _notifyIcon.Text = "proxy off";
            }
            _startWithWindowsCore.Checked = GetStartWithWindows();
        }

        private void SetStartWithWindows(bool isStart)
        {
            // use LocalMachine when program need administrator permission
            var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (isStart)
            {
                key.SetValue(Application.ProductName, Process.GetCurrentProcess().MainModule.FileName);
            }
            else
            {
                key.DeleteValue(Application.ProductName, false);
            }
        }

        private void SwitchProxyEnable()
        {
            ProxyHelper.SetProxyEnable(!ProxyHelper.IsEnable);
            SetBehaviors();
        }
    }
}