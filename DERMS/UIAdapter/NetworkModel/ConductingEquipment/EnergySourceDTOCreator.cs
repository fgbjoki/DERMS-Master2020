using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public class EnergySourceDTOCreator : ConductingEquipmentDTOCreator<EnergySourceDTO>
    {
        protected override EnergySourceDTO InstantiateEntity()
        {
            return new EnergySourceDTO();
        }
    }
}
