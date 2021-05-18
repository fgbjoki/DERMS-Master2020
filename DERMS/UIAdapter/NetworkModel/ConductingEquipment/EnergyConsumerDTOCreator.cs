using Common.UIDataTransferObject.NetworkModel.ConductingEquipment;

namespace UIAdapter.NetworkModel.ConductingEquipment
{
    public class EnergyConsumerDTOCreator : ConductingEquipmentDTOCreator<ConsumerDTO>
    {
        protected override ConsumerDTO InstantiateEntity()
        {
            return new ConsumerDTO();
        }
    }
}
