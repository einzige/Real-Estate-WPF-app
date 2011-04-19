using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Presentation.Commands;
using System.ComponentModel;
using System;

namespace ReferenceBook
{
    public static class Database
    {
        #region GetCounries

        public static List<Country> GetCountries()
        {
            List<Country> res = new List<Country>();
            res.Add(new Country("Россия"));
            res.Add(new Country("Украина"));
            return res;
        }

        #endregion

        #region GetRegions

        public static List<Region> GetRegions(Country country)
        {
            List<Region> res = new List<Region>();
            res.Add(new Region("Красноярский край"));
            res.Add(new Region("Алтайский край"));
            return res;
        }

        #endregion // GetRegions

        #region GetDistricts

        public static District[] GetDistricts(Region region)
        {
            switch (region.Name)
            {
                case "Northeast":
                    return new District[]
                    {
                        new District("Connecticut"),
                        new District("New York")
                    };

                case "Midwest":
                    return new District[]
                    {
                        new District("Indiana")
                    };

                default:
                    return new District[]
                    {
                        new District("Шарыповский район"),
                        new District("Красноярский район")
                    };

            }
        }

        #endregion // GetStates

        #region GetCities

        public static City[] GetCities(District district)
        {
            switch (district.Name)
            {
                case "Connecticut":
                    return new City[]
                    {
                        new City("Bridgeport"),
                        new City("Hartford"),
                        new City("New Haven")
                    };

                case "New York":
                    return new City[]
                    {
                        new City("Buffalo"),
                        new City("New York"),
                        new City("Syracuse")          
                    };

                case "Indiana":
                    return new City[]
                    {
                        new City("Evansville"),
                        new City("Fort Wayne"),
                        new City("Indianapolis"),
                        new City("South Bend")
                    };
                default:
                    return new City[]
                    {
                        new City("Красноярск"),
                        new City("Шарыпово"),
                        new City("Канск"),
                        new City("Ачинск")
                    };
            }
        }

        #endregion // GetCities

        #region boroughs

        public static Borough[] GetBoroughs(City city)
        {
            return new Borough[]
                    {
                        new Borough("b1"),
                        new Borough("b2"),
                        new Borough("b3")          
                    };
        }

        public static Street[] GetStreets(City city)
        {
            return new Street[]
            {
                new Street("s1"),
                new Street("s2")
            };
        }
        #endregion 
    
        public static Microdistrict[] GetMicrodistricts(Borough borough)
        {
            return new Microdistrict[]
            {
                new Microdistrict("Взлетка"),
                new Microdistrict("Северный")
            };
        }
    }

    #region Entity Types

    public class MicrodistrictViewModel : TreeViewItemViewModel
    {
        public MicrodistrictViewModel(Microdistrict m, BoroughViewModel parentBorough)
            :base(parentBorough, false)
        {
            _entity = m;
        }

        public override string Name
        {
            get { return _entity.Name; }
        }
    }

    public class StreetViewModel : TreeViewItemViewModel
    {
        public StreetViewModel(Street street, CityViewModel parentCity)
            :base(parentCity, false)
        {
            _entity = street;
        }

        public override string Name
        {
            get { return _entity.Name; }
        }
    }

    public class BoroughViewModel : TreeViewItemViewModel
    {
        public BoroughViewModel(Borough borough, CityViewModel parentCity)
            :base(parentCity, true)
        {
            _entity = borough;
        }

        public override string Name
        {
            get { return _entity.Name; }
        }

        protected override void LoadChildren()
        {
            foreach (Microdistrict m in RegionsDatabaseDirectory.GetMicrodistricts(_entity as Borough))
                base.Children.Add(new MicrodistrictViewModel(m, this));
        }
    }

    public class CityGroupViewModel : TreeViewItemViewModel
    {
        CityViewModel _parent;

        public override string Name { get; set; }

        public CityGroupViewModel(string name, CityViewModel parentCity)
            : base(parentCity, true)
        {
            this.Name = name;
            _parent = parentCity;
        }

