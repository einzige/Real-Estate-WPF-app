using System;
using System.ComponentModel;
using System.Linq;

namespace MyCompany
{
    using EstateDataAccess;
    using EstateHelpers.Imaging;
    using FocusVMLib;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using System.Windows.Media.Imaging;

    public class MyCompanyViewModel : INotifyPropertyChanged, IDataErrorInfo, IFocusMover
    {
        private Agency _agency;

        #region Properties

        public BitmapImage Logo
        {
            get
            {
                return ImageHelper.GetBitmapImage(_agency.Logo.ToArray(), 120, 0);
            }
            set
            {
                _agency.Logo = ImageHelper.GetImageData(value);
            }
        }

        public BitmapImage Passage
        {
            get 
            {
                return ImageHelper.GetBitmapImage(_agency.Passage.ToArray(), 0, 0);
            }
            set
            {
                _agency.Passage = ImageHelper.GetImageData(value);
            }
        }

        public DateTime? CreationTime
        {
            get { return _agency.CreationTime; }
        }

        public DateTime? LastUpdateTime
        {
            get { return _agency.LastUpdateTime; }
            set
            {
                _agency.LastUpdateTime = value;
                OnProperyChanged("LastUpdateTime");
            }
        }

        public DateTime? Foundation
        {
            get { return _agency.Foundation; }
            set
            {
                _agency.Foundation = value;
                OnProperyChanged("Foundation");
            }
        }

        public bool? Enabled
        {
            get { return _agency.Enabled; }
            set
            {
                _agency.Enabled = value;
                OnProperyChanged("Enabled");
            }
        }

        public string Name
        {
            get { return _agency.Name; }
            set
            {
                _agency.Name = value;
                OnProperyChanged("Name");
            }
        }
        public string Phone
        {
            get { return _agency.Phone; }
            set
            {
                _agency.Phone = value;
                OnProperyChanged("Phone");
            }
        }
        public string Skype
        {
            get { return _agency.Skype; }
            set
            {
                _agency.Skype = value;
                OnProperyChanged("Skype");
            }
        }
        public string ICQ
        {
            get { return _agency.ICQ; }
            set
            {
                _agency.ICQ = value;
                OnProperyChanged("ICQ");
            }
        }

        public string Mail
        {
            get { return _agency.Mail; }
            set
            {
                _agency.Mail = value;
                OnProperyChanged("Mail");
            }
        }

        public string Address
        {
            get { return _agency.Address; }
            set
            {
                _agency.Address = value;
                OnProperyChanged("Address");
            }
        }
        public string MapUrl
        {
            get { return _agency.MapUrl; }
            set
            {
                _agency.MapUrl = value;
                OnProperyChanged("MapUrl");
            }
        }
        public string SiteUrl
        {
            get { return _agency.SiteUrl; }
            set
            {
                _agency.SiteUrl = value;
                OnProperyChanged("SiteUrl");
            }
        }
        public string About
        {
            get { return _agency.About; }
            set
            {
                _agency.About = value;
                OnProperyChanged("About");
            }
        }

        #endregion

        #region Commands

        private DelegateCommand<object> _saveChangesCommand;
        private DelegateCommand<object> _cancelChangesCommand;

        public DelegateCommand<object> SaveChangesCommand
        {
            get { return _saveChangesCommand; }
        }

        public DelegateCommand<object> CancelChangesCommand
        {
            get { return _cancelChangesCommand; }
        }

        #endregion

        #region Constructors
        public MyCompanyViewModel()
        {
            _saveChangesCommand = new DelegateCommand<object>(this.SaveChanges, delegate { return true; });
            _cancelChangesCommand = new DelegateCommand<object>(this.CancelChanges, delegate { return true; });

            this._agency = AgencyModel.GetAgency(1);
        }
        #endregion

        #region Methods
        public void SaveChanges(object param)
        {
            if (this.HasError)
            {
                string msg;
                IsValid(this.Error, out msg);

                System.Windows.MessageBox.Show(msg, "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                this.RaiseMoveFocus(this.Error);
                return;
            }

            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Вы действительно хотите сохранить внесенные изменения?", "Внимание, вопрос", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);

            if (res == System.Windows.MessageBoxResult.Cancel ||
                res == System.Windows.MessageBoxResult.No)
                return;

            AgencyModel.UpdateAgency(_agency);
        }

        public void CancelChanges(object param)
        {
            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Вы действительно хотите отменить изменения?", "Внимание, вопрос", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);

            if (res == System.Windows.MessageBoxResult.Cancel ||
                res == System.Windows.MessageBoxResult.No)
                return;

            this._agency = AgencyModel.GetAgency(1); // FIX: id
            OnProperyChanged(String.Empty); // update all bindings
        }
        #endregion

        #region INotifyProperyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnProperyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        #region IERROR

        static string[] _validatedProperties = new string[]
        {
            "Address",
            "Name"
        };

        bool IsValid(string property, out string errorMessage)
        {
            errorMessage = null;
            if (property == "Address")
            {
                if (String.IsNullOrWhiteSpace(this.Address))
                {
                    errorMessage = "Адрес должен быть указан.";
                }
            }
            else if (property == "Name")
            {
                if (String.IsNullOrWhiteSpace(this.Name))
                {
                    errorMessage = "Укажите имя.";
                }
            }

            return errorMessage == null;
        }

        bool HasError
        {
            get { return _validatedProperties.Any(prop => !String.IsNullOrEmpty(this[prop])); }
        }

        public string Error { get; private set; }

        public string this[string property]
        {
            get
            {
                string msg;
                if (this.IsValid(property, out msg))
                {
                    this.Error = null;
                }
                else
                {
                    this.Error = property;
                }
                return msg;
            }
        }

        #endregion

        #region IFocusMover Members

        public event EventHandler<MoveFocusEventArgs> MoveFocus;

        void RaiseMoveFocus(string focusedProperty)
        {
            var handler = this.MoveFocus;
            if (handler != null)
            {
                var args = new MoveFocusEventArgs(focusedProperty);
                handler(this, args);
            }
        }

        #endregion
    }
}
