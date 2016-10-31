using System;
using System.Configuration;

namespace CallSystem.Class
{
  public class Config
    {
        private static readonly AppSettingsReader appSettings = new AppSettingsReader();
        public static string ConnStr;
        public static int Lport = 8889;
        public static int Rport = 8888;
        public static string LName = "OPL8888";
        public static string RName = "OPL8889";
        public static string IPAddress = "192.168.2.101";
        public static int Delay = 10;
       // public static int WriteEncoder = 932;
        public static int WriteEncoder = 65001;
        public static int ScreenIndex = 1;
        public static string FontLeft;
        public static string FontRight;
        public static bool LoadConfig()
        {
            try
            {
                ConnStr = appSettings.GetValue("ConnectionStr",typeof(string)).ToString();
                IPAddress = appSettings.GetValue("IPAddress", typeof(string)).ToString();
                Lport = (int)appSettings.GetValue("Lport", typeof(int));
                Rport = (int)appSettings.GetValue("Rport", typeof(int));
                LName = appSettings.GetValue("LName",typeof(string)).ToString();
                RName = appSettings.GetValue("RName", typeof(string)).ToString();
                Delay = (int)appSettings.GetValue("Delay", typeof(int));
                WriteEncoder = (int)appSettings.GetValue("WriteEncoder", typeof(int));
                ScreenIndex = (int)appSettings.GetValue("ScreenIndex", typeof(int));
                FontLeft = appSettings.GetValue("FontLeft", typeof(string)).ToString();
                FontRight = appSettings.GetValue("FontRight", typeof(string)).ToString();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
