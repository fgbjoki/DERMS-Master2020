using System;
using System.Collections.Generic;

namespace Common.ComponentStorage
{
    public interface IStorage<T> : ICloneable
    {
        bool AddEntity(T entity);

        bool EntityExists(long globalId);

        List<T> GetAllEntities();

        T GetEntity(long globalId);

        bool ValidateEntity(T entity);

        void ShallowCopyEntities(IStorage<T> storage);
    }
}