        protected override void LoadChildren()
        {
            switch (Name)
            {
                case "Улицы":
                    foreach (Street street in RegionsDatabaseDirectory.GetStreets(_parent.City))
                        base.Children.Add(new StreetViewModel(street, _parent));
                    break;
                case "Районы города":
                    foreach (Borough borough in RegionsDatabaseDirectory.GetBoroughs(_parent.City))
                        base.Children.Add(new BoroughViewModel(borough, _parent));
                    break;
            }
        }
    }

    public class CityViewModel : TreeViewItemViewModel
    {
        public CityViewModel(City city, DistrictViewModel parentDistrict)
            : base(parentDistrict, true)
        {
            _entity = city;
        }

        public City City
        {
            get { return _entity as City; }
        }

        public override string Name
        {
            set { _entity.Name = value; OnPropertyChanged("Name"); }
            get { return _entity.Name; }
        }

        protected override void LoadChildren()
        {
            base.Children.Add(new CityGroupViewModel("Улицы", this));
            base.Children.Add(new CityGroupViewModel("Районы города", this));
        }
    }

    public class DistrictViewModel : TreeViewItemViewModel
    {
        public DistrictViewModel(District district, RegionViewModel parentRegion)
            : base(parentRegion, true)
        {
            _entity = district;
        }

        public override string Name
        {
            get { return _entity.Name; }
        }

        protected override void LoadChildren()
        {
            foreach (City city in RegionsDatabaseDirectory.GetCities(_entity as District))
                base.Children.Add(new CityViewModel(city, this));
        }
    }

    public class RegionViewModel : TreeViewItemViewModel
    {
        public RegionViewModel(Region region, CountryViewModel parentCountry)
            : base(parentCountry, true)
        {
            _entity = region;
        }

        public override string  Name
        {
            get { return _entity.Name; }
        }

        protected override void LoadChildren()
        {
            foreach (District district in RegionsDatabaseDirectory.GetDistricts(_entity as Region))
                base.Children.Add(new DistrictViewModel(district, this));
        }
    }

    public class CountryViewModel : TreeViewItemViewModel
    {
        public CountryViewModel(Country country)
            : base(null, true)
        {
            this._entity = country;
        }

        protected override void LoadChildren()
        {
            foreach (Region region in RegionsDatabaseDirectory.GetRegions(_entity as Country))
                base.Children.Add(new RegionViewModel(region, this));
        }
    }

    public class CountryGroupViewModel :TreeViewItemViewModel
    {
        public override string Name { get { return "Страны"; } }

        public CountryGroupViewModel()
            : base(null, false)
        {
            this.LoadChildren();
        }

        protected override void LoadChildren()
        {
            foreach (Country c in RegionsDatabaseDirectory.GetCountries())
                base.Children.Add(new CountryViewModel(c) { IsExpanded = true });
        }
    }

    #endregion

    public class RegionsEditorViewModel : INotifyPropertyChanged
    {
        #region State parameters
        public bool IsAddingItem { get; set; }
        public bool IsEditingItem { get; set; }
        public TreeViewItemViewModel CurrentItem { get; set; }
        #endregion

        #region Commnads
        private DelegateCommand<object> InitAddingItemCommand;
        private DelegateCommand<object> InitEditingItemCommand;

        private DelegateCommand<object> _addCommand;
        private DelegateCommand<object> _editCommand;

        public DelegateCommand<object> AddCommand { get { return _addCommand; } }
        public DelegateCommand<object> EditCommand { get { return _editCommand; } }
        #endregion

        #region Data
        readonly ObservableCollection<CountryGroupViewModel> _countryGroups;
        public ObservableCollection<CountryGroupViewModel> CountryGroups
        {
            get { return _countryGroups; }
        }
        #endregion

        #region Constructors
        public RegionsEditorViewModel()
        {
            _addCommand = new DelegateCommand<object>(this.AddItem, delegate { return true; });
            _editCommand = new DelegateCommand<object>(this.EditItem, delegate { return true; });

            InitAddingItemCommand = new DelegateCommand<object>(this.InitAddingItem, delegate { return true; });
            DatabaseDirectoryCommands.InitAddingItem.RegisterCommand(InitAddingItemCommand);

            InitEditingItemCommand = new DelegateCommand<object>(this.InitEditingItem, delegate { return true; });
            DatabaseDirectoryCommands.InitEditingItem.RegisterCommand(InitEditingItemCommand);

            _countryGroups = new ObservableCollection<CountryGroupViewModel>();
            _countryGroups.Add(new CountryGroupViewModel() { IsExpanded = true });
        }
        #endregion

