using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System;

namespace MyCompany
{
    using EstateHelpers.Imaging;

    /// <summary>
    /// Interaction logic for MyAgencyView.xaml
    /// </summary>
    public partial class MyAgencyView : UserControl, IDisposable
    {
        public MyAgencyView()
        {
            InitializeComponent();
        }

        private void mapBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mapTB.Text.Contains("http://"))
                    this.mapBrowser.Source = new System.Uri(mapTB.Text.Trim());
                else
                {
                    this.mapBrowser.Source = new System.Uri("http://" + mapTB.Text.Trim());
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Вы ввели неверный адрес", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                mapTB.Undo();
            }
        }

        private void mapBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            mapBtn_Click(null, null);
        }

        private void LogoTB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "картинки (*.jpg)|*.jpg|png-картинки (*.png)|*.png";
            fd.ShowDialog();
            string filePath = fd.FileName;
            if (String.IsNullOrEmpty(filePath)) return;

            logoImage.Source = ImageHelper.GetResizedImageSource(filePath, 120, 0);
        }

        private void passageTB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "картинки (*.jpg)|*.jpg|png-картинки (*.png)|*.png";
            fd.ShowDialog();
            string filePath = fd.FileName;
            if (String.IsNullOrEmpty(filePath)) return;

            passageImage.Source = ImageHelper.GetResizedImageSource(filePath, 800, 0);
        }

        public void Dispose()
        {
            this.DataContext = null;
            System.GC.Collect();
        }
    }
}
