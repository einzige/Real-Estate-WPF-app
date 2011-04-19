namespace ReferenceBook
{
    using System;
    using System.Windows;
    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Fluent;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Infrastructure.Estate;
    using EstateApi;

    [Module(ModuleName = "ReferenceBookModule", OnDemand = true)]
    public class ReferenceBook : IModule
    {
        #region Commnands
        public DelegateCommand<object> ShowRegionsEditorCommand;
        public DelegateCommand<object> SetSimpleGridDictionaryTableCommand;
        #endregion

        public ReferenceBook(IUnityContainer container, IRegionManager regionManager)
        {
            ShowRegionsEditorCommand = new DelegateCommand<object>(ShowRegionEditorView, delegate { return true; });
            DatabaseDirectoryCommands.ShowRegionsEditor.RegisterCommand(ShowRegionsEditorCommand);

            SetSimpleGridDictionaryTableCommand = new DelegateCommand<object>(ShowSimpleGridView, delegate { return true; });
            DatabaseDirectoryCommands.SetDictionaryTableCommand.RegisterCommand(SetSimpleGridDictionaryTableCommand);
        }

        public void ShowRegionEditorView(object o)
        {
            if (Api.ActivateTab("id_name_database_dictionary"))
                return;

            RegionsEditorView _regionEditorView = new RegionsEditorView();
            _regionEditorView.DataContext = new RegionsEditorViewModel();
            _regionEditorView.Name = "id_name_database_dictionary_regions";

            _regionEditorView.ToolTip = "Справочник: Регионы";
            Api.AddTabItem(_regionEditorView);
            System.Windows.MessageBox.Show("xxx");
        }

        public void ShowSimpleGridView(object o)
        {
            Api.ActivateTab("id_name_database_dictionary");

            string tablename = o as string;

            SimpleGridView _simpleGridView = new SimpleGridView(tablename);
            _simpleGridView.Name = "id_name_database_dictionary";

            Api.AddTabItem(_simpleGridView);
        }

        #region IModule Members

        public void Initialize()
        {
            #region RIBBON INITIALIZATION
            Ribbon ribbon = EstateApi.Api.Ribbon;

            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("ReferenceBook;component/RibbonControls.xaml", UriKind.Relative);

            ResourceDictionary tabs = dictionary["Tabs"] as ResourceDictionary;

            #region LOADING TABS:

            if (tabs != null)
            {
                foreach (string key in tabs.Keys)
                {
                    RibbonTabItem newTab = tabs[key] as RibbonTabItem;
                    newTab.IsEnabled = false;
                    foreach (RibbonTabItem t in ribbon.Tabs)
                    {
                        if (t.Header == newTab.Header)
                        {
                            while (newTab.Groups.Count != 0)
                            {
                                RibbonGroupBox newGroup = newTab.Groups[0];
                                newTab.Groups.RemoveAt(0);
                                t.Groups.Add(newGroup);
                            }
                            newTab.IsEnabled = true;
                            break;
                        }
                    }
                    if (!newTab.IsEnabled)
                    {
                        newTab.IsEnabled = true;
                        ribbon.Tabs.Add(newTab);
                    }
                }
            }

            #endregion
            #endregion
        }

        #endregion
                 
    }
}
