using Common.AbstractModel;
using Common.GDA;
using Common.Logger;
using Common.UIDataTransferObject.NetworkModel;
using System.Collections.Generic;
using UIAdapter.NetworkModel.ConductingEquipment;
using UIAdapter.NetworkModel.Measurement;

namespace UIAdapter.NetworkModel
{
    public class DTOContainer : IDTOContainer
    {
        private Dictionary<DMSType, IDTOCreator> dtoCreators;
        private GDAProxy gdaProxy;

        public DTOContainer()
        {
            gdaProxy = new GDAProxy("gdaQueryEndpoint");
            InitialzieDTOCreators();
        }

        public NetworkModelEntityDTO CreateDTO(long entityGid)
        {
            DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(entityGid);

            IDTOCreator dtoCreator;
            if (!dtoCreators.TryGetValue(dmsType, out dtoCreator))
            {
                Logger.Instance.Log($"[{GetType().Name}] Cannot find dto creator for type: {dmsType}.");
                return null;
            }

            ResourceDescription entityRd = GetEntityFromNMS(entityGid, dtoCreator.NeededModelCodes);
            NetworkModelEntityDTO entityDto = dtoCreator.CreateEntityDTO(entityRd);

            foreach (var depedentGid in dtoCreator.GetDependentEntities(entityRd))
            {
                NetworkModelEntityDTO dependencyDTO = CreateDTO(depedentGid);
                dtoCreator.ConnectDependentDTO(entityDto, dependencyDTO);
            }

            return entityDto;
        }

        private ResourceDescription GetEntityFromNMS(long entityGid, List<ModelCode> modelCodes)
        {
            return gdaProxy.GetValues(entityGid, modelCodes);
        }

        private void InitialzieDTOCreators()
        {
            dtoCreators = new Dictionary<DMSType, IDTOCreator>()
            {
                { DMSType.BREAKER, new BreakerDTOCreator() },
                { DMSType.ENERGYCONSUMER, new EnergyConsumerDTOCreator() },
                { DMSType.ENERGYSOURCE, new EnergySourceDTOCreator() },
                { DMSType.ENERGYSTORAGE, new EnergyStorageDTOCreator() },
                { DMSType.WINDGENERATOR, new WindGeneratorDTOCreator() },
                { DMSType.SOLARGENERATOR, new SolarPanelDTOCreator() },
                { DMSType.MEASUREMENTANALOG, new AnalogMeasurementDTOCreator() },
                { DMSType.MEASUREMENTDISCRETE, new DiscreteMeasurementDTOCreator() },
            };
        }
    }
}
