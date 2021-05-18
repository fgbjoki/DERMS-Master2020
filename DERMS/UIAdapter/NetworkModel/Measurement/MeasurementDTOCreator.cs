using Common.UIDataTransferObject.NetworkModel;
using System.Collections.Generic;
using Common.AbstractModel;
using Common.GDA;

namespace UIAdapter.NetworkModel.Measurement
{
    public abstract class MeasurementDTOCreator<T> : DTOCreator<T>
        where T : MeasurementDTO
    {
        protected override List<ModelCode> GetModelCodes()
        {
            var modelCodes = base.GetModelCodes();
            modelCodes.AddRange(new List<ModelCode>()
            {
                ModelCode.MEASUREMENT_ADDRESS,
                ModelCode.MEASUREMENT_DIRECTION,
                ModelCode.MEASUREMENT_MEASUREMENTYPE
            });

            return modelCodes;
        }

        protected override void PopulateProperties(T dto, ResourceDescription rd)
        {
            base.PopulateProperties(dto, rd);

            dto.Address = rd.GetProperty(ModelCode.MEASUREMENT_ADDRESS).AsInt();
            dto.MeasurementType = (MeasurementType)rd.GetProperty(ModelCode.MEASUREMENT_MEASUREMENTYPE).AsEnum();
            dto.SignalDirection = (SignalDirection)rd.GetProperty(ModelCode.MEASUREMENT_DIRECTION).AsEnum();
        }
    }
}
