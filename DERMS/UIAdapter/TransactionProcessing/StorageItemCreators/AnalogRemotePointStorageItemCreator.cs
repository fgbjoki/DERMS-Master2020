﻿using Common.ComponentStorage.StorageItemCreator;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.GDA;
using UIAdapter.Model;

namespace UIAdapter.TransactionProcessing.StorageItemCreators
{
    public class AnalogRemotePointStorageItemCreator : StorageItemCreator
    {
        public AnalogRemotePointStorageItemCreator() : base(CreatePropertiesPerType())
        {
        }

        public override IdentifiedObject CreateStorageItem(ResourceDescription rd, Dictionary<DMSType, List<ResourceDescription>> affectedEntities)
        {
            AnalogRemotePoint remotePoint = new AnalogRemotePoint(rd.Id)
            {
                Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString(),
                Address = rd.GetProperty(ModelCode.MEASUREMENT_ADDRESS).AsInt(),
                MaxValue = rd.GetProperty(ModelCode.MEASUREMENTANALOG_MAXVALUE).AsFloat(),
                MinValue = rd.GetProperty(ModelCode.MEASUREMENTANALOG_MINVALUE).AsFloat()
            };

            remotePoint.Value = rd.GetProperty(ModelCode.MEASUREMENTANALOG_CURRENTVALUE).AsFloat();

            return remotePoint;
        }

        private static Dictionary<DMSType, List<ModelCode>> CreatePropertiesPerType()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                { DMSType.MEASUREMENTANALOG,
                    new List<ModelCode>()
                    {
                        ModelCode.IDOBJ_NAME,
                        ModelCode.MEASUREMENT_ADDRESS,
                        ModelCode.MEASUREMENTANALOG_CURRENTVALUE,
                        ModelCode.MEASUREMENTANALOG_MAXVALUE,
                        ModelCode.MEASUREMENTANALOG_MINVALUE
                    }
                }
            };
        }
    }
}
