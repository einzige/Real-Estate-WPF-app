using System.Collections.Generic;

namespace ReferenceBook
{
    using System;
    using System.Windows;
    using System.ComponentModel;
    using System.Text;

    public delegate void ItemEndEditEventHandler(IEditableObject sender);

    public interface IEntityViewModel
    {
        IEntity Entity { get; set; }
        event ItemEndEditEventHandler ItemEndEdit;
    }

    public class EntityViewModel : IEditableObject, INotifyPropertyChanged, IEntityViewModel, IDataErrorInfo
    {
        protected IEntity _entity;

        public EntityViewModel(IEntity e)
        {
            this._entity = e;
        }

        public IEntity Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }

        public override int GetHashCode()
        {
            return _entity.ID;
        }

        public override bool Equals(object obj)
        {
            EntityViewModel c = obj as EntityViewModel;
            if (c == null)
                return base.Equals(obj);

            if (this.ID == c.ID) return true;
            return false;
        }

        #region wrapper properties

        public int ID
        {
            get { return _entity.ID; }
            set 
            { 
                _entity.ID = value;
                RaisePropertyChanged("ID");
            }
        }

        public string Name
        {
            get { return _entity.Name; }
            set 
            {
                _entity.Name = value;
                if (String.IsNullOrEmpty(value))
                {
                    _errors["Name"] = "Имя не может быть пустым.";
                    MessageBox.Show("Имя не может быть пустым.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    _errors["Name"] = null;

                RaisePropertyChanged("Name");
            }
        }

        #endregion

        #region IERROR
        private Dictionary<string, string> _errors = new Dictionary<string, string>();
        public string Error
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in _errors)
                    if (item.Value != null)
                        sb.AppendLine(item.Value);

                return sb.ToString();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                _errors.TryGetValue(columnName, out result);

                return result;
            }
        }

        #endregion

        #region IEditable members

        public void BeginEdit(){}

        public void CancelEdit(){}

        public void EndEdit()
        {
            if (ItemEndEdit != null){ ItemEndEdit(this); }
        }

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region IEntityViewModel members 

        public event ItemEndEditEventHandler ItemEndEdit;

        #endregion
    }
}
