using Pharos.POS.Retailing.MultipScreen;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System;
namespace Pharos.POS.Retailing
{
    public static class MultipScreenManager
    {
        #region Property

        internal static Screen[] AllScreens
        {
            get
            {
                return Screen.AllScreens;
            }
        }

        internal static Screen PrimaryScreen
        {
            get
            {
                return Screen.PrimaryScreen;
            }
        }

        internal static IEnumerable<Screen> MinorScreens
        {
            get
            {
                return Screen.AllScreens.Where(o => o.Primary == false);

            }
        }

        internal static Screen FirstMinorScreen
        {
            get
            {
                return MinorScreens.FirstOrDefault();
            }
        }

        #endregion Property

        #region Method
        public static void ShowInScreen(this System.Windows.Window win)
        {
            SetScreen(win);
            win.Show();
        }
        public static void ShowDialogInScreen(this System.Windows.Window win)
        {
            SetScreen(win);
            win.ShowDialog();
        }

        private static void SetScreen(System.Windows.Window win) //设置屏幕位置
        {
            var attr = win.GetType().GetCustomAttributes(typeof(MultipScreenAttribute), false).FirstOrDefault(o => o is MultipScreenAttribute);
           // int index = 1;
            int index = CallSystem.Class.Config.ScreenIndex;
            bool ingoreOperation = false;
            WindowStartupLocationInScreen inScreen = WindowStartupLocationInScreen.CenterScreen;
            if (attr != null)
            {
                var temp = (attr as MultipScreenAttribute);
                index = temp.Index;
                inScreen = temp.InScreen;
                ingoreOperation = temp.IngoreMinorScreenError;
            }
            Screen screen = PrimaryScreen;
            if (index == 1 && FirstMinorScreen != null)
            {
                screen = FirstMinorScreen;
            }
            else if (index > 1 && index < MinorScreens.Count())
            {
                screen = MinorScreens.ElementAt(index);
            }
            else if (index > 0 && index >= MinorScreens.Count() && ingoreOperation)
            {
                return;
            }

            switch (inScreen)
            {
                case WindowStartupLocationInScreen.CenterScreen:
                    SetWindowInScreenCenter(win, screen);
                    break;
                case WindowStartupLocationInScreen.Manual:
                    SetWindowInScreenManual(win, screen);
                    break;
            }
        }

        private static void SetWindowInScreenCenter(System.Windows.Window win, Screen screen)
        {
            win.Top = screen.WorkingArea.Y + (screen.WorkingArea.Height - win.Height) / 2;
            win.Left = screen.WorkingArea.X + (screen.WorkingArea.Width - win.Width) / 2;
        }
        private static void SetWindowInScreenManual(System.Windows.Window win, Screen screen)
        {
            win.Top = screen.WorkingArea.Y;
            win.Left = screen.WorkingArea.X;
        }

        #endregion Method
    }
}

namespace Pharos.POS.Retailing.MultipScreen
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MultipScreenAttribute : Attribute
    {

        public MultipScreenAttribute(ScreenType type = ScreenType.Primary, WindowStartupLocationInScreen inScreen = WindowStartupLocationInScreen.CenterScreen)
            : this((int)type, inScreen)
        {
        }
        public MultipScreenAttribute(int index = 0, WindowStartupLocationInScreen inScreen = WindowStartupLocationInScreen.CenterScreen)
        {
            Index = index;
            InScreen = inScreen;
        }
        /// <summary>
        /// 在窗体初始化显示的位置
        /// </summary>
        public WindowStartupLocationInScreen InScreen { get; private set; }
        /// <summary>
        /// 屏幕索引， 0为主屏，1+为次屏
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// 当任何指定次屏没有找到时，如果该值为TRUE，则忽略这个页面的显示，否则将显示在主屏
        /// </summary>
        public bool IngoreMinorScreenError { get; private set; }
    }
}
namespace Pharos.POS.Retailing.MultipScreen
{
    public enum ScreenType
    {
        /// <summary>
        /// 主屏
        /// </summary>
        Primary = 0,
        /// <summary>
        /// 次屏
        /// </summary>
        Minor = 1,
    }
}
namespace Pharos.POS.Retailing.MultipScreen
{
    public enum WindowStartupLocationInScreen
    {
        Manual = 0,
        CenterScreen = 1,
    }
}