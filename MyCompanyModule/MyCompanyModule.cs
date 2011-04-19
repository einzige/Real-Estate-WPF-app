using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Fluent;
using System.Windows;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace MyCompany
{
    using Infrastructure.Estate;
    using System.Windows.Controls;
    using EstateApi;

    [Module(ModuleName = "MyCompanyModule", OnDemand = true)]
    public class MyAgencyModule : IModule
    {
        DelegateCommand<object> ShowMainViewCommand;
        DelegateCommand<object> ShowRealtorViewCommand;
        DelegateCommand<object> ShowRealtorTableViewCommand;

        public MyAgencyModule()
        {
            ShowMainViewCommand = new DelegateCommand<object>(ShowMainView, delegate { return true; });
            MyCompanyCommands.ShowMyAgencyViewCommand.RegisterCommand(ShowMainViewCommand);

            ShowRealtorViewCommand = new DelegateCommand<object>(ShowRealtorView, delegate { return true; });
            MyCompanyCommands.ShowRealtorViewCommand.RegisterCommand(ShowRealtorViewCommand);

            ShowRealtorTableViewCommand = new DelegateCommand<object>(ShowRealtorTable, delegate { return true; });
            MyCompanyCommands.ShowRealtorsTableCommand.RegisterCommand(ShowRealtorTableViewCommand);
        }

        private void ShowRealtorTable(object param)
        {
            if (Api.ActivateTab("id_name_realtorstable"))
                return;

            RealtorsTableView view = new RealtorsTableView();
            view.Name = "id_name_realtorstable";
            view.ToolTip = "Таблица сотрудников";

            view.DataContext = new RealtorsTableViewModel(1); // FIX: id

            Api.AddTabItem(view);
        }

        private void ShowRealtorView(object param)
        {
            if (param == null)
            {
                if (Api.ActivateTab("id_name_realtoreditmode_0"))
                {
                    return;
                }
                else
                {
                    RealtorView _newRealtorView = new RealtorView();

                    _newRealtorView.Name = "id_name_realtoreditmode_0";
                    _newRealtorView.ToolTip = "Добавление риэлтора";

                    _newRealtorView.DataContext = new RealtorViewModel(param as int?);

                    Api.AddTabItem(_newRealtorView);
                    return;
                }
            }

            // else if param is RealtorViewModel

            RealtorViewModel viewModel = param as RealtorViewModel;
            if (viewModel == null) return;

            string viewID = "id_name_realtoreditmode_" + viewModel.ID.ToString();

            if (Api.ActivateTab(viewID)) return;

            // else create new tab

            RealtorView view = new RealtorView();
            view.DataContext = viewModel;

            view.ToolTip = "Риелтор №" + viewModel.ID.ToString();
            view.Name = viewID;

            Api.AddTabItem(view);
        }


        private void ShowMainView(object param)
        {
            if (Api.ActivateTab("id_name_mycompany"))
                return;

            MyAgencyView _agencyView = new MyAgencyView();

            _agencyView.Name = "id_name_mycompany";
            _agencyView.ToolTip = "Моя компания";
            _agencyView.DataContext = new MyCompanyViewModel();

            Api.AddTabItem(_agencyView);
        }


        public void Initialize()
        {
            #region RIBBON INITIALIZATION
            Ribbon ribbon = EstateApi.Api.Ribbon;

            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("MyCompanyModule;component/RibbonControls.xaml", UriKind.Relative);

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
    }
}
