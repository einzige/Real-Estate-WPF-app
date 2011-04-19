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

namespace ReferenceBook.Views
{
    /// <summary>
    /// Interaction logic for TagsView.xaml
    /// </summary>
    public partial class TagsView : UserControl, IDisposable
    {
        public TagsView()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            this.DataContext = null;
            System.GC.Collect();
        }
    }
}
