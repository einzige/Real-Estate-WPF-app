using System.Windows.Controls;

namespace MainWindowNS
{
    using Wpf.Controls;
    using Fluent;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Ribbon Ribbon
        {
            get { return this.ribbon; }
        }

        public TabControlView ContentControl
        {
            get { return this.contentPanel; }
        }
    }
}
