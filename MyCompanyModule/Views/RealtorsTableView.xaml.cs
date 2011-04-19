using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace MyCompany
{
    using EstateApi;
    using EstateDataAccess;
    using System.ComponentModel;
using System.Collections;

    /// <summary>
    /// Interaction logic for RealtorsTableView.xaml
    /// </summary>
    public partial class RealtorsTableView : System.Windows.Controls.UserControl, IDisposable
    {
        public RealtorsTableView()
        {
            InitializeComponent();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // iteratively traverse the visual tree
            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            if (dep is DataGridRow)
            {
                DataGridRow row = dep as DataGridRow;
                string viewID = "id_name_realtoreditmode_" + (row.Item as RealtorViewModel).ID.ToString();

                if (!Api.ActivateTab(viewID))
                {
                    // create new Tab by this RealtorViewModel
                    MyCompanyCommands.ShowRealtorViewCommand.Execute(row.Item);
                }
            }
        }

        private void OnMailClick(object sender, RoutedEventArgs e)
        {
            Process.Start("mailto:" + (e.Source as Hyperlink).NavigateUri.ToString());
        }

        #region SORTING FORCE

        private void RealtorsGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            ListSortDirection direction =
                    (e.Column.SortDirection != ListSortDirection.Ascending) ?
                        ListSortDirection.Ascending :
                        ListSortDirection.Descending;
            e.Column.SortDirection = direction;
            
            ListCollectionView lcv = (ListCollectionView)
                CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            SortLogic sortLogic = new SortLogic(direction, e.Column);
            lcv.CustomSort = sortLogic;
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            this.DataContext = null;
            System.GC.Collect();
        }
        #endregion
    }

    public class SortLogic : IComparer
    {
        public delegate int SortDelegate(RealtorViewModel arg1, RealtorViewModel arg2);

        SortDelegate _compare;
        public SortLogic(ListSortDirection direction, DataGridColumn column)
        {
            int dir = (direction == ListSortDirection.Ascending) ? 1 : -1;
            switch ((string)column.Header)
            {
                case "ФИО":
                    _compare = (x, y) => x.FIO.CompareTo(y.FIO) * dir;
                    break;
                case "Права":
                    _compare = (x, y) => x.Permission.ID.CompareTo(y.Permission.ID) * dir;
                    break;
                case "Стаж":
                    _compare = (x, y) => (x.Commencement ?? DateTime.MinValue).CompareTo(y.Commencement ?? DateTime.MinValue) * dir;
                    break;
                case "Должность":
                    _compare = (x, y) => x.Appointment.CompareTo(y.Appointment) * dir;
                    break;
                case "Зарплата":
                    _compare = (x, y) => (x.Remuneration ?? 0).CompareTo(y.Remuneration ?? 0) * dir;
                    break;
                case "Вкл.":
                    _compare = (x, y) => (x.Enabled ?? false).CompareTo(y.Enabled ?? false) * dir;
                    break;
                case "":
                    _compare = (x, y) => (x.IsNotSaved).CompareTo(y.IsNotSaved) * dir;
                    break;
                default:
                    _compare = (x, y) => x.FIO.CompareTo(y.FIO) * dir;
                    break;
            }
        }
        int IComparer.Compare(object X, object Y)
        {
            return _compare((RealtorViewModel)X, (RealtorViewModel)Y);
        }
    }
}
