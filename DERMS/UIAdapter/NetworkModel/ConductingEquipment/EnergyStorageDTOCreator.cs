using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public class EnergyStorageDTOCreator : DistirbutedEnergyResouceDTOCreator<EnergyStorageDTO>
    {
        protected override EnergyStorageDTO InstantiateEntity()
        {
            return new EnergyStorageDTO();
        }

        protected override List<ModelCode> GetModelCodes()
        {
            var modelCodes = base.GetModelCodes();
            modelCodes.Add(ModelCode.ENERGYSTORAGE_CAPACITY);

            return modelCodes;
        }

        protected override void PopulateProperties(EnergyStorageDTO dto, ResourceDescription rd)
        {
            base.PopulateProperties(dto, rd);

            dto.Capacity = rd.GetProperty(ModelCode.ENERGYSTORAGE_CAPACITY).AsFloat();
        }
    }
}
