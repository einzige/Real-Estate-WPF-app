using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace ReferenceBook
{
    /// <summary>
    /// Interaction logic for RegionsView.xaml
    /// </summary>
    public partial class RegionsEditorView : UserControl, IDisposable
    {

        public RegionsEditorView()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            TreeViewItemViewModel treeNode = (TreeViewItemViewModel)tree.SelectedItem;

            System.Windows.MessageBox.Show(treeNode.ToString());
        }

        private void TreeViewItem_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }

        private void editorPopup_Opened(object sender, System.EventArgs e)
        {
            newNameTextBox.Clear(); 
        }

        private void adderPopup_Opened(object sender, System.EventArgs e)
        {
            newRegionTextBox.Clear();
        }

        private void CloseAdderPopup(object sender, System.Windows.RoutedEventArgs e)
        {
            adderPopup.IsOpen = false;
        }

        private void CloseEditorPopup(object sender, System.Windows.RoutedEventArgs e)
        {
            editorPopup.IsOpen = false;
        }

        public void Dispose()
        {
            this.DataContext = null;
            System.GC.Collect();
        }
    }
}
