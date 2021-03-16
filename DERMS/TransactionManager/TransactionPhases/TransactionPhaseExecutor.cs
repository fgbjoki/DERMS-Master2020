using Common.Communication;
using Common.Logger;
using Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TransactionManager.TransactionStates;

namespace TransactionManager.TransactionPhases
{
    /// <summary>
    /// Unit resposible for executing each distributed transaction phase.
    /// </summary>
    class TransactionPhaseExecutor : IDisposable
    {
        private static readonly int LOCKER_TIME_OUT = 10000; // 10 seconds

        private TransactionPhase currentPhase;

        private ReaderWriterLockSlim transactionStateLocker;
        private ReaderWriterLock phaseLocker;

        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Task which executes each phase.
        /// </summary>
        private Task executorTask;

        private Semaphore semaphore;

        public TransactionPhaseExecutor(ReaderWriterLockSlim locker)
        {
            transactionStateLocker = locker;
            phaseLocker = new ReaderWriterLock();

            semaphore = new Semaphore(0, 1);

            cancellationTokenSource = new CancellationTokenSource();
            executorTask = new Task(() => ExecutePhases(cancellationTokenSource.Token));
            executorTask.Start();
        }

        /// <summary>
        /// Schedule prepare phase and signalise executor task to start executing.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="transactionStateWrapper"></param>
        public void SchedulePreparePhase(Dictionary<string, WCFClient<ITransaction>> services, TransactionStateWrapper transactionStateWrapper)
        {
            phaseLocker.AcquireWriterLock(LOCKER_TIME_OUT);
            currentPhase = new PrepareTransactionPhase(transactionStateLocker, phaseLocker, transactionStateWrapper, services);
            phaseLocker.ReleaseWriterLock();

            semaphore.Release();
        }

        public void ScheduleRollbackPhase(Dictionary<string, WCFClient<ITransaction>> services, TransactionStateWrapper transactionStateWrapper)
        {
            phaseLocker.AcquireWriterLock(LOCKER_TIME_OUT);
            currentPhase = new RollbackTransactionPhase(transactionStateLocker, phaseLocker, transactionStateWrapper, services);
            phaseLocker.ReleaseWriterLock();

            semaphore.Release();
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            semaphore.Release();
        }

        private void ExecutePhases(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Logger.Instance.Log("[ExecutePhases] Waiting for a new transaction.");
                semaphore.WaitOne();

                if (cancellationToken.IsCancellationRequested)
                {
                    Logger.Instance.Log("[ExecutePhases] Closing executor.");
                    break;
                }

                Logger.Instance.Log("[ExecutePhases] New transaction started.");

                while (currentPhase != null)
                {
                    currentPhase.Execute();
                    currentPhase = currentPhase.NextPhase;
                }

                Logger.Instance.Log("[ExecutePhases] Transaction finished.");
            }
        }
    }
}
