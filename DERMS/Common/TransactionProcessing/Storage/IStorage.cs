using System.Collections.Generic;

namespace Common.ComponentStorage
{
    public interface IStorage<T>
    {
        bool AddEntity(T entity);

        bool EntityExists(long globalId);

        List<T> GetAllEntities();

        T GetEntity(long globalId);

        bool ValidateEntity(T entity);
    }
}
