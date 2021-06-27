﻿using Core.Common.AbstractModel;
using Core.Common.GDA;
using Common.ServiceInterfaces;
using Core.Common.ServiceInterfaces.Transaction;
using NetworkManagementService.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.ServiceInterfaces.NMS;

namespace NetworkManagementService
{
    public class NetworkModel : INetworkModelDeltaContract, INetworkModelGDAContract, ITransaction
    {
        private ModelResourcesDesc resourcesDescs;

        private IDeltaProcessor deltaProcessor;
        private ITransaction transactionParticipant;
        private INetworkModelGDAContract gdaProcessor;

        public NetworkModel()
        {
            Semaphore deltaWaitSemaphore = new Semaphore(0, 1);

            ModelProcessor modelProcessor = new ModelProcessor(deltaWaitSemaphore);

            transactionParticipant = modelProcessor;
            gdaProcessor = new GDAProcessor(modelProcessor);
            deltaProcessor = new DeltaProcessor(modelProcessor, deltaWaitSemaphore);

            Initialize();
        }

        public Task<UpdateResult> ApplyUpdate(Delta delta)
        {
            return new Task<UpdateResult>(() => deltaProcessor.ApplyDelta(delta));
        }

        public int GetExtentValues(ModelCode entityType, List<ModelCode> propIds)
        {
            return gdaProcessor.GetExtentValues(entityType, propIds);
        }

        public int GetRelatedValues(long source, List<ModelCode> propIds, Association association)
        {
            return gdaProcessor.GetRelatedValues(source, propIds, association);
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

        public bool IteratorRewind(int id)
        {
            return IteratorRewind(id);
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

        public async Task<bool> Prepare()
        {
            return await transactionParticipant.Prepare();
        }

        public async Task<bool> Commit()
        {
            return await transactionParticipant.Commit();
        }

        public async Task<bool> Rollback()
        {
            return await transactionParticipant.Rollback();
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
