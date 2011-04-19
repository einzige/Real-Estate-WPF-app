using System;

namespace ReferenceBook
{
    using System.Collections.Generic;
    using System.Data.SqlServerCe;
    using Infrastructure.Estate;
    using System.Data.SqlTypes;

    public class SimpleEntity : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class EntitiesDatabaseModel : IEntityDatabaseModel
    {
        System.Data.SqlServerCe.SqlCeConnection _connection;
        public string TableName { get; set; }

        public EntitiesDatabaseModel()
        {
            _connection = new SqlCeConnection(DatabaseInfo.connectionString);
        }

        public EntitiesDatabaseModel(string tableName)
        {
            this.TableName = tableName;
            _connection = new SqlCeConnection(DatabaseInfo.connectionString);
        }

        #region IEntityDatabaseModel Members

        public List<IEntity> GetEntities()
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
            command.CommandText = String.Format("SELECT ID, Name FROM {0}", this.TableName);
            SqlCeDataReader r = command.ExecuteReader();
            while (r.Read())
            {
                l.Add(new SimpleEntity() { ID = r.GetInt32(0), Name = r.GetString(1) });
            }
            _connection.Close();

            return l;
        }

        public void UpdateEntity(IEntity param)
        {
            SimpleEntity entity = param as SimpleEntity;

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
                                                                   this.TableName, entity.ID, entity.Name);
            }
            else
            {
                command.CommandText = String.Format("INSERT INTO {0}(Name) VALUES('{1}')",
                                                                   this.TableName, entity.Name);
            }

            command.ExecuteNonQuery();
            _connection.Close();
        }

        public int InsertEntity(IEntity param)
        {
            SimpleEntity entity = param as SimpleEntity;

            if (entity.Name.Length == 0) return -1;
            if (entity == null) throw new Exception("Entity is null at refbooks IEntityDatabaseModel.cs");

            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                throw new Exception("_connection is not in Closed state!!!");
            }

            _connection.Open();
            SqlCeCommand command = _connection.CreateCommand();

            command.CommandText = String.Format("INSERT INTO {0}(Name) VALUES('{1}')",
                                                                   this.TableName, entity.Name);

            command.ExecuteNonQuery();

            // get the last insert id using identity
            command.CommandText = String.Format("SELECT ID FROM {0} WHERE ID = @@Identity", this.TableName);
            int id = (int)command.ExecuteScalar();

            _connection.Close();

            return id;
        }

        public void DeleteEntity(IEntity param)
        {
            SimpleEntity entity = param as SimpleEntity;

            if (entity == null) throw new Exception("Entity is null at RefModel.cs");

            if (_connection.State != System.Data.ConnectionState.Closed)
            {
                throw new Exception("_connection is not in Closed state!!!");
            }

            _connection.Open();
            SqlCeCommand command = _connection.CreateCommand();
            command.CommandText = String.Format("DELETE FROM {0} WHERE ID={1}", this.TableName, entity.ID);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        #endregion
    }
    /*
    public class RegionsModel
    {
        SqlCeConnection _connection;

        public RegionsModel()
        {
            _connection = new SqlCeConnection(DatabaseInfo.connectionString);
        }

        #region REGIONS
        public List<IEntity> GetRegions()
        {
            string tableName = "Regions";
            List<IEntity> l = new List<IEntity>();

            _connection.Open();
            SQLiteCommand command = _connection.CreateCommand();
            command.CommandText = String.Format("SELECT [oid] as ID, Name FROM [{0}]", tableName);

            SQLiteDataReader r = command.ExecuteReader();
            while (r.Read())
            {
                l.Add(new Entity() { ID = r.GetInt64(0), Name = r.GetString(1) });
            }
            _connection.Close();

            return l;
        }

        public long AddNewRegion(string newRegionName)
        {
            if (newRegionName.Trim().Length == 0) return -1;
            _connection.Open();
            SQLiteCommand command = _connection.CreateCommand();
            SQLiteCommand idCommand = new SQLiteCommand("SELECT last_insert_rowid() FROM [Regions]", _connection);

            command.CommandText = String.Format("INSERT INTO [Regions] ([Name]) VALUES ('{0}')", newRegionName);
            command.ExecuteNonQuery();
            long id = (long)idCommand.ExecuteScalar();

            _connection.Close();
            return id;
        }
        #endregion

        public List<Entity> GetReferencedEntities(string tableName, long reference, string referenceFieldName)
        {
            List<Entity> l = new List<Entity>();

            _connection.Open();
            SQLiteCommand command = _connection.CreateCommand();
            command.CommandText = String.Format("SELECT [oid] as ID, [{2}], Name FROM [{0}] WHERE [{2}]={1}", tableName, reference, referenceFieldName);

            using (SQLiteDataReader r = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
            {
                while (r.Read())
                {
                    l.Add(new Entity() { ID = r.GetInt64(0), Name = r.GetString(2) });
                }
                r.Close();
                _connection.Close();
            }
            return l;
        }

        public long AddNewReferencedEntity(string tableName, string newEntityName, long reference, string referenceFieldName)
        {
            _connection.Open();
            SQLiteCommand command = _connection.CreateCommand();
            SQLiteCommand idCommand = new SQLiteCommand(String.Format("SELECT last_insert_rowid() FROM [{0}]", tableName), _connection);

            command.CommandText = String.Format("INSERT INTO [{0}] ([Name], [{3}]) VALUES ('{1}', {2})", tableName, newEntityName, reference, referenceFieldName);
            command.ExecuteNonQuery();
            long id = (long)idCommand.ExecuteScalar();

            _connection.Close();
            return id;
        }

        #region CITIES
        public List<Entity> GetCities(long regionID)
        {
            return GetReferencedEntities("Cities", regionID, "RegionID");
        }

        public long AddNewCity(string newCityName, long regionID)
        {
            return AddNewReferencedEntity("Cities", newCityName, regionID, "RegionID");
        }
        #endregion

        #region DISTRICTS
        public List<Entity> GetDistricts(long cityID)
        {
            return GetReferencedEntities("Districts", cityID, "CityID");
        }

        public long AddNewDistrict(string newDistrictName, long cityID)
        {
            return AddNewReferencedEntity("Districts", newDistrictName, cityID, "CityID");
        }
        #endregion

        #region STREETS
        public List<Entity> GetStreets(long cityID)
        {
            return GetReferencedEntities("Streets", cityID, "CityID");
        }

        public long AddNewStreet(string newStreetName, long cityID)
        {
            return AddNewReferencedEntity("Streets", newStreetName, cityID, "CityID");
        }
        public void ReplaceStreet(Entity updatedStreet)
        {
            _connection.Open();
            SQLiteCommand command = _connection.CreateCommand();

            command.CommandText = String.Format("UPDATE [Streets] SET [Name]='{0}' WHERE [oid]={1}", updatedStreet.Name, updatedStreet.ID);
            command.ExecuteNonQuery();

            _connection.Close();
        }
        #endregion

        #region BOROUGHS
        public List<Entity> GetBoroughs(long districtID)
        {
            return GetReferencedEntities("Boroughs", districtID, "DistrictID");
        }

        public long AddNewBorough(string newBoroughName, long districtID)
        {
            return AddNewReferencedEntity("Boroughs", newBoroughName, districtID, "DistrictID");
        }
        public void ReplaceBorough(Entity updatedBorough)
        {
            _connection.Open();
            SQLiteCommand command = _connection.CreateCommand();

            command.CommandText = String.Format("UPDATE [Boroughs] SET [Name]='{0}' WHERE [oid]={1}", updatedBorough.Name, updatedBorough.ID);
            command.ExecuteNonQuery();

            _connection.Close();
        }
        #endregion
    }
    */
}
