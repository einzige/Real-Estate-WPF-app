using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System;

namespace MyCompany
{
    using EstateHelpers.Imaging;
    using CustomControls;
    /// <summary>
    /// Interaction logic for RealtorView.xaml
    /// </summary>
    public partial class RealtorView : UserControl, IDisposable
    {
        public RealtorView()
        {
            InitializeComponent();
        }

        private void PhotoTB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "картинки (*.jpg)|*.jpg|png-картинки (*.png)|*.png";
            fd.ShowDialog();
            string filePath = fd.FileName;
            if (String.IsNullOrEmpty(filePath)) return;

            photoImage.Source = ImageHelper.GetResizedImageSource(filePath, 120, 0);
        }

        private void saveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
        }

        public void Dispose()
        {
            this.DataContext = null;
            System.GC.Collect();
        }
    }
}
