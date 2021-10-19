using Common.AbstractModel;
using Common.GDA;
using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using System.Collections.Generic;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public class EnergyConsumerDTOCreator : ConductingEquipmentDTOCreator<ConsumerDTO>
    {
        protected override ConsumerDTO InstantiateEntity()
        {
            return new ConsumerDTO();
        }

        protected override List<ModelCode> GetModelCodes()
        {
            var modelCodes = base.GetModelCodes();
            modelCodes.Add(ModelCode.ENERGYCONSUMER_PFIXED);
            modelCodes.Add(ModelCode.ENERGYCONSUMER_TYPE);

            return modelCodes;
        }

        protected override void PopulateProperties(ConsumerDTO dto, ResourceDescription rd)
        {
            base.PopulateProperties(dto, rd);

            dto.Pfixed = rd.GetProperty(ModelCode.ENERGYCONSUMER_PFIXED).AsFloat();
            dto.Type = (DERMS.CustomConsumerType)rd.GetProperty(ModelCode.ENERGYCONSUMER_TYPE).AsEnum();
        }
    }
}
