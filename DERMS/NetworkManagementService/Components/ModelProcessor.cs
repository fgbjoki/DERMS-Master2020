using Common.AbstractModel;
using Common.GDA;
using Common.ServiceInterfaces.Transaction;
using NetworkManagementService.DataModel.Core;
using NetworkManagementService.ServiceStates;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NetworkManagementService.Components
{
    public enum ModelAccessScope
    {
        ApplyDelta,
        CurrentModel
    }

    public sealed class ModelProcessor : IInsertionComponent, ITransaction, IStorageComponent
    {
        /// <summary>
        /// Defines current state of the service.
        /// It is used to determine which model should be used for GDA queries. 
        /// </summary>
        private ServiceState currentServiceState;

        /// <summary>
		/// Dictionary which contains all data: Key - DMSType, Value - Container. Defines current model.
		/// </summary>
		private Dictionary<DMSType, Container> currentModel;

        /// <summary>
		/// Dictionary which contains all data: Key - DMSType, Value - Container and is used for transactions.
		/// </summary>
        private Dictionary<DMSType, Container> temporaryModel;

        /// <summary>
		/// Dictionary which contains all data: Key - DMSType, Value - Container. Is used to get information about the current model.
        /// Changes between <see cref="temporaryModel"/> and <see cref="currentModel"/> depending on the transaction phase.
		/// </summary>
		private Dictionary<DMSType, Container> currentWorkingModel;

        /// <summary>
        /// Locker used to make sure that <see cref="currentModel"/>, <see cref="temporaryModel"/> and <see cref="currentModel"/> are in valid state.  
        /// </summary>
        private ReaderWriterLockSlim locker;

        /// <summary>
        /// Semaphore to indicate when transaction is finished.
        /// </summary>
        /// <param name="sempahore">Semaphore.</param>
        private Semaphore transactionFinishedSemaphore;

        public ModelProcessor(Semaphore semaphore)
        {
            transactionFinishedSemaphore = semaphore;

            locker = new ReaderWriterLockSlim();

            currentServiceState = new IdleState();

            currentModel = new Dictionary<DMSType, Container>(typeof(DMSType).GetEnumValues().Length);
            currentWorkingModel = currentModel;
        }

        public Dictionary<DMSType, Container> Model
        {
            get
            {
                Dictionary<DMSType, Container> returnDictionary = null;
                locker.EnterReadLock();
                returnDictionary = currentWorkingModel;
                locker.ExitReadLock();

                return returnDictionary;
            }
        }

        public void InsertEntity(ResourceDescription rd)
        {
            if (currentServiceState.CurrentState != ServiceStateEnum.ApplyDelta)
            {
                // LOG
                throw new Exception($"Cannot add entities. Service must be in {ServiceStateEnum.ApplyDelta.ToString()} state, current state {currentServiceState.CurrentState.ToString()}.");
            }

            if (rd == null)
            {
                // LOG "Argument null.";
                return;
            }

            long globalId = rd.Id;

            // LOG "Inserting entity with GID ({0:x16})."

            // check if mapping for specified global id already exists			
            if (EntityExists(globalId, ModelAccessScope.ApplyDelta))
            {
                string message = String.Format("Failed to insert entity because entity with specified GID ({0:x16}) already exists in network model.", globalId);
                // LOG 
                throw new Exception(message);
            }

            try
            {
                // find type
                DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

                Container container = null;

                // get container or create container 
                if (ContainerExists(type, ModelAccessScope.ApplyDelta))
                {
                    container = GetContainer(type, ModelAccessScope.ApplyDelta);
                }
                else
                {
                    container = new Container();
                    temporaryModel.Add(type, container);
                }

                // create entity and add it to container
                IdentifiedObject io = container.CreateEntity(globalId);

                // apply properties on created entity
                if (rd.Properties != null)
                {
                    foreach (Property property in rd.Properties)
                    {
                        // globalId must not be set as property
                        if (property.Id == ModelCode.IDOBJ_GID)
                        {
                            continue;
                        }

                        if (property.Type == PropertyType.Reference)
                        {
                            // if property is a reference to another entity 
                            long targetGlobalId = property.AsReference();

                            if (targetGlobalId != 0)
                            {

                                if (!EntityExists(targetGlobalId, ModelAccessScope.ApplyDelta))
                                {
                                    string message = string.Format("Failed to get target entity with GID: 0x{0:X16}. {1}", targetGlobalId);
                                    throw new Exception(message);
                                }

                                // get referenced entity for update
                                IdentifiedObject targetEntity = GetEntity(targetGlobalId, ModelAccessScope.ApplyDelta);
                                targetEntity.AddReference(property.Id, io.GlobalId);

                                io.SetProperty(property);
                            }
                        }
                        else
                        {
                            io.SetProperty(property);
                        }
                    }
                }

                // LOG "Inserting entity with GID ({0:x16}) successfully finished.", globalId
            }
            catch (Exception ex)
            {
                string message = String.Format("Failed to insert entity (GID = 0x{0:x16}) into model. {1}", rd.Id, ex.Message);
                // LOG
                throw new Exception(message);
            }
        }

        public IdentifiedObject GetEntity(long globalId, ModelAccessScope accessScope = ModelAccessScope.CurrentModel)
        {
            if (EntityExists(globalId, accessScope))
            {
                DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
                IdentifiedObject io = GetContainer(type, accessScope).GetEntity(globalId);

                return io;
            }
            else
            {
                string message = string.Format("Entity  (GID = 0x{0:x16}) does not exist.", globalId);
                throw new Exception(message);
            }
        }

        public bool EntityExists(long globalId, ModelAccessScope accessScope)
        {
            DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            locker.EnterReadLock();
            if (ContainerExists(type, accessScope))
            {
                locker.ExitReadLock();
                Container container = GetContainer(type, accessScope);
                locker.EnterReadLock();

                if (container.EntityExists(globalId))
                {
                    locker.ExitReadLock();
                    return true;
                }
            }

            locker.ExitReadLock();
            return false;
        }

        public void ApplyDeltaPreparation()
        {
            locker.EnterWriteLock();
            currentServiceState = currentServiceState.ApplyDelta(ref currentModel, ref currentWorkingModel, ref temporaryModel);
            locker.ExitWriteLock();
        }

        public void ApplyDeltaFailed()
        {
            locker.EnterWriteLock();
            currentServiceState = currentServiceState.ChangeToIdleState(ref currentModel, ref currentWorkingModel, ref temporaryModel);
            locker.ExitWriteLock();
        }

        public bool Prepare()
        {
            try
            {
                locker.EnterWriteLock();
                currentServiceState = currentServiceState.Prepare(ref currentModel, ref currentWorkingModel, ref temporaryModel);
                locker.ExitWriteLock();
            }
            catch
            {
                // LOG
                transactionFinishedSemaphore.Release();
                locker.ExitWriteLock();
                return false;
            }

            return true;
        }

        public bool Commit()
        {
            try
            {
                locker.EnterWriteLock();
                currentServiceState = currentServiceState.Commit(ref currentModel, ref currentWorkingModel, ref temporaryModel);
                locker.ExitWriteLock();

                transactionFinishedSemaphore.Release();
            }
            catch (Exception e)
            {
                // LOG

                transactionFinishedSemaphore.Release();
                locker.ExitWriteLock();
                return false;
            }

            return true;
        }

        public bool Rollback()
        {
            try
            {
                locker.EnterWriteLock();
                currentServiceState = currentServiceState.Rollback(ref currentModel, ref currentWorkingModel, ref temporaryModel);
                locker.ExitWriteLock();

                transactionFinishedSemaphore.Release();
            }
            catch (Exception e)
            {
                // LOG

                transactionFinishedSemaphore.Release();
                locker.ExitWriteLock();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calculates the number of instances per <see cref="DMSType"/>.
        /// </summary>
        /// <returns><see cref="Dictionary{TKey, TValue}"/> where key is <see cref="DMSType"/> and the value represents the number of instance for given key.</returns>
        public Dictionary<short, int> GetCounters(ModelAccessScope accesScope = ModelAccessScope.ApplyDelta)
        {
            Dictionary<short, int> typesCounters = new Dictionary<short, int>();

            foreach (DMSType type in Enum.GetValues(typeof(DMSType)))
            {
                typesCounters[(short)type] = 0;

                if (temporaryModel.ContainsKey(type))
                {
                    typesCounters[(short)type] = GetContainer(type, accesScope).Count;
                }
            }

            return typesCounters;
        }

        /// <summary>
        /// Gets container of specified type.
        /// </summary>
        /// <param name="type">Type of container.</param>
        /// <returns>Container for specified local id</returns>
        private Container GetContainer(DMSType type, ModelAccessScope accessScope)
        {
            locker.EnterReadLock();
            Dictionary<DMSType, Container> modelDictionary = GetModelBasedOnScope(accessScope);
            if (ContainerExists(type, accessScope))
            {
                locker.ExitReadLock();
                return modelDictionary[type];
            }
            else
            {
                locker.ExitReadLock();
                string message = string.Format("Container does not exist for type {0}.", type);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Checks if container exists in model.
        /// </summary>
        /// <param name="type">Type of container.</param>
        /// <returns>True if container exists, otherwise FALSE.</returns>
        private bool ContainerExists(DMSType type, ModelAccessScope accessScope)
        {
            Dictionary<DMSType, Container> modelDictionary = GetModelBasedOnScope(accessScope);

            if (modelDictionary.ContainsKey(type))
            {
                return true;
            }

            return false;
        }

        private Dictionary<DMSType, Container> GetModelBasedOnScope(ModelAccessScope accessScope = ModelAccessScope.CurrentModel)
        {
            return accessScope == ModelAccessScope.ApplyDelta ? temporaryModel : currentWorkingModel;
        }

        public List<long> GetEntitiesIdByDMSType(DMSType dmsType, ModelAccessScope accessScope = ModelAccessScope.CurrentModel)
        {
            locker.EnterReadLock();
            Dictionary<DMSType, Container> modelDictionary = GetModelBasedOnScope(accessScope);
            locker.ExitReadLock();

            Container container;
            List<long> gids;

            if (modelDictionary.TryGetValue(dmsType, out container))
            {
                gids = container.GetEntitiesGlobalIds();
            }
            else
            {
                gids = new List<long>();
            }

            return gids;
        }
    }
}
