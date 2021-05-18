using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public class BreakerDTOCreator : ConductingEquipmentDTOCreator<BreakerDTO>
    {
        protected override BreakerDTO InstantiateEntity()
        {
            return new BreakerDTO();
        }

        protected override List<ModelCode> GetModelCodes()
        {
            var modelCodes = base.GetModelCodes();
            modelCodes.Add(ModelCode.SWITCH_NORMALOPEN);

            return modelCodes;
        }

        protected override void PopulateProperties(BreakerDTO dto, ResourceDescription rd)
        {
            base.PopulateProperties(dto, rd);

            dto.NormalOpen = rd.GetProperty(ModelCode.SWITCH_NORMALOPEN).AsBool();
        }
    }
}
