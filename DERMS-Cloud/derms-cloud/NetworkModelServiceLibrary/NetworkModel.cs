using Core.Common.AbstractModel;
using Core.Common.GDA;
using Common.ServiceInterfaces;
using NetworkManagementService.Components;
using System;
using System.Collections.Generic;
using Core.Common.ServiceInterfaces.NMS;
using Microsoft.ServiceFabric.Data;

namespace NetworkManagementService
{
    public class NetworkModel : INetworkModelDeltaContract, INetworkModelGDAContract, Core.Common.ServiceInterfaces.Transaction.ITransaction
    {
        private IDeltaProcessor deltaProcessor;
        private Core.Common.ServiceInterfaces.Transaction.ITransaction transactionParticipant;
        private INetworkModelGDAContract gdaProcessor;

        private IReliableStateManager stateManager;

        public NetworkModel(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;

            ModelProcessor modelProcessor = new ModelProcessor(stateManager);

            transactionParticipant = modelProcessor;
            gdaProcessor = new GDAProcessor(modelProcessor, stateManager);
            deltaProcessor = new DeltaProcessor(modelProcessor, stateManager);

            Initialize();
        }

        public UpdateResult ApplyUpdate(Delta delta)
        {
            return deltaProcessor.ApplyDelta(delta);
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
        {
            return gdaProcessor.GetExtentValues(entityType, propIds);
        }

        public ResourceDescription GetValues(long resourceId, List<ModelCode> propIds)
        {
            return gdaProcessor.GetValues(resourceId, propIds);
        }

        public bool IteratorClose(int id)
        {
            return gdaProcessor.IteratorClose(id);
        }

        public List<ResourceDescription> IteratorNext(int n, int id)
        {
            return gdaProcessor.IteratorNext(n, id);
        }

        public int IteratorResourcesLeft(int id)
        {
            return gdaProcessor.IteratorResourcesLeft(id);
        }

        public int IteratorResourcesTotal(int id)
        {
            return gdaProcessor.IteratorResourcesLeft(id);
        }

        public void Dispose()
        {
            
        }

        private void Initialize()
        {
            List<Delta> result = ReadDeltasFromDB();

            foreach (Delta delta in result)
            {
                try
                {
                    deltaProcessor.ApplyDelta(delta, true);
                }
                catch (Exception ex)
                {
                    //CommonTrace.WriteTrace(CommonTrace.TraceError, "Error while applying delta (id = {0}) during service initialization. {1}", delta.Id, ex.Message);
                }
            }
        }

        private List<Delta> ReadDeltasFromDB()
        {
            // TODO QUERY FROM DB
            return new List<Delta>(0);
        }

        public bool Prepare()
        {
            return transactionParticipant.Prepare();
        }

        public bool Commit()
        {
            return transactionParticipant.Commit();
        }

        public bool Rollback()
        {
            return transactionParticipant.Rollback();
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds, List<long> gids)
        {
            return gdaProcessor.GetExtentValues(entityType, propIds, gids);
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds)
        {
            return gdaProcessor.GetExtentValues(dmsType, propIds);
        }

        public int GetExtentValues(DMSType dmsType, List<ModelCode> propIds, List<long> gids)
        {
            return gdaProcessor.GetExtentValues(dmsType, propIds, gids);
        }
    }
}
