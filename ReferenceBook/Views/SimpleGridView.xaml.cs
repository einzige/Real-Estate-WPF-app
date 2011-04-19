using System.Windows.Controls;
using System;

namespace ReferenceBook
{
    /// <summary>
    /// Interaction logic for SimpleGridView.xaml
    /// </summary>
    public partial class SimpleGridView : UserControl, IDisposable
    {
        public SimpleGridViewModel Model
        {
            get { return (SimpleGridViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }

        public SimpleGridView()
        {
            InitializeComponent();
        }

        public SimpleGridView(string tableName)
        {
            InitializeComponent();
            this.Model = new SimpleGridViewModel(tableName);
        }

        public void Dispose()
        {
            this.DataContext = null;
            System.GC.Collect();
        }
    }
}
