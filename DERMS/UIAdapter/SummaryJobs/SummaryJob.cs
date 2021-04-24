using Common.ComponentStorage;
using System.Collections.Generic;

namespace UIAdapter.SummaryJobs
{
    public abstract class SummaryJob<TSummaryType, TDTOType>
        where TSummaryType : SummaryItem<TDTOType>
    {
        private IStorage<TSummaryType> storage;

        public SummaryJob(IStorage<TSummaryType> storage)
        {
            this.storage = storage;
        }    

        public List<TDTOType> GetAllEntities()
        {
            List<TSummaryType> entities = storage.GetAllEntities();

            return ConvertEntities(entities);
        }

        public TDTOType GetEntity(long globalId)
        {
            TSummaryType entity = storage.GetEntity(globalId);

            if (entity == null)
            {
                return default(TDTOType);
            }

            return entity.CreateDTO();
        }

        private List<TDTOType> ConvertEntities(List<TSummaryType> allEntities)
        {
            List<TDTOType> entities = new List<TDTOType>(allEntities.Count);
            foreach (var entity in allEntities)
            {
                entities.Add(entity.CreateDTO());
            }

            return entities;
        }
    }
}
