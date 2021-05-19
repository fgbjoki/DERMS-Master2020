using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public abstract class DistirbutedEnergyResouceDTOCreator<T> : ConductingEquipmentDTOCreator<T>
        where T : DistributedEnergyResourceDTO
    {
        protected override List<ModelCode> GetModelCodes()
        {
            var modelCodes = base.GetModelCodes();
            modelCodes.Add(ModelCode.DER_NOMINALPOWER);

            return modelCodes;
        }

        protected override void PopulateProperties(T dto, ResourceDescription rd)
        {
            base.PopulateProperties(dto, rd);

            dto.NominalActivePower = rd.GetProperty(ModelCode.DER_NOMINALPOWER).AsFloat();
        }
    }
}
