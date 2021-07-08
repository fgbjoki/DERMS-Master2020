using System.Collections.Generic;
using Core.Common.AbstractModel;
using Core.Common.GDA;
using Core.Common.Transaction.StorageTransactionProcessor;
using Core.Common.Transaction.Storage;
using Core.Common.Transaction.StorageItemCreator;
using System;
using Microsoft.ServiceFabric.Data;
using Core.Common.Transaction.Models.FEP.FEPStorage;

namespace FieldProcessor.TransactionProcessing
{
    public class AnalogTransactionProcessor : StorageTransactionProcessor<AnalogRemotePoint>
    {
        //private List<IRemotePointDependentTransactionUnit> remotePointDependentUnits;

        private Action<string> log;

        public AnalogTransactionProcessor(IStorage<AnalogRemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators, Action<string> log/*,*/
            /*params IRemotePointDependentTransactionUnit[] remotePointDependentUnits*/) : base(storage, storageItemCreators)
        {
            this.log = log;
            //this.remotePointDependentUnits = new List<IRemotePointDependentTransactionUnit>(remotePointDependentUnits);
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTANALOG };
        }

        public override bool Commit(IReliableStateManager stateManager)
        {
            bool commited = base.Commit(stateManager);

            if (commited == false)
            {
                return false;
            }

            //remotePointDependentUnits.ForEach(x => x.Commit());
            //remotePointDependentUnits.ForEach(x => x.RelaseTransactionResources());

            return commited;
        }

        public override bool Rollback(IReliableStateManager stateManager)
        {
            //remotePointDependentUnits.ForEach(x => x.Rollback());
            return base.Rollback(stateManager);
        }

        protected override bool AdditionalProcessing(Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            bool isProcessingValid = base.AdditionalProcessing(affectedEntities);

            if (isProcessingValid == false)
            {
                return false;
            }

            return AdditionalProcessing();
        }

        private bool AdditionalProcessing()
        {
            //remotePointDependentUnits.ForEach(x => x.Prepare(preparedObjects.Values.ToList()));

            return true;
        }

        protected override void Log(string text)
        {
            log(text);
        }
    }
}
