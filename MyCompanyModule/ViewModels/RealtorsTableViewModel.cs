using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MyCompany
{
    using EstateDataAccess;
    using System.Windows.Media.Imaging;
    using EstateHelpers.Imaging;
    using SharedTypes;
    using System.Windows.Data;
    using Microsoft.Practices.Composite.Presentation.Commands;

    class RealtorsTableViewModel : INotifyPropertyChanged
    {
        public DelegateCommand<object> AddNewRealtorCommand { get; private set; }
        public DelegateCommand<object> GroupCollectionCommand { get; private set; }

        private bool? _groupByAppointment;
        public bool? GroupByAppointment
        {
            get { return _groupByAppointment; }
            set
            {
                this._groupByAppointment = value;
                if (value ?? false)
                {
                    Realtors.GroupDescriptions.Add(new PropertyGroupDescription("Appointment"));
                    OnProperyChanged("Realtors");
                }
                else
                {
                    foreach (PropertyGroupDescription g in Realtors.GroupDescriptions)
                    {
                        if (g.PropertyName == "Appointment")
                        {
                            Realtors.GroupDescriptions.Remove(g);
                            OnProperyChanged("Realtors");
                            return;
                        }
                    }
                }
            }
        }

        private ListCollectionView _realtors;
        public ListCollectionView Realtors
        {
            get { return _realtors; }
            set
            {
                _realtors = value;
                OnProperyChanged("Realtors");
            }
        }

        public RealtorsTableViewModel(int agencyID)
        {
            DelegateCommand<object> AddNewRealtorCommand = new DelegateCommand<object>(this.AddNewRealtor, delegate { return true; });
            MyCompanyCommands.NewRealtorAddedCommand.RegisterCommand(AddNewRealtorCommand);

            GroupCollectionCommand = new DelegateCommand<object>(this.GroupCollection, delegate { return true; });

            List<Realtor> rlist = AgencyModel.GetRealtors(agencyID);
            List<RealtorViewModel> rvlist = new List<RealtorViewModel>();

            foreach (Realtor r in rlist)
            {
                rvlist.Add(new RealtorViewModel(r));
            }

            Realtors = new ListCollectionView(rvlist);
            //Realtors.GroupDescriptions.Add(new PropertyGroupDescription("Appointment"));

            OnProperyChanged("Realtors"); // FIX: may be spare
        }

        private void GroupCollection(object o)
        {
            string fieldname = o as string;
            if (String.IsNullOrEmpty(fieldname))
            {
                // set no grouping
                Realtors.GroupDescriptions.Clear();
                OnProperyChanged("Realtors");
                return;
            }

            // delete group descriptions
            Realtors.GroupDescriptions.Clear();

            // add new group description
            Realtors.GroupDescriptions.Add(new PropertyGroupDescription(fieldname));
            OnProperyChanged("Realtors");
        }

        private void AddNewRealtor(object o)
        {
            if (Realtors.Contains(o as RealtorViewModel)) return;
            this.Realtors.AddNewItem(o as RealtorViewModel);
        }

        #region INotifyProperyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnProperyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion
    }
}
