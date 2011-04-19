using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EstateApi
{
    using MainWindowNS;
    using Fluent;

    internal static class EstateSystem
    {
        public static MainWindow mainWindow = new MainWindow();
    }

    public static class Api
    {
        public static RibbonWindow MainWindow
        {
            get { return (EstateSystem.mainWindow as RibbonWindow); }
        }

        public static Ribbon Ribbon
        {
            get { return EstateSystem.mainWindow.Ribbon; }
        }

        public static bool ContainsControlName(string name)
        {
            return EstateSystem.mainWindow.ContentControl.ContainsTabWithContentName(name);
        }

        public static bool ActivateTab(string tabID)
        {
            return EstateSystem.mainWindow.ContentControl.ActivateTabWithContentName(tabID);
        }

        public static void AddTabItem(object view)
        {
            EstateSystem.mainWindow.ContentControl.AddNewTabItem(view);
        }
    }

}
