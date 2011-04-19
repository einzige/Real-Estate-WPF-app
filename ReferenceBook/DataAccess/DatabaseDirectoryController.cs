namespace ReferenceBook
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlServerCe;
    using Infrastructure.Estate;

    public class RegionsDatabaseModel
    {
        System.Data.SqlServerCe.SqlCeConnection _connection;

        #region Constructors
        public RegionsDatabaseModel()
        {
            _connection = new SqlCeConnection(DatabaseInfo.connectionString);
        }
        #endregion

        #region SELECTORS
        public List<IEntity> GetCountries()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                throw new Exception("_connection is not in Closed state!!!");
            }

            List<IEntity> l = new List<IEntity>();

            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message); // здесь ошибка
            }

            SqlCeCommand command = _connection.CreateCommand();

            command.CommandText = String.Format("SELECT ID, Name FROM Countries");

            SqlCeDataReader r = command.ExecuteReader();
            while (r.Read())
            {
                l.Add(new Country(r.GetString(1)) { ID = r.GetInt32(0) });
            }
            _connection.Close();

            return l;
        }

        public List<IEntity> GetReferencedEntities(string tableName, string referenceField, int reference)
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                throw new Exception("_connection is not in Closed state!!!");
            }

            List<IEntity> l = new List<IEntity>();

            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message); // здесь ошибка
            }

            SqlCeCommand command = _connection.CreateCommand();

            command.CommandText = String.Format("SELECT ID, Name FROM {2} WHERE {1} = {0}", reference, referenceField, tableName);

            SqlCeDataReader r = command.ExecuteReader();
            while (r.Read())
            {
                l.Add(new SimpleEntity() { ID = r.GetInt32(0), Name = r.GetString(1) });
            }
            _connection.Close();

            return l;
        }

        public List<IEntity> GetRegions(Country country)
        {
            List<IEntity> res = new List<IEntity>();
            List<IEntity> temp = GetReferencedEntities("Regions", "CountryID", country.ID);

            foreach (SimpleEntity se in temp)
            {
                res.Add(new Region(se.Name) { ID = se.ID });
            }

            return res;
        }

        public List<IEntity> GetDistricts(Region region)
        {
            List<IEntity> res = new List<IEntity>();
            List<IEntity> temp = GetReferencedEntities("Districts", "RegionID", region.ID);

            foreach (SimpleEntity se in temp)
            {
                res.Add(new District(se.Name) { ID = se.ID });
            }

            return res;
        }

        public List<IEntity> GetCities(District district)
        {
            List<IEntity> res = new List<IEntity>();
            List<IEntity> temp = GetReferencedEntities("Cities", "DistrictID", district.ID);

            foreach (SimpleEntity se in temp)
            {
                res.Add(new City(se.Name) { ID = se.ID });
            }

            return res;
        }

        public List<IEntity> GetBoroughs(City city)
        {
            List<IEntity> res = new List<IEntity>();
            List<IEntity> temp = GetReferencedEntities("Boroughs", "CityID", city.ID);

            foreach (SimpleEntity se in temp)
            {
                res.Add(new Borough(se.Name) { ID = se.ID });
            }

            return res;
        }

        public List<IEntity> GetMicrodistricts(Borough borough)
        {
            List<IEntity> res = new List<IEntity>();
            List<IEntity> temp = GetReferencedEntities("Microdistricts", "BoroughID", borough.ID);

            foreach (SimpleEntity se in temp)
            {
                res.Add(new Microdistrict(se.Name) { ID = se.ID });
            }

            return res;
        }

        public List<IEntity> GetStreets(City city)
        {
            List<IEntity> res = new List<IEntity>();
            List<IEntity> temp = GetReferencedEntities("Streets", "CityID", city.ID);

            foreach (SimpleEntity se in temp)
            {
                res.Add(new Street(se.Name) { ID = se.ID });
            }

            return res;
        }
        #endregion

        #region Data Editing
        public void UpdateEntity(IEntity entity)
        {
            string tableName = "error table";

            if (entity is Country)            tableName = "Countries";
            else if (entity is Region)        tableName = "Regions";
            else if (entity is District)      tableName = "Districts";
            else if (entity is City)          tableName = "Cities";
            else if (entity is Borough)       tableName = "Boroughs";
            else if (entity is Microdistrict) tableName = "Microdistricts";
            else if (entity is Street)        tableName = "Streets";

            if (entity.Name.Length == 0) return;
            if (entity == null) throw new Exception("Entity is null at refbooks IEntityDatabaseModel.cs");

            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                throw new Exception("_connection is not in Closed state!!!");
            }

            _connection.Open();
            SqlCeCommand command = _connection.CreateCommand();

            if (entity.ID != 0)
            {
                command.CommandText = String.Format("UPDATE {0} SET Name = '{2}' WHERE ID = {1}",
                                                                   tableName, entity.ID, entity.Name);
            }
            else
            {
                throw new Exception("FUCK OFF ID == 0");
            }

            command.ExecuteNonQuery();
            _connection.Close();
        }

        public void DeleteEntity(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public int InsertCountry(Country country)
        {
            if (country.Name.Length == 0) return -1;
            if (country == null) throw new Exception("Entity is null at refbooks IEntityDatabaseModel.cs");

            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                throw new Exception("_connection is not in Closed state!!!");
            }

            _connection.Open();
            SqlCeCommand command = _connection.CreateCommand();

            command.CommandText = String.Format("INSERT INTO Countries (Name) VALUES('{0}')", country.Name);

            command.ExecuteNonQuery();

            // get the last insert id using identity
            command.CommandText = String.Format("SELECT ID FROM Countries WHERE ID = @@Identity");
            int id = (int)command.ExecuteScalar();

            _connection.Close();

            return id;
        }

        public int InsertEntity (IEntity entity, IEntity parent)
        {
            string tableName = "error table";
            string referenceName = "error ref";

            if (entity is Region) { tableName = "Regions"; referenceName = "CountryID"; }
            else if (entity is District) { tableName = "Districts"; referenceName = "RegionID"; }
            else if (entity is City) { tableName = "Cities"; referenceName = "DistrictID"; }
            else if (entity is Borough) { tableName = "Boroughs"; referenceName = "CityID"; }
            else if (entity is Microdistrict) { tableName = "Microdistricts"; referenceName = "BoroughID"; }
            else if (entity is Street) { tableName = "Streets"; referenceName = "CityID"; }

            if (entity.Name.Length == 0) return -1;
            if (entity == null) throw new Exception("Entity is null at refbooks IEntityDatabaseModel.cs");

            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                throw new Exception("_connection is not in Closed state!!!");
            }

            _connection.Open();
            SqlCeCommand command = _connection.CreateCommand();

            command.CommandText = String.Format("INSERT INTO {0}(Name, {1}) VALUES('{2}', {3})",
                                                                   tableName, referenceName, entity.Name, parent.ID);

            command.ExecuteNonQuery();

            // get the last insert id using identity
            command.CommandText = String.Format("SELECT ID FROM {0} WHERE ID = @@Identity", tableName);
            int id = (int)command.ExecuteScalar();

            _connection.Close();

            return id;
        }
        #endregion
    }

    public class RegionsDatabaseDirectory
    {
        RegionsDatabaseModel _regionsModel;

        #region singleton definion

        protected RegionsDatabaseDirectory() 
        {
            _regionsModel = new RegionsDatabaseModel();
        }

        private sealed class SingletonCreator
        {
            private static readonly RegionsDatabaseDirectory instance = new RegionsDatabaseDirectory();
            public static RegionsDatabaseDirectory Instance { get { return instance; } }
        }

        public static RegionsDatabaseDirectory Instance
        {
            get { return SingletonCreator.Instance; }
        }

        #endregion

        #region Select
        public static List<IEntity> GetCountries()
        {
            return Instance._regionsModel.GetCountries();
        }
        public static List<IEntity> GetRegions(Country c)
        {
            return Instance._regionsModel.GetRegions(c);
        }
        public static List<IEntity> GetDistricts(Region r)
        {
            return Instance._regionsModel.GetDistricts(r);
        }
        public static List<IEntity> GetCities(District d)
        {
            return Instance._regionsModel.GetCities(d);
        }
        public static List<IEntity> GetBoroughs(City c)
        {
            return Instance._regionsModel.GetBoroughs(c);
        }
        public static List<IEntity> GetMicrodistricts(Borough b)
        {
            return Instance._regionsModel.GetMicrodistricts(b);
        }
        public static List<IEntity> GetStreets(City c)
        {
            return Instance._regionsModel.GetStreets(c);
        }
        #endregion

        #region Update
        public static void Update(IEntity e)
        {
            Instance._regionsModel.UpdateEntity(e);
        }
        #endregion

        #region Insert
        public static int Insert(IEntity entity, IEntity parent)
        {
            if (entity is Country) return Instance._regionsModel.InsertCountry(entity as Country);
            return Instance._regionsModel.InsertEntity(entity, parent);
        }
        #endregion
    }

    public class EntitiesDatabaseDirectory
    {
        #region singleton definion

        protected EntitiesDatabaseDirectory() { }

        private sealed class SingletonCreator
        {
            private static readonly EntitiesDatabaseDirectory instance = new EntitiesDatabaseDirectory();
            public static EntitiesDatabaseDirectory Instance { get { return instance; } }
        }

        public static EntitiesDatabaseDirectory Instance
        {
            get { return SingletonCreator.Instance; }
        }

        #endregion

        IEntityDatabaseModel _model;
        Hashtable _register = new Hashtable();

        // FIX:
        // Внимание, здесь мы собираем все использованные справочники в памяти, 
        // но не отчищаем память от них, когда они уже не используются

        #region Wrappers

        public static EntityCollection GetDatabaseDictionary(string tableName)
        {
            return Instance.GetDatabaseDictionaryFromInstance(tableName);
        }

        public static void DeleteEntity(IEntityViewModel e)
        {
            Instance._model.DeleteEntity(e.Entity);
        }

        public static void UpdateEntity(IEntity e)
        {
            Instance._model.UpdateEntity(e);
        }

        public static int InsertEntity(IEntity e)
        {
            return Instance._model.InsertEntity(e);

        }

        public static void UpdateEntity(IEntityViewModel e)
        {
            Instance._model.UpdateEntity(e.Entity);
        }

        private EntityCollection GetDatabaseDictionaryFromInstance(string tableName)
        {
            if (_register.ContainsKey(tableName))
                return (EntityCollection)_register[tableName];

            _model = new EntitiesDatabaseModel(tableName);
            EntityCollection res = new EntityCollection(_model.GetEntities());

            _register.Add(tableName, res);
            return res;
        }
        #endregion
    }
}
