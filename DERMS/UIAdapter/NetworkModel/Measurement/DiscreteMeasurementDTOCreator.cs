using Common.UIDataTransferObject.NetworkModel;

namespace UIAdapter.NetworkModel.Measurement
{
    public class DiscreteMeasurementDTOCreator : MeasurementDTOCreator<DiscreteMeasurementDTO>
    {
        protected override DiscreteMeasurementDTO InstantiateEntity()
        {
            return new DiscreteMeasurementDTO();
        }
    }
}
