using CalculationEngine.Model.Topology.Transaction;
using Common.ComponentStorage.StorageItemCreator;
using System;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using Common.Logger;

namespace CalculationEngine.TransactionProcessing.StorageItemCreators
{
    public class DiscreteRemotePointStorageItemCreator : StorageItemCreator
    {
        public DiscreteRemotePointStorageItemCreator() : base()
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            DiscreteRemotePoint remotePoint = new DiscreteRemotePoint(rd.Id);

            try
            {
                PopulateBreakerGid(rd, remotePoint);
                PopulateValueProperty(rd, remotePoint);
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType()}] Couldn't create entity. More info: {e.Message}, stack trace: {e.StackTrace}");
                return null;
            }

            return remotePoint;
        }

        public void PopulateValueProperty(ResourceDescription rd, DiscreteRemotePoint remotePoint)
        {
            int value = rd.GetProperty(ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE).AsInt();
            remotePoint.Value = value;
        }

        public void PopulateBreakerGid(ResourceDescription rd, DiscreteRemotePoint remotePoint)
        {
            long breakerGid = rd.GetProperty(ModelCode.MEASUREMENT_PSR).AsReference();
            remotePoint.BreakerGid = breakerGid;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.MEASUREMENTDISCRETE, new List<ModelCode>() { ModelCode.MEASUREMENTDISCRETE_CURRENTVALUE, ModelCode.MEASUREMENT_PSR } }
            };
        }
    }
}
