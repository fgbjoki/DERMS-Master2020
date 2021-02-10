using Common.ComponentStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UIAdapter.SummaryJobs
{
    public abstract class SummaryJob<TSummaryType, TDTOType>
        where TSummaryType : SummaryItem<TDTOType>
    {
        private Dictionary<long, TDTOType> entities;
        private ReaderWriterLockSlim locker;

        private Storage<TSummaryType> storage;
        private AutoResetEvent commited;

        private CancellationTokenSource tokenSource;
        private Thread getNewEntitiesWorker;

        public SummaryJob(Storage<TSummaryType> storage)
        {
            locker = new ReaderWriterLockSlim();
            entities = new Dictionary<long, TDTOType>();

            this.storage = storage;
            commited = storage.Commited;

            tokenSource = new CancellationTokenSource();
            getNewEntitiesWorker = new Thread(() => AddNewEntities(tokenSource));
            getNewEntitiesWorker.Start();
        }    

        public List<TDTOType> GetAllEntities()
        {
            locker.EnterReadLock();
            List<TDTOType> entities = this.entities.Values.ToList();
            locker.ExitReadLock();

            return entities;
        }

        private void AddNewEntities(CancellationTokenSource tokenSource)
        {
            while (!tokenSource.IsCancellationRequested)
            {
                commited.WaitOne();

                if (tokenSource.IsCancellationRequested)
                {
                    return;
                }

                List<TSummaryType> allEntities = storage.GetAllEntities();

                locker.EnterWriteLock();

                foreach (var entity in allEntities)
                {
                    if (!entities.ContainsKey(entity.GlobalId))
                    {
                        TDTOType dtoItem = entity.CreateDTO();
                        entities.Add(entity.GlobalId, dtoItem);
                    }
                }

                locker.ExitWriteLock();
            }
        }
    }
}