        #region StateEvents

        public void InitAddingItem(object o)
        {
            TreeViewItemViewModel parent = o as TreeViewItemViewModel;
            
            CurrentItem = parent;
            IsAddingItem = true;
            OnPropertyChanged("IsAddingItem");
        }

        public void InitEditingItem(object o)
        {
            TreeViewItemViewModel selectedItem = o as TreeViewItemViewModel;

            CurrentItem = selectedItem;
            IsEditingItem = true;
            OnPropertyChanged("IsEditingItem");
        }

        public void EditItem(object o)
        {
            // FIX: check if contains
            string newItemName = (o as string);

            if (String.IsNullOrWhiteSpace(newItemName)) return;

            CurrentItem.Name = newItemName;
            RegionsDatabaseDirectory.Update(CurrentItem.Entity);
        }

        public void AddItem(object o)
        {
            // FIX: check if contains

            if (CurrentItem == null) return;

            CurrentItem.IsExpanded = true;
            string newItemName = (o as string).Trim();
            if (String.IsNullOrEmpty(newItemName)) return;

            int newRecordId;
            IEntity newEntity;

            if (CurrentItem is CountryGroupViewModel)
            {
                newEntity = new Country(newItemName);
                newRecordId = RegionsDatabaseDirectory.Insert(newEntity, null);
                newEntity.ID = newRecordId;
                CurrentItem.Children.Add(new CountryViewModel(newEntity as Country));
            }

            else if (CurrentItem is CountryViewModel)
            {
                newEntity = new Region(newItemName);
                newRecordId = RegionsDatabaseDirectory.Insert(newEntity, CurrentItem.Entity);
                newEntity.ID = newRecordId;
                CurrentItem.Children.Add(new RegionViewModel(newEntity as Region, CurrentItem as CountryViewModel));
            }

            else if (CurrentItem is RegionViewModel)
            {
                newEntity = new District(newItemName);
                newRecordId = RegionsDatabaseDirectory.Insert(newEntity, CurrentItem.Entity);
                newEntity.ID = newRecordId;
                CurrentItem.Children.Add(new DistrictViewModel(newEntity as District, CurrentItem as RegionViewModel));
            }

            else if (CurrentItem is DistrictViewModel)
            {
                newEntity = new City(newItemName);
                newRecordId = RegionsDatabaseDirectory.Insert(newEntity, CurrentItem.Entity);
                newEntity.ID = newRecordId;
                CurrentItem.Children.Add(new CityViewModel(newEntity as City, CurrentItem as DistrictViewModel));
            }

            else if (CurrentItem is CityGroupViewModel)
            {
                CityGroupViewModel ci = CurrentItem as CityGroupViewModel;
                switch (ci.Name)
                {
                    case "Улицы":
                        newEntity = new Street(newItemName);
                        newRecordId = RegionsDatabaseDirectory.Insert(newEntity, CurrentItem.Parent.Entity);
                        newEntity.ID = newRecordId;
                        CurrentItem.Children.Add(new StreetViewModel(newEntity as Street, ci.Parent as CityViewModel));
                        break;
                    case "Районы города":
                        newEntity = new Borough(newItemName);
                        newRecordId = RegionsDatabaseDirectory.Insert(newEntity, CurrentItem.Parent.Entity);
                        newEntity.ID = newRecordId;
                        CurrentItem.Children.Add(new BoroughViewModel(newEntity as Borough, ci.Parent as CityViewModel));
                        break;
                }
            }

            else if (CurrentItem is BoroughViewModel)
            {
                newEntity = new Microdistrict(newItemName);
                newRecordId = RegionsDatabaseDirectory.Insert(newEntity, CurrentItem.Entity);
                newEntity.ID = newRecordId;

                CurrentItem.Children.Add(new MicrodistrictViewModel(newEntity as Microdistrict, CurrentItem as BoroughViewModel));
            }
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}