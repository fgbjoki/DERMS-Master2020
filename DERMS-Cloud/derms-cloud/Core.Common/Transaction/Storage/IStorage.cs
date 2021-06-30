using System;
using System.Collections.Generic;
using System.Threading;

namespace Core.Common.Transaction.Storage
{
    public interface IStorage<T> : ICloneable
    {
        bool AddEntity(T entity);

        bool EntityExists(long globalId);

        List<T> GetAllEntities();

        T GetEntity(long globalId);

        bool ValidateEntity(T entity);

        void UpdateEntityProperty(long entityGid, Predicate<T> predicate);

        void ShallowCopyEntities(IStorage<T> storage);
    }
}
