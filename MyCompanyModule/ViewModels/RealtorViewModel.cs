using System;
using System.ComponentModel;
using Microsoft.Practices.Composite.Presentation.Commands;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;

namespace MyCompany
{
    using EstateDataAccess;
    using EstateHelpers.Imaging;
    using SharedTypes;
    using FocusVMLib;

    public class RealtorViewModel : IDataErrorInfo, INotifyPropertyChanged, IFocusMover
    {
        private Realtor _realtor;

        #region Properties

        private bool _isNotSaved = false;
        public bool IsNotSaved {
            get { return _isNotSaved; }
            set
            {
                _isNotSaved = value;
                OnProperyChanged("IsNotSaved");
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public Realtor Realtor
        {
            get { return _realtor; }
        }

        public int ID
        {
            get { return _realtor.ID; }
        }

        public string Standing
        {
            get
            {
                int s = DateTime.Now.Year - (Commencement ?? DateTime.Now).Year;
                if (s < 1) return "Меньше года";
                return s.ToString() + " лет";
            }
        }

        public BitmapImage Photo
        {
            get
            {
                if (_realtor.Photo == null) return null;
                return ImageHelper.GetBitmapImage(_realtor.Photo.ToArray(), 70, 0);
            }
            set
            {
                _realtor.Photo = ImageHelper.GetImageData(value);
                OnProperyChanged("Photo");
            }
        }
        public string Appointment
        {
            get { return _realtor.Appointment; }
            set
            {
                _realtor.Appointment = value;
                OnProperyChanged("Appointment");
            }
        }
        public string FIO
        {
            get { return _realtor.FIO; }
            set
            {
                _realtor.FIO = value;
                OnProperyChanged("FIO");
            }
        }
        public DateTime? LastUpdateTime
        {
            get { return _realtor.LastUpdateTime; }
            set
            {
                _realtor.LastUpdateTime = value;
                OnProperyChanged("LastUpdateTime");
            }
        }
        public DateTime? CreationTime
        {
            get { return _realtor.CreationTime; }
            set
            {
                _realtor.LastUpdateTime = value;
                OnProperyChanged("CreationTime");
            }
        }
        public DateTime? Birthday
        {
            get { return _realtor.Birthday; }
            set
            {
                _realtor.Birthday = value;
                OnProperyChanged("Birthday");
            }
        }
        public DateTime? Commencement
        {
            get { return _realtor.Commencement; }
            set
            {
                _realtor.Commencement = value;
                OnProperyChanged("Commencement");
            }
        }
        public DateTime? Discharge
        {
            get { return _realtor.Discharge; }
            set
            {
                _realtor.Discharge = value;
                OnProperyChanged("Discharge");
            }
        }

        public int PermissionID
        {
            get { return _realtor.PermissionID; }
        }

        public SimpleEntity Permission
        {
            get
            {
                if (this._realtor.PermissionID == 0) return new SimpleEntity() { ID = 0, Name = "" };
                return (from p in Permissions where p.ID == _realtor.PermissionID select p).First<SimpleEntity>();
            }
            set
            {
                if (value == null) return;
                _realtor.PermissionID = value.ID;
                OnProperyChanged("Permission");
            }
        }
        public List<SimpleEntity> Permissions
        {
            get
            {
                List<SimpleEntity> p = new List<SimpleEntity>();

                p.Add(new SimpleEntity() { Name = "Суперпользователь", ID = (int)PermissionType.super });
                p.Add(new SimpleEntity() { Name = "Администратор", ID = (int)PermissionType.admin });
                p.Add(new SimpleEntity() { Name = "Риелтор", ID = (int)PermissionType.realtor });
                p.Add(new SimpleEntity() { Name = "Визор", ID = (int)PermissionType.visor });

                return p;
            }
        }
        public string Login
        {
            get { return _realtor.Login; }
            set
            {
                _realtor.Login = value;
                OnProperyChanged("Login");
            }
        }
        public string Password
        {
            get { return _realtor.Password ?? String.Empty; }
            set
            {
                _realtor.Password = value;
                OnProperyChanged("Password");
                OnProperyChanged("PasswordConfirmation");
            }
        }
        private string _passwordConfirmation;
        public string PasswordConfirmation
        {
            get { return _passwordConfirmation ?? String.Empty; }
            set
            {
                _passwordConfirmation = value;
                OnProperyChanged("PasswordConfirmation");
                OnProperyChanged("Password");
            }
        }
        public bool? Enabled
        {
            get { return _realtor.Enabled; }
            set
            {
                _realtor.Enabled = value;
                OnProperyChanged("Enabled");
            }
        }
        public string ContactPhone
        {
            get { return _realtor.ContactPhone; }
            set
            {
                _realtor.ContactPhone = value;
                OnProperyChanged("Phone");
            }
        }
        public string OfficePhone
        {
            get { return _realtor.OfficePhone; }
            set
            {
                _realtor.OfficePhone = value;
                OnProperyChanged("OfficePhone");
            }
        }
        public string Skype
        {
            get { return _realtor.Skype; }
            set
            {
                _realtor.Skype = value;
                OnProperyChanged("Skype");
            }
        }
        public string ICQ
        {
            get { return _realtor.ICQ; }
            set
            {
                _realtor.ICQ = value;
                OnProperyChanged("ICQ");
            }
        }

        public string Mail
        {
            get { return _realtor.Mail; }
            set
            {
                _realtor.Mail = value;
                OnProperyChanged("Mail");
            }
        }
        public string Info
        {
            get { return _realtor.Info; }
            set
            {
                _realtor.Info = value;
                OnProperyChanged("Info");
            }
        }
        public int? Remuneration
        {
            get { return _realtor.Remuneration; }
            set
            {
                _realtor.Remuneration = value;
                OnProperyChanged("Remuneration");
            }
        }

        #endregion

        #region Commands

        private DelegateCommand<object> _saveChangesCommand;
        private DelegateCommand<object> _cancelChangesCommand;
        public DelegateCommand<object> SaveChangesCommand
        {
            get { 
                if(_saveChangesCommand == null)
                    _saveChangesCommand = new DelegateCommand<object>(this.SaveChanges, delegate { return IsNotSaved; });
                return _saveChangesCommand; 
            }
        }
        public DelegateCommand<object> CancelChangesCommand
        {
            get { 
                if(_cancelChangesCommand == null)
                    _cancelChangesCommand = new DelegateCommand<object>(this.CancelChanges, delegate { return true; });
                return _cancelChangesCommand; 
            }
        }

        #endregion

        #region Constructors
        public RealtorViewModel(int? id)
        {
            IsNotSaved = false;
            _realtor = AgencyModel.GetRealtor(id);
        }
        public RealtorViewModel(Realtor r)
        {
            IsNotSaved = false;
            this._realtor = r;
        }
        #endregion

        public void SaveChanges(object param)
        {
            if (this.HasError)
            {
                string msg;
                IsValid(this.Error, out msg);

                System.Windows.MessageBox.Show(msg, "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                this.RaiseMoveFocus(this.Error);

                // show the View on Error
                MyCompanyCommands.ShowRealtorViewCommand.Execute(this);

                return;
            }

            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Вы действительно хотите сохранить внесенные изменения?", "Внимание, вопрос", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);

            if (res == System.Windows.MessageBoxResult.Cancel ||
                res == System.Windows.MessageBoxResult.No)
                return;

            IsNotSaved = false;
            AgencyModel.UpdateRealtor(_realtor);

            MyCompanyCommands.NewRealtorAddedCommand.Execute(this);
        }
        public void CancelChanges(object param)
        {
            System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show("Вы действительно хотите отменить изменения?", "Внимание, вопрос", System.Windows.MessageBoxButton.YesNoCancel, System.Windows.MessageBoxImage.Question);

            if (res == System.Windows.MessageBoxResult.Cancel ||
                res == System.Windows.MessageBoxResult.No)
                return;

            _realtor = AgencyModel.GetRealtor(_realtor.ID);
            OnProperyChanged(String.Empty); // update all bindings
        }

        #region INotifyProperyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnProperyChanged(string propName)
        {
            if(propName != "IsNotSaved") IsNotSaved = true;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        #region IERROR

        static string[] _validatedProperties = new string[]
        {
            "Login",
            "Permission",
            "FIO",
            "Password",
            "PasswordConfirmation",
            "Appointment"
        };

        bool IsValid(string property, out string errorMessage)
        {
            errorMessage = null;
            if (property == "Login")
            {
                if (String.IsNullOrWhiteSpace(this.Login))
                {
                    errorMessage = "Логин не может быть пустым.";
                }
            }
            else if (property == "FIO")
            {
                if (String.IsNullOrWhiteSpace(this.FIO))
                {
                    errorMessage = "ФИО не может быть пустым.";
                }
            }
            else if (property == "Permission")
            {
                if (this.Permission.ID == 0)
                    errorMessage = "Назначение прав пользователя обязательно";
            }
            else if (property == "Password")
            {
                if (this.Password.Trim().Length < 4)
                {
                    errorMessage = "Длина пароля - не менее 4х символов";
                }
            }
            else if (property == "PasswordConfirmation")
            {
                if (this.Password != PasswordConfirmation)
                {
                    errorMessage = "Подтвердите пароль";
                }
            }
            else if (property == "Appointment")
            {
                if (String.IsNullOrWhiteSpace(this.Appointment))
                {
                    errorMessage = "Укажите должность";
                }
            }
            //else if(this.Remuneratio
            

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

        #region OBJECT redefinions
        public override bool Equals(object obj)
        {
            if(!(obj is RealtorViewModel))
                return base.Equals(obj);
            return (obj as RealtorViewModel).ID == this.ID;
        }
        public override int GetHashCode()
        {
            return this.ID;
        }
        #endregion
    }
}
