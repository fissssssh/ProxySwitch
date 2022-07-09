using Microsoft.Win32;

namespace ProxySwitch
{
    internal class ProxyHelper
    {
        private const string KeyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";
        private const string ValueName = "ProxyEnable";
        public static bool IsEnable { get => ReadProxyStatus() == 1; }

        public static void SetProxyEnable(bool enable) => SetProxyStatus(enable ? 1 : 0);
        private static int ReadProxyStatus()
        {
            return (int)Registry.GetValue(KeyName, ValueName, 0);
        }

        private static void SetProxyStatus(int value)
        {
            Registry.SetValue(KeyName, ValueName, value, RegistryValueKind.DWord);
        }
    }
}