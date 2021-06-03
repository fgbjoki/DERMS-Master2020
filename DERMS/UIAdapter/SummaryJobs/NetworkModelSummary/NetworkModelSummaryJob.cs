using Common.AbstractModel;
using Common.ComponentStorage;
using Common.ServiceInterfaces.UIAdapter.SummaryJobs;
using Common.UIDataTransferObject.NetworkModel;
using System.Collections.Generic;
using UIAdapter.Model.NetworkModel;
using UIAdapter.NetworkModel;

namespace UIAdapter.SummaryJobs.NetworkModelSummary
{
    public class NetworkModelSummaryJob : INetworkModelSummaryJob
    {
        private IStorage<NetworkModelItem> storage;
        private IDTOContainer dtoContainer;

        public NetworkModelSummaryJob(IStorage<NetworkModelItem> storage)
        {
            this.storage = storage;

            dtoContainer = new DTOContainer();
        }

        public List<NetworkModelEntityDTO> GetAllEntities()
        {
            List<NetworkModelItem> entities = storage.GetAllEntities();

            return ConvertEntities(entities);
        }

        public List<NetworkModelEntityDTO> GetAllEntities(List<DMSType> entityTypes)
        {
            List<NetworkModelItem> entities = storage.GetAllEntities();

            entities.RemoveAll(x => !entityTypes.Contains(x.DMSType));

            return ConvertEntities(entities);
        }

        public NetworkModelEntityDTO GetEntity(long globalId)
        {
            return dtoContainer.CreateDTO(globalId);
        }

        private List<NetworkModelEntityDTO> ConvertEntities(List<NetworkModelItem> allEntities)
        {
            List<NetworkModelEntityDTO> entities = new List<NetworkModelEntityDTO>(allEntities.Count);
            foreach (var entity in allEntities)
            {
                entities.Add(entity.CreateDTO());
            }

            return entities;
        }

    }
}
