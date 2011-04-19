using System;

namespace ReferenceBook
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows.Input;
    using Microsoft.Practices.Composite.Presentation.Commands;

    public delegate void PersistenceErrorHandler(SimpleGridViewModel dataProvider, Exception e);

    public class SimpleGridViewModel : INotifyPropertyChanged
    {
        #region Commands
        public ICommand AddNewRecordCommand { get; set; }
        #endregion

        #region Events

        public static event PersistenceErrorHandler PersistenceError;

        #endregion

        #region Properties

        public EntityCollection _entities;
        public EntityCollection Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public string NewRecordName { get; set; }

        string _rusTableName;
        string _tableName;

        public string RusTableName
        {
            get { return _rusTableName; }
            set
            {
                _rusTableName = value;
            }
        }
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                if (_tableName == value) return;
                _tableName = value;
                this._entities = EntitiesDatabaseDirectory.GetDatabaseDictionary(_tableName);

                this._entities.ItemEndEdit += new ItemEndEditEventHandler(EntitiesEndEdit);
                this._entities.CollectionChanged += new NotifyCollectionChangedEventHandler(EntitiesCollectionChanged);

                RaisePropertyChanged("Entities");

                switch (value)
                {
                    case "Balconies":
                        RusTableName = "Балконы";
                        break;
                    case "BuildingSeries":
                        RusTableName = "Строительные серии";
                        break;
                    default:
                        RusTableName = value;
                        break;
                }
                RaisePropertyChanged("RusTableName");
            }
        }

        #endregion

        #region Constructors
        public SimpleGridViewModel()
        {
            AddNewRecordCommand = new DelegateCommand<string>(this.AddNewRecord, delegate { return true; });
        }

        public SimpleGridViewModel(string tableName) : this()
        {
            TableName = tableName;
        }
        #endregion

        #region Actions

        void AddNewRecord(object parameter)
        {
            if (String.IsNullOrEmpty(NewRecordName)) return;

            NewRecordName = NewRecordName.Trim();

            SimpleEntity e = new SimpleEntity() { Name = NewRecordName };
            int id = EntitiesDatabaseDirectory.InsertEntity(e);

            // we need to set last inserted id to the collection item
            Entities.Add(new EntityViewModel(e) { ID = id }); 

            NewRecordName = ""; // eg clear textbox...
            RaisePropertyChanged("AddNewRecordName");
        }

        public void DeleteRow(object p) { this.Entities.Remove(p as EntityViewModel); }

        #endregion

        #region Events

        void EntitiesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                try
                {
                    foreach (EntityViewModel item in e.OldItems)
                    {
                        EntitiesDatabaseDirectory.DeleteEntity(item);
                    }
                }
                catch (Exception ex)
                {
                    if (PersistenceError != null)
                    {
                        PersistenceError(this, ex);
                    }
                }
            }
        }

        void EntitiesEndEdit(IEditableObject sender)
        {
            try
            {
                EntitiesDatabaseDirectory.UpdateEntity((EntityViewModel)sender);
            }
            catch (Exception ex)
            {
                if (PersistenceError != null)
                {
                    PersistenceError(this, ex);
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
