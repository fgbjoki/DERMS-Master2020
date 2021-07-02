using Core.Common.GDA;
using Core.Common.ReliableCollectionProxy;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;

namespace NetworkManagementService.Components
{
    public sealed class DeltaProcessor : IDeltaProcessor
    {
        private readonly string serviceName = "NetworkManagementSystem";
        private string serviceUrlForTransaction;

        private IInsertionComponent modelController;

        private ITransactionStarter transactionStarter;

        private IReliableStateManager stateManager;

        public DeltaProcessor(IInsertionComponent modelController, IReliableStateManager stateManager)
        {
            LoadConfigurationFromAppConfig();
            transactionStarter = new TransactionStarter(serviceName, serviceUrlForTransaction);

            this.modelController = modelController;
            this.stateManager = stateManager;
        }

        public UpdateResult ApplyDelta(Delta delta, bool isInitializing = false)
        {
            bool isModelValid = true;
            UpdateResult updateResult = new UpdateResult();

            try
            {
                modelController.ApplyDeltaPreparation();

                Dictionary<short, int> typesCounters = modelController.GetCounters();
                Dictionary<long, long> globalIdPairs = new Dictionary<long, long>();
                delta.FixNegativeToPositiveIds(ref typesCounters, ref globalIdPairs);
                updateResult.GlobalIdPairs = globalIdPairs;
                delta.SortOperations();


                foreach (ResourceDescription rd in delta.InsertOperations)
                {
                    modelController.InsertEntity(rd);
                }
            }
            catch (Exception e)
            {
                isModelValid = false;
                updateResult.Message = e.Message + $"\nStackTrace:\n{e.StackTrace}";
                updateResult.Result = ResultType.Failed;

                return updateResult;
            }
            finally
            {
                bool successfulExecution = false;
                if (isModelValid)
                {
                    if (!isInitializing)
                    {
                        if (transactionStarter.StartTransaction(delta.InsertOperations.Select(x => x.Id)))
                        {
                            bool dequeuedItem = false;
                            do
                            {
                                dequeuedItem = ReliableQueueCollectionProxy.Dequeue(stateManager, "transactionQueue", out successfulExecution);

                            } while (!dequeuedItem);

                        }
                        else
                        {
                            modelController.ApplyDeltaFailed();
                            updateResult.Result = ResultType.Failed;
                            updateResult.Message = "Transaction failed.";
                        }
                    }
                }
                if (successfulExecution == true)
                {
                        string mesage = "Applying delta to network model successfully finished.";
                        // LOG 
                        updateResult.Message = mesage;
                }
                else
                {
                    updateResult.Result = ResultType.Failed;
                    updateResult.Message = "Transaction failed.";
                }
            }

            return updateResult;
        }

        private void LoadConfigurationFromAppConfig()
        {
            ServicesSection serviceSection = ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;
            ServiceEndpointElementCollection endpoints = serviceSection.Services[0].Endpoints;
            string transactionAddition = String.Empty;
            for (int i = 0; i < endpoints.Count; i++)
            {
                ServiceEndpointElement endpoint = endpoints[i];
                if (endpoint.Contract.Equals(typeof(Core.Common.ServiceInterfaces.Transaction.ITransaction).ToString()))
                {
                    transactionAddition = $"/{endpoint.Address.OriginalString}";
                }
            }

            serviceUrlForTransaction = serviceSection.Services[0].Host.BaseAddresses[0].BaseAddress + transactionAddition;
        }
    }
}
