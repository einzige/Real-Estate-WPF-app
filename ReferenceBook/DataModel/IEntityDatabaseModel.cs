using System;

namespace ReferenceBook
{
    using System.Collections.Generic;

    public interface IEntity
    {
        int ID { get; set; }
        string Name { get; set; }
    }

    public interface IEntityDatabaseModel
    {
        string TableName { get; set; }

        List<IEntity> GetEntities();

        void UpdateEntity(IEntity entity);
        void DeleteEntity(IEntity entity);
        int InsertEntity(IEntity entity);
    }
}
