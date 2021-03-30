using Common.ComponentStorage;
using System.Collections.Generic;

namespace UIAdapter.SummaryJobs
{
    public abstract class SummaryJob<TSummaryType, TDTOType>
        where TSummaryType : SummaryItem<TDTOType>
    {
        private Storage<TSummaryType> storage;

        public SummaryJob(Storage<TSummaryType> storage)
        {

            this.storage = storage;
        }    

        public List<TDTOType> GetAllEntities()
        {
            List<TSummaryType> entities = storage.GetAllEntities();

            return ConvertEntities(entities);
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
