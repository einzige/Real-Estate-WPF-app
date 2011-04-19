using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace MyCompany
{
    public static partial class MyCompanyCommands
    {
        static CompositeCommand _showRealtorViewCommand;
        public static CompositeCommand ShowRealtorViewCommand
        {
            get
            {
                if (_showRealtorViewCommand == null)
                    _showRealtorViewCommand = new CompositeCommand();
                return _showRealtorViewCommand;
            }
        }

        static CompositeCommand _showMainViewCommand;
        public static CompositeCommand ShowMyAgencyViewCommand
        {
            get
            {
                if (_showMainViewCommand == null)
                    _showMainViewCommand = new CompositeCommand();
                return _showMainViewCommand;
            }
        }

        static CompositeCommand _showRealtorsTableCommand;
        public static CompositeCommand ShowRealtorsTableCommand
        {
            get
            {
                if (_showRealtorsTableCommand == null)
                    _showRealtorsTableCommand = new CompositeCommand();
                return _showRealtorsTableCommand;
            }
        }

        static CompositeCommand _newRealtorAddedCommand;
        public static CompositeCommand NewRealtorAddedCommand
        {
            get
            {
                if (_newRealtorAddedCommand == null)
                    _newRealtorAddedCommand = new CompositeCommand();
                return _newRealtorAddedCommand;
            }
        }
    }
}
