using FieldProcessor.Model;
using System.Collections.Generic;

namespace FieldProcessor.TransactionProcessing.TransactionProcessors
{
    public interface IRemotePointDependentTransactionUnit
    {
        bool Prepare(List<RemotePoint> remotePoints);
        void Commit();
        void Rollback();
        void RelaseTransactionResources();
    }
}
