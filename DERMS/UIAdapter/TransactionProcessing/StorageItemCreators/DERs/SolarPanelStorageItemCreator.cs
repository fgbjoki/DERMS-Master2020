﻿using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;
using UIAdapter.Model.DERs;

namespace UIAdapter.TransactionProcessing.StorageItemCreators.DERs
{
    public class SolarPanelStorageItemCreator : GeneratorStorageItemCreator
    {
        public SolarPanelStorageItemCreator() : base()
        {
        }

        protected override Generator InstantiateGenerator(ResourceDescription rd)
        {
            SolarGenerator solarGenerator = new SolarGenerator(rd.Id);

            return solarGenerator;
        }

        public override Dictionary<DMSType, List<ModelCode>> GetNeededProperties()
        {
            return new Dictionary<DMSType, List<ModelCode>>()
            {
                {
                    DMSType.SOLARGENERATOR,
                    new List<ModelCode>() { ModelCode.IDOBJ_NAME, ModelCode.DER_NOMINALPOWER, ModelCode.GENERATOR_ENERGYSTORAGE }
                },
            };
        }
    }
}
