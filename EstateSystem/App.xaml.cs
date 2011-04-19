using System.Windows;

namespace EstateSystem
{
    using EstateApi;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper b = new Bootstrapper();
            b.Run();
            Api.MainWindow.Show();
        }
    }
}
