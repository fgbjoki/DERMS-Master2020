using System;

namespace Core.Common.Transaction
{
    public class TransactionSteps
    {
        public Func<bool> Prepare { get; set; }
        public Func<bool> Commit { get; set; }
        public Func<bool> Rollback { get; set; }
    }
}
