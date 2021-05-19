using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public class SolarPanelDTOCreator : DistirbutedEnergyResouceDTOCreator<SolarPanelDTO>
    {
        protected override SolarPanelDTO InstantiateEntity()
        {
            return new SolarPanelDTO();
        }
    }
}
