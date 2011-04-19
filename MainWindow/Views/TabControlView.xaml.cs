using System.Windows.Controls;

namespace MainWindowNS
{
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Wpf.Controls;
    using System.Windows;
    using System;

    /// <summary>
    /// Interaction logic for TabControlView.xaml
    /// </summary>
    public partial class TabControlView : UserControl
    {
        public TabControlView()
        {
            InitializeComponent();
        }

        public bool ContainsTabWithContentName(string tabID)
        {
            foreach (TabItem ti in tabControl.Items)
            {
                if ((ti.Content as UserControl) != null)
                    if ((ti.Content as UserControl).Name == tabID)
                    {
                        return true;
                    }
            }
            return false;
        }

        public bool ActivateTabWithContentName(string tabID)
        {
            foreach (TabItem ti in tabControl.Items)
            {
                if ((ti.Content as UserControl) != null)
                    if ((ti.Content as UserControl).Name == tabID)
                    {
                        tabControl.SelectedItem = ti;
                        return true;
                    }
            }
            return false;
        }

        public TabItem TabItemByContent(UserControl control)
        {
            foreach (TabItem ti in tabControl.Items)
            {
                if((ti.Content as UserControl) != null)
                    if ((ti.Content as UserControl).Name == control.Name)
                    {
                        return ti;
                    }
            }
            return null;
        }

        public void AddNewTabItem(object view)
        {
            TabItem ti = TabItemByContent(view as UserControl);
            if (ti != null)
            {
                tabControl.SelectedItem = ti;

                if (ti.Content is IDisposable)
                    (ti.Content as IDisposable).Dispose();
               
                ti.Content = view;
                ti.Header = (view as UserControl).ToolTip ?? "Новая вкладка";
                return;
            }

            tabControl.AddTabItem();
            UserControl newTabContent = view as UserControl;
            TabItem newTab = tabControl.SelectedItem as TabItem;

            newTab.Content = newTabContent;
            newTab.Header = newTabContent.ToolTip;
        }

        private void tabControl_TabItemClosing(object sender, TabItemCancelEventArgs e)
        {
            if(e.TabItem.Content is IDisposable)
                (e.TabItem.Content as IDisposable).Dispose();

            e.TabItem.Content = null; // set no parent
        }
    }
}
