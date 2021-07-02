using Core.Common.AbstractModel;
using Core.Common.Transaction.Storage;
using Core.Common.Transaction.StorageItemCreator;
using Core.Common.Transaction.StorageTransactionProcessor;
using FEPStorage.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FEPStorage.Transaction.StorageTransactionProcessor
{
    public class DiscreteTransactionProcessor : StorageTransactionProcessor<DiscreteRemotePoint>
    {
        private Action<string> log;

        public DiscreteTransactionProcessor(IStorage<DiscreteRemotePoint> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators, Action<string> log) : base(storage, storageItemCreators)
        {
            this.log = log;
        }

        protected override List<DMSType> GetPrimaryTypes()
        {
            return new List<DMSType>() { DMSType.MEASUREMENTDISCRETE };
        }

        protected override void Log(string text)
        {
            log(text);
        }
    }
}
