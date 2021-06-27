using Core.Common.GDA;
using Core.Common.ServiceInterfaces.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Threading;

namespace NetworkManagementService.Components
{
    internal sealed class DeltaProcessor : IDeltaProcessor
    {
        private readonly string serviceName = "NetworkManagementSystem";
        private string serviceUrlForTransaction;

        private IInsertionComponent modelController;

        private ITransactionStarter transactionStarter;

        private Semaphore transactionFinishedSempahore;

        public DeltaProcessor(IInsertionComponent modelController, Semaphore sempahore)
        {
            LoadConfigurationFromAppConfig();
            transactionStarter = new TransactionStarter(serviceName, serviceUrlForTransaction);

            transactionFinishedSempahore = sempahore;
            this.modelController = modelController;
        }

        public UpdateResult ApplyDelta(Delta delta, bool isInitializing = false)
        {

            bool isModelValid = true;
            UpdateResult updateResult = new UpdateResult();

            try
            {
                modelController.ApplyDeltaPreparation();
                // LOG "Applying  delta to network model.";

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
                updateResult.Message = e.Message;
                updateResult.Result = ResultType.Failed;
            }
            finally
            {
                if (isModelValid)
                {
                    if (!isInitializing)
                    {
                        if (transactionStarter.StartTransaction(delta.InsertOperations.Select(x => x.Id)))
                        {
                            transactionFinishedSempahore.WaitOne();
                        }
                        else
                        {
                            modelController.ApplyDeltaFailed();
                            updateResult.Result = ResultType.Failed;
                            updateResult.Message = "Transaction failed.";
                        }
                    }
                }

                if (updateResult.Result == ResultType.Succeeded)
                {
                    string mesage = "Applying delta to network model successfully finished.";
                    // LOG 
                    updateResult.Message = mesage;
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
                if (endpoint.Contract.Equals(typeof(ITransaction).ToString()))
                {
                    transactionAddition = $"/{endpoint.Address.OriginalString}";
                }
            }

            serviceUrlForTransaction = serviceSection.Services[0].Host.BaseAddresses[0].BaseAddress + transactionAddition;
        }
    }
}
