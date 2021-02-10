using Common.ComponentStorage;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage.StorageItemCreator;
using System.Threading;

namespace UIAdapter.TransactionProcessing.StorageTransactionProcessors
{
    public abstract class SummaryTransactionProcessor<T> : StorageTransactionProcessor<T>
        where T : IdentifiedObject
    {
        private AutoResetEvent commitDone;
        protected SummaryTransactionProcessor(IStorage<T> storage, Dictionary<DMSType, IStorageItemCreator> storageItemCreators, AutoResetEvent commitDone) : base(storage, storageItemCreators)
        {
            this.commitDone = commitDone;
        }

        public override bool Commit()
        {
            bool commited = base.Commit();

            if (commited)
            {
                commitDone.Set();
            }

            return commited;
        }
    }
}
