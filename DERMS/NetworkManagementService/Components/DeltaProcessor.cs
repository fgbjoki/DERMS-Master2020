using Common.GDA;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NetworkManagementService.Components
{
    internal sealed class DeltaProcessor : IDeltaProcessor
    {
        private IInsertionComponent modelController;

        private Semaphore transactionFinishedSempahore;

        public DeltaProcessor(IInsertionComponent modelController, Semaphore sempahore)
        {
            transactionFinishedSempahore = sempahore;
            this.modelController = modelController;
        }

        public UpdateResult ApplyDelta(Delta delta, bool isInitializing = false)
        {

            bool isSuccessful = true;
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
                isSuccessful = false;
                updateResult.Message = e.Message;
                updateResult.Result = ResultType.Failed;
            }
            finally
            {
                if (isSuccessful)
                {
                    if (!isInitializing)
                    {
                        // TODO
                        //  - StartEnlist
                        //  - Enlist
                        //  - Call services to enlist
                        //  - End enlist

                        // isSuccessful
                        
                        //  transactionFinishedSempahore.WaitOne();
                    }
                }

                if (!isSuccessful)
                {
                    modelController.ApplyDeltaFailed();
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
    }
}
