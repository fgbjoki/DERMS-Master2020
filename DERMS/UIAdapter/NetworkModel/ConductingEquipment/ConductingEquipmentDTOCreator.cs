using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.UIDataTransferObject.NetworkModel;
using Common.GDA;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public abstract class ConductingEquipmentDTOCreator<T> : DTOCreator<T>
        where T : ConductingEquipmentDTO
    {
        protected override void ConnectDependentDTO(T entity, NetworkModelEntityDTO depedencyDTO)
        {
            base.ConnectDependentDTO(entity, depedencyDTO);

            if (entity.Measurements == null)
            {
                entity.Measurements = new List<MeasurementDTO>();
            }

            entity.Measurements.Add(depedencyDTO as MeasurementDTO);
        }

        protected override List<ModelCode> GetModelCodes()
        {
            var modelCodes = base.GetModelCodes();
            modelCodes.Add(ModelCode.PSR_MEASUREMENTS);

            return modelCodes;
        }

        public override List<long> GetDependentEntities(ResourceDescription rd)
        {
            return rd.GetProperty(ModelCode.PSR_MEASUREMENTS).AsReferences();
        }
    }
}
