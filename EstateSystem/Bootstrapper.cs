// Zinin Serge 2009(R)
//
using System.IO;

namespace EstateSystem
{
    using System.Windows;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.UnityExtensions;
    using EstateApi;
    using System.Data;
    using System.Linq;
    

    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            App.Current.MainWindow = Api.MainWindow;
            return Api.MainWindow;
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\" };
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            try
            {
                ModuleManager _moduleManager = this.Container.Resolve<ModuleManager>();
                StreamReader sr = new StreamReader("modules.ini");
                string moduleName;


                while ((moduleName = sr.ReadLine()) != null)
                {
                    try
                    {
                        _moduleManager.LoadModule(moduleName);
                    }
                    catch (Microsoft.Practices.Composite.Modularity.ModuleNotFoundException e)
                    {
                        System.Windows.MessageBox.Show("Ошибка загрузки модуля (modules.ini): " + e.Message);
                    }

                }
                sr.Close();
            }
            catch (System.IO.IOException e)
            {
                System.Windows.MessageBox.Show("Не могу найти файл конфига модулей (modules.ini): " + e.Message);
            }
        }
    }
}
